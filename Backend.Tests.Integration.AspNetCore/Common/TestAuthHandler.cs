using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AspNetCore.Common;

/// <summary>
/// Fake authentication handler for test purposes.
/// </summary>
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Add all permissions required by all endpoints
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim("permissions", "List Floors"),
            new Claim("permissions", "Create Floors"),
            new Claim("permissions", "Delete Floors"),
            new Claim("permissions", "List Learning Space"),
            new Claim("permissions", "View Learning Space"),
            new Claim("permissions", "Create Learning Space"),
            new Claim("permissions", "Edit Learning Space"),
            new Claim("permissions", "Delete Learning Space"),
            new Claim("permissions", "View Audit")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
