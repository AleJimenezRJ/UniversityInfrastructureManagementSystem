using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using UCR.ECCI.PI.ThemePark.Backend.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Authorization;
using UCR.ECCI.PI.ThemePark.Backend.SchemaFilter;

var builder = WebApplication.CreateBuilder(args);


var b2cConfig = builder.Configuration.GetSection("AzureAdB2C");

// Add services to the container
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<LearningComponentDiscriminatorSchemaFilter>();
    options.UseAllOfForInheritance();
    options.SelectSubTypesUsing(baseType =>
    {
        if (baseType == typeof(LearningComponentDto))
            return new[] { typeof(ProjectorDto), typeof(WhiteboardDto) };

        if (baseType == typeof(LearningComponentNoIdDto))
            return new[] { typeof(ProjectorNoIdDto), typeof(WhiteboardNoIdDto) };

        return Enumerable.Empty<Type>();
    });

    // Configure Swagger to use OAuth 2.0 for authentication (with documentation)
    var scheme = new OpenApiSecurityScheme()
    {
        Name = "Authentication",
        Description = "Uses OAuth 2.0 to authenticate requests.",
        Type = SecuritySchemeType.OAuth2,
        In = ParameterLocation.Header,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"https://{b2cConfig["TenantB2C"]}/{b2cConfig["TenantMS"]}/{b2cConfig["SignUpSignInPolicyId"]}/oauth2/v2.0/authorize"),
                TokenUrl = new Uri($"https://{b2cConfig["TenantB2C"]}/{b2cConfig["TenantMS"]}/{b2cConfig["SignUpSignInPolicyId"]}/oauth2/v2.0/token"),
                Scopes = new Dictionary<string, string>
            {
                { $"https://{b2cConfig["TenantMS"]}/ThemePark_webAPI_v2/ApiAccess", "Access API as user" }
            }
            }
        }
    };

    options.AddSecurityDefinition("OAuth2", scheme);

    // Add OpenApi scheme as a security requirement for all endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "OAuth2",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new[] { $"https://{b2cConfig["TenantMS"]}/ThemePark_webAPI_v2/ApiAccess" }
            }
        }
    );
});

builder.Services.AddCleanArchitectureServices(builder.Configuration);


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

// Adds JWT authentication setup with metadata and validation parameters
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.MetadataAddress = $"https://{b2cConfig["TenantB2C"]}/tfp/{b2cConfig["TenantMS"]}/{b2cConfig["SignUpSignInPolicyId"]}/v2.0/.well-known/openid-configuration";
    options.Authority = $"https://{b2cConfig["TenantB2C"]}/tfp/{b2cConfig["TenantMS"]}/{b2cConfig["SignUpSignInPolicyId"]}/v2.0/";
    options.Audience = b2cConfig["ApiClientId"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = $"https://{b2cConfig["TenantB2C"]}/tfp/{b2cConfig["TenantMS"]}/{b2cConfig["SignUpSignInPolicyId"]}/v2.0/",
        ValidateAudience = true,
        ValidAudience = b2cConfig["ApiClientId"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = "name"
    };
});


// Add permission auth
builder.Services.AddSingleton<IAuthorizationHandler, PermissionsHandler>();



// Add Authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiAccess", policy =>
    {
        policy.RequireAuthenticatedUser();
    });

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
            policy.Requirements.Add(new PermissionsRequirement(permission));
        });
    }
});

builder.Services.MapTools();

builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

var app = builder.Build();

// Global exception handler middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        var ex = exceptionHandlerPathFeature?.Error;

        context.Response.StatusCode = ex switch
        {
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            JsonException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var errorResponse = new
        {
            error = ex is BadHttpRequestException || ex is JsonException
                ? "Malformed JSON or invalid request."
                : "An unexpected error occurred.",
            details = ex?.Message
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // Additional authentication parameters for the swagger client.
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ThemePark API v2");
        var b2cConfig = app.Configuration.GetSection("AzureAdB2C");

        options.OAuthClientId(b2cConfig["SwaggerClientId"]);
        options.OAuthUsePkce();
        options.OAuthScopeSeparator(" ");
        options.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
        {
            { "p", b2cConfig["SignUpSignInPolicyId"]! }
        });
    }

    );
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.Run();

public partial class Program { }