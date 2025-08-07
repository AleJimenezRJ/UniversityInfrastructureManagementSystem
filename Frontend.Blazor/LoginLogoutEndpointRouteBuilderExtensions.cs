using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor;

/// <summary>
/// Provides extension methods for mapping login and logout endpoints in an ASP.NET Core application.
/// </summary>
internal static class LoginLogoutEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps the login and logout endpoints to the specified route builder.
    /// </summary>
    /// <param name="endpoints"> The route builder to which the endpoints will be added.</param>
    /// <returns> The updated route builder with the login and logout endpoints.</returns>
    internal static IEndpointConventionBuilder MapLoginAndLogout(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(string.Empty);

        group.MapGet("/login", (string? returnUrl) => TypedResults.Challenge(GetAuthProperties(returnUrl)))
            .AllowAnonymous();

        // Sign out with both the Cookie and OIDC authentication schemes. Users who have not signed out with the OIDC scheme will
        // automatically get signed back in as the same user the next time they visit a page that requires authentication
        // with no opportunity to choose another account.
        group.MapPost("/logout", ([FromForm] string? returnUrl) => TypedResults.SignOut(GetAuthProperties(returnUrl),
            [CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme]));

        group.MapGet("/logout", (string? returnUrl) => TypedResults.SignOut(GetAuthProperties(returnUrl),
            [CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme]));

        group.MapGet("/reset-password", () =>
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/login" 
            };
            props.Items["policy"] = "B2C_1_password_reset"; 

            return Results.Challenge(props, [OpenIdConnectDefaults.AuthenticationScheme]);
        })
      .AllowAnonymous();


        return group;
    }

    // Prevent open redirects. Non-empty returnUrls are absolute URIs provided by NavigationManager.Uri.
    private static AuthenticationProperties GetAuthProperties(string? returnUrl) =>
        new()
        {
            RedirectUri = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl
        };
}