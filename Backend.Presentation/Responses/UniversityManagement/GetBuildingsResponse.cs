using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// The response model returned by the GET /building endpoint.
/// Wraps a DTO version of the Building entity to avoid exposing domain objects directly.
/// </summary>
/// <param name="Buildings">The Building DTO that contains a Building.</param>
public record class GetBuildingResponse(List<ListBuildingDto> Buildings);
