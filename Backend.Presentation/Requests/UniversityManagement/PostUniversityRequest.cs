using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;

/// <summary>
/// Represents a request to add a new university.
/// </summary>
/// <param name="University">
/// The data transfer object (DTO) containing the name of the university to be added.
/// </param>
public record class PostUniversityRequest(AddCampusUniversityDto University);