namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response returned when a user-role association is retrieved.
/// </summary>
public record class GetByUserAndRoleResponse
{
    /// <summary>
    /// The message indicating the result of the retrieval operation.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetByUserAndRoleResponse"/> class.
    /// </summary>
    /// <param name="message">The message to return to the client.</param>
    public GetByUserAndRoleResponse(string message)
    {
        Message = message;
    }
}

