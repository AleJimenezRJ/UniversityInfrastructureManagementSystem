namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response sent to Azure B2C for token enrichment,
/// </summary>
/// <param name="Version"> The version of the response format.</param>
/// <param name="Action"> The action to be taken, typically "Continue".</param>
/// <param name="Extension_Roles"> A comma-separated list of roles assigned to the user.</param>
/// <param name="Extension_Permissions"> A comma-separated list of permissions assigned to the user.</param>
public record class PostTokenEnrichmentResponse(
    string Version = "1.0.0",
    string Action = "Continue",
    string? Extension_Roles = null,
    string? Extension_Permissions = null
);
