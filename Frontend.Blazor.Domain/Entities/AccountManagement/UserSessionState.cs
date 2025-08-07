namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;

/// <summary>
/// Represents the state of a user session in the application.
/// </summary>
public class UserSessionState
{
    /// <summary>
    /// Identifies wether the user is authenticated or not.
    /// </summary>
    public bool IsAuthenticated { get; set; } = false;

    /// <summary>
    /// Used for verifying if the user has completed the registration process.
    /// </summary>
    public bool HasCompletedRegistration { get; set; } = false;
}
