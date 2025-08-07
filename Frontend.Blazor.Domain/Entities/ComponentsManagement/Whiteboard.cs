using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;

/// <summary>
/// Represents a whiteboard in a learning component.
/// </summary>
public class Whiteboard : LearningComponent
{
    /// <summary>
    /// Color identifier for the marker used on the whiteboard.
    /// </summary>
    public Colors? MarkerColor { get; set; }

    /// <summary>
    /// Default constructor for the Whiteboard class.
    /// </summary>
    public Whiteboard() { }

    /// <summary>
    /// Constructor for the Whiteboard class.
    /// </summary>
    /// <param name="markerColor"></param>
    /// <param name="id"></param>
    /// <param name="orientation"></param>
    /// <param name="position"></param>
    /// <param name="dimensions"></param>
    public Whiteboard(
        Colors markerColor,
        int id,
        Orientation orientation,
        Coordinates position,
        Dimension dimensions
    ) : base(id, orientation, dimensions, position)
    {
        MarkerColor = markerColor;
    }

    public Whiteboard(
    Colors markerColor,
    Orientation orientation,
    Coordinates position,
    Dimension dimensions
) : base(orientation, dimensions, position)
    {
        MarkerColor = markerColor;
    }
}
