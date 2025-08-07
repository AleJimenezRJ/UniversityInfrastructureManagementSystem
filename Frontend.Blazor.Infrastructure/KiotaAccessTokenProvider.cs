using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Kiota.Abstractions.Authentication;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure;

/// <summary>
/// Provides an access token for Kiota API requests by retrieving it from the current HTTP context.
/// </summary>
internal class KiotaAccessTokenProvider : IAccessTokenProvider
{
    /// <summary>
    /// The HTTP context accessor used to retrieve the current HTTP context and access token.
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Validator for allowed hosts, ensuring that the access token is only used for requests to specified hosts.
    /// </summary>
    public AllowedHostsValidator AllowedHostsValidator { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaAccessTokenProvider"/> class.
    /// </summary>
    /// <param name="httpContextAccessor"> The HTTP context accessor used to access the current HTTP context.</param>
    public KiotaAccessTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Asynchronously retrieves an authorization token for the specified URI using the current HTTP context.
    /// </summary>
    /// <param name="uri"> The URI for which the authorization token is requested.</param>
    /// <param name="additionalAuthenticationContext"> Optional additional authentication context that may be used for token retrieval.</param>
    /// <param name="cancellationToken"> An optional cancellation token to cancel the operation.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"> Thrown if the HTTP context is not available or if the access token cannot be retrieved.</exception>
    public async Task<string> GetAuthorizationTokenAsync(
        Uri uri,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException("No HttpContext is available.");
        }

        var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

        if (accessToken is null)
        {
            throw new InvalidOperationException("No token available.");
        }

        return accessToken;
    }
}