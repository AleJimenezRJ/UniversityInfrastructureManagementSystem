namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;

/// <summary>
/// Represents a request to delete a building by its name.
/// </summary>
/// <param name="BuildingId">The unique identifier for the Building</param>
public record class DeleteBuildingRequest(int BuildingId);
