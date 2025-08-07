namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response for validating user account creation during signup.
/// </summary>
/// <param name="Version"> The version of the response format.</param>
/// <param name="Action"> The action performed, if applicable.</param>
/// <param name="status"> The HTTP status code of the response, default is "200".</param>
/// <param name="userMessage"> An optional user-friendly message providing additional context or information about the response.</param>
/// <param name="Extension_Roles"> Optional roles assigned to the user, if applicable.</param>
/// <param name="Extension_Permissions"> Optional permissions granted to the user, if applicable.</param>
public record class PostValidateSignupResponse(
    string Version = "1.0.0",
    string? Action = "",
    string status = "200",
    string? userMessage = null,
    string? Extension_Roles = null,
    string? Extension_Permissions = null
);
