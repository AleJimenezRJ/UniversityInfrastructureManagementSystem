using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// The response model returned by the POST /area endpoint.
/// Wraps a DTO version of the university entity to avoid exposing domain objects directly.
/// </summary>
/// <param name="University">The University DTO that contains a university.</param>
public record class PostUniversityResponse(UniversityDto University);