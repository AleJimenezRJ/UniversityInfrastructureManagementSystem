namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

/// <summary>
/// Represents the physical dimensions of an object in 3D space.
/// </summary>
/// <param name="Height">The height of the object.</param>
/// <param name="Width">The width of the object.</param>
/// <param name="Length">The length (depth) of the object.</param>
public record class DimensionsDto(double Width, double Length, double Height);
