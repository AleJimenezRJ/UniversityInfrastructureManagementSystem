namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// Represents the response for a university deletion operation.
/// </summary>
public record class DeleteUniversityResponse
{
    public string? ErrorMessage { get; init; } = null;

    public DeleteUniversityResponse()
    {
    }

    public DeleteUniversityResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
