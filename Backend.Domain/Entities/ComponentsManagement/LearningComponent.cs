using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;

/// <summary>
/// Represents a learning component in a learning space.
/// </summary>
public abstract class LearningComponent
{
    /// <summary>
    /// Unique identifier for the learning component.
    /// </summary>
    public int ComponentId { get; set; }

    /// <summary>
    /// Orientation of the learning component.
    /// </summary>
    public Orientation Orientation { get; set; } = null!;

    /// <summary>
    /// Dimensions of the learning component in 3D space.
    /// </summary>
    public Dimension Dimensions { get; set; } = null!;

    /// <summary>
    /// Position of the learning component in 3D space.
    /// </summary>
    public Coordinates Position { get; set; } = null!;

    /// <summary>
    /// Indicates whether the component has been logically deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Represents a formatted identifier for the learning component, used for display or external communication.
    /// </summary>
    public string DisplayId { get; set; } = null!;

    /// <summary>
    /// Constructor for the LearningComponent class.
    /// </summary>
    /// <param name="componentId">Unique identifier for the learning component.</param>
    /// <param name="orientation">Specifies the directional orientation of the component (North, South, East, or West).</param>
    /// <param name="dimensions">The 3D dimensions (height, width, length) of the learning component.</param>
    /// <param name="position">The 3D coordinates (X, Y, Z) defining the position of the learning component in space.</param>
    protected LearningComponent(int componentId, Orientation orientation, Dimension dimensions, Coordinates position)
    {
        ComponentId = componentId;
        Orientation = orientation;
        Dimensions = dimensions;
        Position = position;
    }

    /// <summary>
    /// Constructor for the LearningComponent class.
    /// </summary>
    /// <param name="componentId">Unique identifier for the learning component.</param>
    /// <param name="orientation">Specifies the directional orientation of the component (North, South, East, or West).</param>
    /// <param name="dimensions">The 3D dimensions (height, width, length) of the learning component.</param>
    /// <param name="position">The 3D coordinates (X, Y, Z) defining the position of the learning component in space.</param>
    /// <param name="displayId">The formatted id of the learning component.</param>
    protected LearningComponent(int componentId, Orientation orientation, Dimension dimensions, Coordinates position, string displayId)
    {
        ComponentId = componentId;
        Orientation = orientation;
        Dimensions = dimensions;
        Position = position;
        DisplayId = displayId;
    }
    
    protected LearningComponent(Orientation orientation, Dimension dimensions, Coordinates position)
    {
        Orientation = orientation;
        Dimensions = dimensions;
        Position = position;
    }

    /// <summary>
    /// Default constructor for the LearningComponent class.
    /// </summary>
    protected LearningComponent() { }
}
