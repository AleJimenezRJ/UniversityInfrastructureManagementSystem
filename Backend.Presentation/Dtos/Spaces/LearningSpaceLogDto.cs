namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

/// <summary>
/// Data Transfer Object that represents a historical log entry for a learning space.
/// Contains dimensional, visual, and metadata information about changes made to a learning space.
/// </summary>
/// <param name="LearningSpaceLogInternalId">Unique internal identifier for the log entry.</param>
/// <param name="LearningSpaceId">Identifier of the original learning space that was modified.</param>
/// <param name="Name">Name of the learning space at the time of the logged change.</param>
/// <param name="MaxCapacity">Maximum capacity at the time of the change.</param>
/// <param name="Type">Type of the learning space (e.g., Classroom, Lab).</param>
/// <param name="Width">Width of the space in meters.</param>
/// <param name="Height">Height of the space in meters.</param>
/// <param name="Length">Length of the space in meters.</param>
/// <param name="ColorFloor">Color of the floor.</param>
/// <param name="ColorWalls">Color of the walls.</param>
/// <param name="ColorCeiling">Color of the ceiling.</param>
/// <param name="FloorId">Identifier of the floor where the space is located.</param>
/// <param name="ModifiedAt">Timestamp indicating when the modification or action took place.</param>
/// <param name="Action">Type of action performed (e.g., "CREATED", "UPDATED", "DELETED").</param>
public record class LearningSpaceLogDto(
    int LearningSpaceLogInternalId,
    string Name,
    int? MaxCapacity,
    string Type,
    decimal? Width,
    decimal? Height,
    decimal? Length,
    string ColorFloor,
    string ColorWalls,
    string ColorCeiling,
    DateTime ModifiedAt,
    string Action
);
