namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;


/// <summary>
/// Represents a request to delete a university by its name.
/// </summary>
/// <param name="university">The unique identifier for the university</param>
public record class DeleteUniversityRequest(string UniversityName);
