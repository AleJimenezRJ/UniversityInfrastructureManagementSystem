namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response returned when a user is deleted.
/// </summary>
public record class DeleteUserResponse
{
    /// <summary>
    /// The message indicating the result of the deletion operation.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserResponse"/> class.
    /// </summary>
    /// <param name="message">The message to return to the client.</param>
    public DeleteUserResponse(string message)
    {
        Message = message;
    }
}
