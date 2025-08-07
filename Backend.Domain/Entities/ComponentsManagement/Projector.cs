using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;

/// <summary>
/// Represents a projector in a learning component.
/// </summary>
public class Projector : LearningComponent
{
    /// <summary>
    /// Area of the projector in the learning component.
    /// </summary>
    public Area2D? ProjectionArea { get; set; }

    /// <summary>
    /// Content that the projector will display.
    /// </summary>
    public string? ProjectedContent { get; set; }
    
    /// <summary>
    /// Default constructor for the Projector class.
    /// </summary>
    public Projector() { }

    /// <summary>
    /// Constructor for the Projector class.
    /// </summary>
    /// <param name="projectedContent"></param>
    /// <param name="projectionArea"></param>
    /// <param name="id"></param>
    /// <param name="orientation"></param>
    /// <param name="position"></param>
    /// <param name="dimensions"></param>
    public Projector(
        string projectedContent,
        Area2D projectionArea,
        int id,
        Orientation orientation,
        Coordinates position,
        Dimension dimensions
    ) : base(id, orientation, dimensions, position)
    {
        ProjectionArea = projectionArea;
        ProjectedContent = projectedContent;
    }

    public Projector(
    string projectedContent,
    Area2D projectionArea,
    Orientation orientation,
    Coordinates position,
    Dimension dimensions
) : base(orientation, dimensions, position)
    {
        ProjectionArea = projectionArea;
        ProjectedContent = projectedContent;
    }
}

