namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response returned when a person is deleted.
/// </summary>
public record class DeletePersonResponse
{
    public string Message { get; }
   
    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePersonResponse"/> class.
    /// </summary>
    /// <param name="message">The message indicating the result of the deletion operation.</param>
    public DeletePersonResponse(string message)
    {
        Message = message;
    }
}