namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services;

/// <summary>
/// Defines the contract for user audit logging services.
/// </summary>
public interface IUserAuditLoggerService
{
    /// <summary>
    /// Logs an action performed by a user asynchronously.
    /// </summary>
    /// <param name="userName"> The username of the user performing the action.</param>
    /// <param name="action"> The action being logged, typically a description of the action performed.</param>
    /// <returns> A task that represents the asynchronous operation.</returns>
    Task LogAsync(string userName, string action);
}