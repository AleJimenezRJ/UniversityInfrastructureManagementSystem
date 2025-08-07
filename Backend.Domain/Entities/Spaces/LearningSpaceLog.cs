using System;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;

/// <summary>
/// Represents a log entry for a learning space within the theme park domain.
/// </summary>
public class LearningSpaceLog
{
    /// <summary>
    /// Gets or sets the internal identifier used by the database.
    /// </summary>
    public int LearningSpaceLogInternalId { get; set; }

    /// <summary>
    /// Gets or sets the name of the learning space.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type of the learning space.
    /// </summary>
    public string Type { get; set; } = null!;

    /// <summary>
    /// Gets or sets the maximum capacity of the learning space.
    /// </summary>
    public int MaxCapacity { get; set; }

    /// <summary>
    /// Gets or sets the width in meters.
    /// </summary>
    public decimal Width { get; set; }

    /// <summary>
    /// Gets or sets the height in meters.
    /// </summary>
    public decimal Height { get; set; }

    /// <summary>
    /// Gets or sets the length in meters.
    /// </summary>
    public decimal Length { get; set; }

    /// <summary>
    /// Gets or sets the color of the floor.
    /// </summary>
    public string ColorFloor { get; set; } = null!;

    /// <summary>
    /// Gets or sets the color of the walls.
    /// </summary>
    public string ColorWalls { get; set; } = null!;

    /// <summary>
    /// Gets or sets the color of the ceiling.
    /// </summary>
    public string ColorCeiling { get; set; } = null!;

    /// <summary>
    /// Gets or sets the modification timestamp.
    /// </summary>
    public DateTime ModifiedAt { get; set; }

    /// <summary>
    /// Gets or sets the action performed (e.g., Created, Updated, Deleted).
    /// </summary>
    public string Action { get; set; } = null!;
}
