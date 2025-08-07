using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// The response model returned by the POST /area endpoint.
/// Wraps a DTO version of the area entity to avoid exposing domain objects directly.
/// </summary>
/// <param name="Area">The Area DTO that contains an area.</param>
public record class PostAreaResponse(AddAreaDto Area);