using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;

/// <summary>
/// Represents a request to add a new campus.
/// </summary>
/// <param name="Campus">
/// The data transfer object (DTO) containing the name of the campus to be added.
/// </param>
public record class PostCampusRequest(AddCampusDto Campus);