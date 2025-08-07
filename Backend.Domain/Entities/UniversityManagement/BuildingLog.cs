namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

/// <summary>
/// Represents a log entry for a building within the theme park domain.
/// </summary>
public class BuildingLog
{
    /// <summary>
    ///  Gets or sets the internal identifier used by the database.
    /// </summary>
    public int BuildingsLogInternalId {  get; set; }

    /// <summary>
    /// Gets or sets the name of the building.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the X coordinate value of the building's location.
    /// </summary>
    public decimal X { get; set; }

    /// <summary>
    /// Gets or sets the nullable Y-coordinate value.
    /// </summary>
    public decimal Y { get; set; }

    /// <summary>
    /// Gets or sets the Z-coordinate value, which may be null if the value is not set.
    /// </summary>
    public decimal Z { get; set; }

    /// <summary>
    /// Gets or sets the physical height of the building in meters.
    /// </summary>
    public decimal Height { get; set; }

    /// <summary>
    /// Gets or sets the width of the building.
    /// </summary>
    public decimal Width { get; set; }

    /// <summary>
    /// Gets or sets the length of the building.
    /// </summary>
    public decimal Length { get; set; }

    /// <summary>
    /// Gets or sets the primary color scheme of the building.
    /// </summary>
    public string Color { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the area this building belongs to (foreign key).
    /// </summary>
    public string AreaName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when the building was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; set; }

    /// <summary>
    /// Gets or sets the action performed on the building log entry (e.g., "Create", "Update", "Delete").
    /// </summary>
    public string Action { get; set; } = null!;

}
