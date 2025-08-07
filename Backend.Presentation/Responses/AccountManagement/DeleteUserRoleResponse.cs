namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response after removing a user-role association.
/// </summary>
public class DeleteUserRoleResponse
{
    /// <summary>
    /// Gets the success message describing the result of the operation.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserRoleResponse"/> class.
    /// </summary>
    /// <param name="message"></param>
    public DeleteUserRoleResponse(string message)
    {
        Message = message;
    }
}