namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
/// <summary>
/// Data Transfer Object (DTO) for a projector component.
/// Inherits from <see cref="LearningComponentDto"/>.
/// </summary>
/// <param name="Orientation">The direction the projector is facing.</param>
/// <param name="Position">The location of the projector in the learning space.</param>
/// <param name="Dimensions">The physical dimensions of the projector.</param>
/// <param name="ProjectionArea">The size of the area where the projector displays its content.</param>
/// <param name="ProjectedContent">The type of content the projector displays (e.g., image, video).</param>
public record class ProjectorNoIdDto(
    string Orientation,
    PositionDto Position,
    DimensionsDto Dimensions,
    ProjectionAreaDto ProjectionArea,
    string ProjectedContent
) : LearningComponentNoIdDto(Orientation, Position, Dimensions);