using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;

/// <summary>
/// Dimensions of the learning component in 3D space.
/// </summary>
public class Dimensions
{
    /// <summary>
    /// Height of the learning component in 3D space.
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// Width of the learning component in 3D space.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Length (depth) of the learning component in 3D space.
    /// </summary>
    public double Length { get; set; }

    /// <summary>
    /// Constructor for the Dimensions class.
    /// </summary>
    /// <param name="height"></param>
    /// <param name="width"></param>
    /// <param name="length"></param>
    public Dimensions(double height, double width, double length)
    {
        Height = height;
        Width = width;
        Length = length;
    }

    /// <summary>
    /// Default constructor for the Dimensions class.
    /// </summary>
    private Dimensions() { }

}

/// <summary>
/// Position of the learning component in 3D space.
/// </summary>
public class Position
{
    /// <summary>
    /// X-coordinate of the learning component in 3D space.
    /// </summary>
    public double X { get; set; }
    /// <summary>
    /// Y-coordinate of the learning component in 3D space.
    /// </summary>
    public double Y { get; set; }
    /// <summary>
    /// Z-coordinate of the learning component in 3D space.
    /// </summary>
    public double Z { get; set; }

    /// <summary>
    /// Constructor for the Position class.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Position(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Default constructor for the Position class.
    /// </summary>
    private Position() { }

}

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
