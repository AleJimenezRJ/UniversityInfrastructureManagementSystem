namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

/// <summary>
/// Data Transfer Object (DTO) that represents a detailed view of a learning space.
/// </summary>
/// <param name="Name">
/// Name of the learning space.
/// </param>
/// <param name="Type">
/// Type of the learning space (e.g., classroom, lab, auditorium).
/// </param>
/// <param name="MaxCapacity">
/// Maximum capacity of the learning space (e.g., number of students).
/// </param>
/// <param name="Height">
/// Height of the learning space in meters.
/// </param>
/// <param name="Width">
/// Width of the learning space in meters.
/// </param>
/// <param name="Length">
/// Length of the learning space in meters.
/// </param>
/// <param name="ColorFloor">
/// Color of the floor in the learning space. Must be a valid predefined color (e.g., 'White', 'Gray').
/// </param>
/// <param name="ColorWalls">
/// Color of the walls in the learning space. Must be a valid predefined color from the allowed set.
/// </param>
/// <param name="ColorCeiling">
/// Color of the ceiling in the learning space. Must be a valid predefined color.
/// </param>
public record class LearningSpaceDto(
    string Name,
    string Type,
    int MaxCapacity,
    double Height,
    double Width,
    double Length,
    string ColorFloor,
    string ColorWalls,
    string ColorCeiling
);
