namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response after removing a role-permission association.
/// </summary>
public record class DeleteRolePermissionResponse
{
    /// <summary>
    /// Gets the success message describing the result of the operation.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteRolePermissionResponse"/> class.
    /// </summary>
    /// <param name="message"></param>
    public DeleteRolePermissionResponse(string message)
    {
        Message = message;
    }
}