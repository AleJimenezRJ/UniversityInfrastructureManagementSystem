using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request received from Azure B2C for token enrichment,
/// containing the necessary user information to create or enrich a user.
/// </summary>
public record class PostTokenEnrichmentRequest(string Email);
