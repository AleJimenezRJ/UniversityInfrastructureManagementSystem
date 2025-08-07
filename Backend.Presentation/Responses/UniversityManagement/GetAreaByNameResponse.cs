using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// The response model returned by the GET /area endpoint.
/// Wraps a DTO version of the Area entity to avoid exposing domain objects directly.
/// </summary>
/// <param name="Area"></param>
public record class GetAreaByNameResponse(ListAreaDto Area);