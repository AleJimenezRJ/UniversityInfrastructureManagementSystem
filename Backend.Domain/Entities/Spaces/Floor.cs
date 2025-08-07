using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;

/// <summary>
/// Represents a floor in a building of the theme park UCR.
/// </summary>
public class Floor
{
    /// <summary>
    /// Unique identifier for the floor, used by the database (auto-incremented).
    /// </summary>
    public int FloorId { get; }

    /// <summary>
    /// Floor number that identifies the floor within the building.
    /// </summary>
    public FloorNumber Number { get; private set; }

    /// <summary>
    /// Constructor for the floor class.
    /// </summary>
    /// <param name="number">Identifier of the floor number</param>
    public Floor(FloorNumber number)
    {
        Number = number;
    }

    /// <summary>
    /// Constructor for the floor class.
    /// </summary>
    /// <param name="floorId">Unique identifier for the floor, used by the database (auto-incremented)</param>
    /// <param name="number">Identifier of the floor number</param>
    public Floor(int floorId, FloorNumber number)
    {
        FloorId = floorId;
        Number = number;
    }

    /// <summary>
    /// Changes the floor number of the current floor instance.
    /// Private setter is used to ensure that the number can only be changed through this method.
    /// </summary>
    /// <param name="newNumber">The new floor number that the floor will have</param>
    public void ChangeFloorNumber(int newNumber)
    {
        Number = FloorNumber.Create(newNumber);
    }
}
