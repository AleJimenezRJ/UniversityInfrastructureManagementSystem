using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// The response model returned by the GET /area endpoint.
/// Wraps a DTO version of the Area entity to avoid exposing domain objects directly.
/// </summary>
/// <param name="Area">The Area DTO that contains an Area.</param>
public record class GetAreaResponse(List<ListAreaDto> Area);
