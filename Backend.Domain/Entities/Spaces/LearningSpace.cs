using System.Drawing;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;

/// <summary>
/// Represents a learning space in a building of the theme park UCR.
/// </summary>
public class LearningSpace
{
    /// <summary>
    /// Unique identifier for the learning space (database-internal).
    /// </summary>
    public int LearningSpaceId { get; }

    /// <summary>
    /// Unique name of the learning space within the floor.
    /// </summary>
    public EntityName Name { get; set; }

    /// <summary>
    /// Type of the learning space (e.g., classroom, lab, auditorium).
    /// </summary>
    public LearningSpaceType Type { get; set; }

    /// <summary>
    /// Maximum capacity of the people that can fit in the learning space.
    /// </summary>
    public Capacity MaxCapacity { get; set; }

    /// <summary>
    /// Height of the learning space in meters.
    /// </summary>
    public ValueObjects.Spaces.Size Height { get; set; }

    /// <summary>
    /// Width of the learning space in meters.
    /// </summary>
    public ValueObjects.Spaces.Size Width { get; set; }

    /// <summary>
    /// Length of the learning space in meters.
    /// </summary>
    public ValueObjects.Spaces.Size Length { get; set; }

    /// <summary>
    /// Color of the floor in the learning space.
    /// </summary>
    public Colors ColorFloor { get; set; }

    /// <summary>
    /// Color of the walls in the learning space.
    /// </summary>
    public Colors ColorWalls { get; set; }

    /// <summary>
    /// Color of the ceiling in the learning space.
    /// </summary>
    public Colors ColorCeiling { get; set; }

    /// <summary>
    /// Constructor for the LearningSpace class.
    /// </summary>
    /// <param name="name">Name of the learning space (unique within the floor)</param>
    /// <param name="type">Type of the learning space</param>
    /// <param name="maxCapacity">Maximum capacity of the people that can fit in the learning space</param>
    /// <param name="height">Height of the learning space in meters</param>
    /// <param name="width">Width of the learning space in meters</param>
    /// <param name="length">Length of the learning space in meters</param>
    /// <param name="colorFloor">Color of the floor</param>
    /// <param name="colorWalls">Color of the walls</param>
    /// <param name="colorCeiling">Color of the ceiling</param>
    public LearningSpace(EntityName name, LearningSpaceType type, Capacity maxCapacity, ValueObjects.Spaces.Size height, ValueObjects.Spaces.Size width, ValueObjects.Spaces.Size length, Colors colorFloor, Colors colorWalls, Colors colorCeiling)
    {
        Name = name;
        Type = type;
        MaxCapacity = maxCapacity;
        Height = height;
        Width = width;
        Length = length;
        ColorFloor = colorFloor;
        ColorWalls = colorWalls;
        ColorCeiling = colorCeiling;
    }

    /// <summary>
    /// Constructor for the LearningSpace class.
    /// </summary>
    /// <param name="id">Unique identifier for the learning space</param>
    /// <param name="name">Name of the learning space (unique within the floor)</param>
    /// <param name="type">Type of the learning space</param>
    /// <param name="maxCapacity">Maximum capacity of the people that can fit in the learning space</param>
    /// <param name="height">Height of the learning space in meters</param>
    /// <param name="width">Width of the learning space in meters</param>
    /// <param name="length">Length of the learning space in meters</param>
    /// <param name="colorFloor">Color of the floor</param>
    /// <param name="colorWalls">Color of the walls</param>
    /// <param name="colorCeiling">Color of the ceiling</param>
    public LearningSpace(int id, EntityName name, LearningSpaceType type, Capacity maxCapacity, ValueObjects.Spaces.Size height, ValueObjects.Spaces.Size width, ValueObjects.Spaces.Size length, Colors colorFloor, Colors colorWalls, Colors colorCeiling)
    {
        LearningSpaceId = id;
        Name = name;
        Type = type;
        MaxCapacity = maxCapacity;
        Height = height;
        Width = width;
        Length = length;
        ColorFloor = colorFloor;
        ColorWalls = colorWalls;
        ColorCeiling = colorCeiling;
    }
}
