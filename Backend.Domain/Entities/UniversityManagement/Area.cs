using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

/// <summary>
/// Represents a thematic or functional area within the theme park.
/// Each area is uniquely identified by a validated name.
/// </summary>
public class Area
{
    /// <summary>
    /// Gets or sets the name of the area.
    /// Uses the <see cref="EntityName"/> value object to ensure validation rules are enforced.
    /// </summary>
    public EntityName? Name { get; set; }

    /// <summary>
    /// Gets or sets the name of the campus this building belongs to (foreign key).
    /// </summary>
    public EntityName CampusName { get; set; } = null!;

    /// <summary>
    /// Navigation property to the Campus entity.
    /// </summary>
    public Campus Campus { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Area"/> class with the specified name.
    /// </summary>
    /// <param name="name">The validated name of the area.</param>
    /// <param name="campus">The campus to which this area belongs.</param>
    public Area(EntityName name, Campus campus)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name), "Area name cannot be null.");
        Campus = campus ?? throw new ArgumentNullException(nameof(campus), "Campus cannot be null.");
    }

    /// <summary>
    /// Private parameterless constructor required by Entity Framework Core.
    /// </summary>
    private Area() { }
}
