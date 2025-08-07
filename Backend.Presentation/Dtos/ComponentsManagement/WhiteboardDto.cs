namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

/// <summary>
/// Data Transfer Object (DTO) for a whiteboard component.
/// Inherits from <see cref="LearningComponentDto"/>.
/// </summary>
/// <param name="Id">The unique identifier of the whiteboard.</param>
/// <param name="Orientation">The orientation of the whiteboard.</param>
/// <param name="Position">The location of the whiteboard within the learning space.</param>
/// <param name="Dimensions">The size of the whiteboard.</param>
/// <param name="MarkerColor">The color of the marker assigned to the whiteboard.</param>
public record WhiteboardDto(
    int Id,
    string Orientation,
    PositionDto Position,
    DimensionsDto Dimensions,
    string MarkerColor
) : LearningComponentDto(Id, Orientation, Position, Dimensions);
