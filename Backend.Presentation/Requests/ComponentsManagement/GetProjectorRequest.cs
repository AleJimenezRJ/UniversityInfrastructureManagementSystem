using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;

/// <summary>
/// Represents the response for a request to get projector information.
/// </summary>
/// <param name="Projector"></param>
public record class GetProjectorRequest(ProjectorDto Projector);