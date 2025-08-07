namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response returned when a role is deleted.
/// </summary>
public record class DeleteRoleResponse
{
    /// <summary>
    /// Gets the success message describing the result of the operation.
    /// </summary>
    public string Message { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteRoleResponse"/> class.
    /// </summary>
    /// <param name="message">The message indicating the result of the deletion operation.</param>
    public DeleteRoleResponse(string message)
    {
        Message = message;
    }
}