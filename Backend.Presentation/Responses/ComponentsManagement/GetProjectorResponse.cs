using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

/// <summary>
/// Response wrapper for a projector component.
/// </summary>
/// <param name="Projectors">The list of DTO containing projector-specific data.</param>
public record class GetProjectorResponse(List<ProjectorDto> Projectors);
