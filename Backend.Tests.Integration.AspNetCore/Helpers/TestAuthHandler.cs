using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Helpers;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var permissionsHeader = Request.Headers["X-Test-Permissions"].FirstOrDefault();

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, "Test User")
    };

        if (!string.IsNullOrWhiteSpace(permissionsHeader))
        {
            claims.Add(new Claim("extension_Permissions", permissionsHeader));
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

}

