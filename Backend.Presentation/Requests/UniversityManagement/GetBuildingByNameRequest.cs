using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;

/// <summary>
/// Represents a request to retrieve a building by its ID.
/// </summary>
/// <param name="Building">
/// The data transfer object (DTO) containing the attributes of the building being retrieved.
/// </param>
public record class GetBuildingByNameRequest(ListBuildingDto Building);