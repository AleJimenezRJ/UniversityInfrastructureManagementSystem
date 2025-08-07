namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// Represents the response for an area deletion operation.
/// </summary>
public record class DeleteAreaResponse
{
    public string? ErrorMessage { get; init; } = null;

    public DeleteAreaResponse()
    {
    }

    public DeleteAreaResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
