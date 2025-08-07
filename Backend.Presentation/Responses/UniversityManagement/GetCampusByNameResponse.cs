using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// The response model returned by the GET /campus endpoint.
/// Wraps a DTO version of the Campus entity to avoid exposing domain objects directly.
/// </summary>
public record class GetCampusByNameResponse(ListCampusDto Campus);
