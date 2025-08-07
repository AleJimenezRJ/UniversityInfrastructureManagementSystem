namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

/// <summary>
/// Represents a 3D position using X, Y, and Z coordinates.
/// </summary>
/// <param name="X">The X-coordinate of the position.</param>
/// <param name="Y">The Y-coordinate of the position.</param>
/// <param name="Z">The Z-coordinate of the position.</param>
public record class PositionDto(double X, double Y, double Z);
