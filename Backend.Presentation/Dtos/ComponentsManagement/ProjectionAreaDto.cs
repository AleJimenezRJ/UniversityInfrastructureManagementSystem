namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

/// <summary>
/// DTO that represents the size of the area projected by a projector.
/// </summary>
/// <param name="ProjectedHeight">Height of the projected area in centimeters.</param>
/// <param name="ProjectedWidth">Width of the projected area in centimeters.</param>
public record class ProjectionAreaDto(
    double ProjectedHeight,
    double ProjectedWidth
);