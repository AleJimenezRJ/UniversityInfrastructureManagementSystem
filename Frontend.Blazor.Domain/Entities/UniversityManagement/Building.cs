using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;


/// <summary>
/// Represents a building within the theme park domain.
/// Utilizes value objects to encapsulate the properties of a building.
/// </summary>
public class Building
{
    /// <summary>
    /// Gets or sets the internal identifier used by the database (auto-incremented).
    /// </summary>
    public int? BuildingInternalId { get; set; }

    /// <summary>
    /// Gets or sets the name of the building.
    /// </summary>
    public EntityName? Name { get; set; }

    /// <summary>
    /// Gets or sets the geographical coordinates of the building's location.
    /// </summary>
    public Coordinates? BuildingCoordinates { get; set; }

    /// <summary>
    /// Gets or sets the physical dimensions (width, length, height) of the building in meters.
    /// </summary>
    public Dimension? Dimensions { get; set; }

    /// <summary>
    /// Gets or sets the primary color scheme of the building.
    /// </summary>
    public Colors? Color { get; set; }

    /// <summary>
    /// Gets or sets the name of the area this building belongs to (foreign key).
    /// </summary>
    public EntityName? AreaName { get; set; } = null!;

    /// <summary>
    /// Navigation property to the Area entity.
    /// </summary>
    public Area? Area { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Building"/> class with the specified values.
    /// </summary>
    /// <param name="buildingInternalId">The Id auto-assigned to the building by the user.</param>
    /// <param name="name">The name assigned to the building by the user.</param>
    /// <param name="coordinates">The geographical coordinates representing the building's location.</param>
    /// <param name="dimensions">The dimensions of the building (width, length, and height).</param>
    /// <param name="color">The color or color scheme of the building.</param>
    /// <param name="area"> The area to which this building belongs.</param>
    public Building(int? buildingInternalId, EntityName? name, Coordinates? coordinates, Dimension? dimensions, Colors? color, Area area)
    {
        BuildingInternalId = buildingInternalId;
        Name = name;
        BuildingCoordinates = coordinates;
        Dimensions = dimensions;
        Color = color;
        Area = area;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Building"/> class with the specified values.
    /// </summary>
    /// <param name="name">The name assigned to the building by the user.</param>
    /// <param name="coordinates">The geographical coordinates representing the building's location.</param>
    /// <param name="dimensions">The dimensions of the building (width, length, and height).</param>
    /// <param name="color">The color or color scheme of the building.</param>
    public Building(EntityName? name, Coordinates? coordinates, Dimension? dimensions, Colors? color, EntityName? area)
    {
        Name = name;
        BuildingCoordinates = coordinates;
        Dimensions = dimensions;
        Color = color;
        AreaName = area;
    }

    public Building(EntityName? name, Coordinates? coordinates, Dimension? dimensions, Colors? color, Area area)
    {
        Name = name;
        BuildingCoordinates = coordinates;
        Dimensions = dimensions;
        Color = color;
        Area = area;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Building"/> class.
    /// Required by Entity Framework Core for materialization.
    /// </summary>
    private Building() { }
}