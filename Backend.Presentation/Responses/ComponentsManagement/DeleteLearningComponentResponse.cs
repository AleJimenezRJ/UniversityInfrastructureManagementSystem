namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

/// <summary>
/// Represents the response for a delete whiteboard operation.
/// </summary>
public record class DeleteLearningComponentResponse
{
    /// <summary>
    /// Contains an error message if the delete operation fails.
    /// </summary>
    public string? ErrorMessage { get; init; } = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLearningComponentResponse"/> class with no error message.
    /// </summary>
    public DeleteLearningComponentResponse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLearningComponentResponse"/> class with a specified error message.
    /// </summary>
    /// <param name="errorMessage">The error message describing the failure.</param>
    public DeleteLearningComponentResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
