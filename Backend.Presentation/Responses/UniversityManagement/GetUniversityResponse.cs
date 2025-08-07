using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// The response model returned by the GET /list-university endpoint. 
/// Contains a list of the information of the universities in the system.
/// </summary>
/// <param name="University"></param>
public record class GetUniversityResponse(List<UniversityDto> University);
