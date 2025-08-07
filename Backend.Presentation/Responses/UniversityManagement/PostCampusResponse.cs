using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// The response model returned by the POST /area endpoint.
/// Wraps a DTO version of the campus entity to avoid exposing domain objects directly.
/// </summary>
/// <param name="Campus">The Campus DTO that contains a campus.</param>
public record class PostCampusResponse(AddCampusDto Campus);