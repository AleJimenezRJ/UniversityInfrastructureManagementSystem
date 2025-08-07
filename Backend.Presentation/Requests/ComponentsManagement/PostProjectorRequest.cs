using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;

/// <summary>
/// Represents a request to create a new projector.
/// </summary>
/// <param name="Projector"></param>
public record class PostProjectorRequest(ProjectorNoIdDto Projector);

