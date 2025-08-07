namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// Represents the response for a building deletion operation.
/// </summary>
public record class DeleteBuildingResponse
{
    public string? ErrorMessage { get; init; } = null;

    public DeleteBuildingResponse()
    {
    }

    public DeleteBuildingResponse(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
