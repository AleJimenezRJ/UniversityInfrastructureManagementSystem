using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;

/// <summary>
/// Represents a request to display a new building.
/// </summary>
/// <param name="Building">
/// The data transfer object (DTO) containing the attributes of the building to be added.
/// </param>
public record class GetBuildingRequest(ListBuildingDto Building);