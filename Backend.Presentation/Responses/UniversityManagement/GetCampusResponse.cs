using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// The response model returned by the GET /list-campus endpoint.
/// Returns a list of Dtos containing the information from the campus entities
/// in the system.
/// </summary>
/// <param name="Campus">The Campus list that contains campus Dtos.</param>
public record class GetCampusResponse(List<ListCampusDto> Campus);
