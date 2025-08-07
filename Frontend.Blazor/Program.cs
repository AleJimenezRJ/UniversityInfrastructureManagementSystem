using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Components;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.DependencyInjection;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization(options => options.SerializeAllClaims = true);

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(msIdentityOptions =>
    {
        msIdentityOptions.CallbackPath = "/signin-oidc";
        msIdentityOptions.ClientId = "07270b25-0842-4eca-a37f-2149c9c17340";
        msIdentityOptions.Domain = "ucrpiis.onmicrosoft.com";
        msIdentityOptions.ResponseType = "code";
        msIdentityOptions.TenantId = "43566108-6c78-488a-8259-a6fbfea31b93";
        msIdentityOptions.Instance = "https://ucrpiis.b2clogin.com/";
        msIdentityOptions.SignUpSignInPolicyId = "B2C_1_signupsignin2";
        msIdentityOptions.ClientSecret = builder.Configuration.GetSection("ClientSecret").Value;
        msIdentityOptions.SaveTokens = true;
        msIdentityOptions.Scope.Add("https://ucrpiis.onmicrosoft.com/ThemePark_webAPI_v2/ApiAccess");
        // Prompt the user to log in every time they access the application
        msIdentityOptions.Prompt = "login";
        // Configure the OpenID Connect events to handle remote failures
        msIdentityOptions.Events = new OpenIdConnectEvents
        {
            OnRemoteFailure = context =>
            {
                var fromQuery = context.Request.Query["error_description"].ToString();
                var fromException = context.Failure?.Message;

                if ((fromQuery?.Contains("AADB2C90118", StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (fromException?.Contains("AADB2C90118", StringComparison.OrdinalIgnoreCase) ?? false))
                {
                    context.Response.Redirect("/reset-password");
                    context.HandleResponse();
                    return Task.CompletedTask;
                }

                if ((fromQuery?.Contains("AADB2C90091", StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (fromException?.Contains("AADB2C90091", StringComparison.OrdinalIgnoreCase) ?? false))
                {

                    context.Response.Redirect("/logout?returnUrl=/login");
                    context.HandleResponse();
                    return Task.CompletedTask;
                }

                context.Response.Redirect("/auth-error");
                context.HandleResponse();
                return Task.CompletedTask;
            },

            OnRedirectToIdentityProvider = context =>
            {
                if (context.Properties.Items.TryGetValue("policy", out var policy))
                {
                    context.ProtocolMessage.IssuerAddress = context.ProtocolMessage.IssuerAddress
                        .Replace("B2C_1_signupsignin2", policy);
                }

                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                var policy = context.Properties.Items.TryGetValue("policy", out var value) ? value : null;

                if (policy == "B2C_1_password_reset")
                {
                    context.Response.Redirect("/login?reset=ok");
                    context.HandleResponse();
                }

                return Task.CompletedTask;
            }
        };
    })
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddDownstreamApi("BackendApi", configOptions =>
    {
        configOptions.BaseUrl = "https://localhost:7111";
        configOptions.Scopes = ["https://ucrpiis.onmicrosoft.com/ThemePark_webAPI_v2/ApiAccess"];
    })
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization(options =>
{
    var permissions = new[]
    {
        "View Users", "Create Users", "Delete Users", "Edit Users",
        "List Buildings", "View Specific Building", "Edit Buildings", "Create Buildings", "Delete Buildings",
        "List Universities", "View Specific University", "Delete Universities", "Create Universities",
        "List Components", "View Specific Component", "Edit Components", "Create Components", "Delete Components",
        "Delete Areas", "Delete Campus", "Create Campus", "Create Area",
        "List Areas", "List Campus", "View Specific Area", "View Specific Campus",
        "List Floors", "Create Floors", "Delete Floors",
        "List Learning Space", "View Learning Space", "Create Learning Space", "Edit Learning Space", "Delete Learning Space",
        "View Audit"
    };

    foreach (var permission in permissions)
    {
        options.AddPolicy(permission, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireAssertion(context =>
            {
                var claim = context.User.FindFirst("extension_Permissions");
                return claim is not null && claim.Value.Split(',').Contains(permission);
            });
        });
    }

    // Add policy for users with at least one permission
    options.AddPolicy("AtLeastOnePermission", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireAssertion(context =>
        {
            var claim = context.User.FindFirst("extension_Permissions");
            return claim is not null && !string.IsNullOrEmpty(claim.Value);
        });
    });
});
builder.Services.AddCleanArchitectureServices(builder.Configuration);
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(UCR.ECCI.PI.ThemePark.Frontend.Blazor.Client._Imports).Assembly,
        typeof(UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation._Imports).Assembly);

app.MapLoginAndLogout();

app.Run();
