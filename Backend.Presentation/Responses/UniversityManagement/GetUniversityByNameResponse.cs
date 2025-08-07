using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// The response model returned by the GET /university endpoint.
/// Wraps a DTO version of the University entity to avoid exposing domain objects directly.
/// </summary>
public record class GetUniversityByNameResponse(UniversityDto University);
