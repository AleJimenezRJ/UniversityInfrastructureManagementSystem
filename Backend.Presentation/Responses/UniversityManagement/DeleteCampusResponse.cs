namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// Represents the response for a campus deletion operation.
/// </summary>
public record class DeleteCampusResponse
{
    public string? ErrorMessage { get; init; } = null;

    public DeleteCampusResponse()
    {
    }

    public DeleteCampusResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
