namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;

/// <summary>
/// Represents a request to delete a campus by its name.
/// </summary>
/// <param name="campus">The unique identifier for the campus name</param>
public record class DeleteCampusRequest(string campus);
