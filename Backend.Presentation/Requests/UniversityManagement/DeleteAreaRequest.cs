namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;

/// <summary>
/// Represents a request to delete an area by its name.
/// </summary>
/// <param name="area">The unique identifier for the area name</param>
public record class DeleteAreaRequest(string area);