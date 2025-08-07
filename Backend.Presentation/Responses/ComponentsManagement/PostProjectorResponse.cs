using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

/// <summary>
/// Represents a response containing a single projector.
/// </summary>
/// <param name="Projector"></param>
public record class PostProjectorResponse(ProjectorNoIdDto Projector);
