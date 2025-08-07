using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;

/// <summary>
/// Represents a request to add a new area.
/// </summary>
/// <param name="Area">
/// The data transfer object (DTO) containing the name of the area to be added.
/// </param>
public record class PostAreaRequest(AddAreaDto Area);