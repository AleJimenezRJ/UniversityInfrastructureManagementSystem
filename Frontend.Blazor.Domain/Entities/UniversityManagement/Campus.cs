using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;

/// <summary>
/// Represents a university campus, including its identification and related properties.
/// </summary>
public class Campus
{
    /// <summary>
    /// Gets or sets the name of the  campus.
    /// Uses the <see cref="EntityName"/> value object to ensure validation rules are enforced.
    /// </summary>  
    public EntityName? Name { get; set; }

    /// <summary>
    /// Gets or sets the name of the university associated with the entity.
    /// Uses the <see cref="EntityName"/> value object to ensure validation rules are enforced.
    /// </summary>
    public EntityName? UniversityName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the location of the campus.
    /// Uses the <see cref="EntityLocation"/> value object to ensure validation rules are enforced.
    /// </summary>  
    public EntityLocation? Location { get; set; }

    /// <summary>
    /// Navigation property to the University entity.
    /// </summary>
    public University? University { get; set; } = null!;


    /// <summary>
    /// Initializes a new instance of Campus class with specified properties.
    /// <param name="name">Unique name (ID) for the Campus</param>
    /// <param name="location">Location of the campus</param>
    /// <param name="university">University to which this campus belongs</param>
    /// </summary>
    public Campus(EntityName name, EntityLocation location, University university)
    {
        Name = name;
        Location = location;
        University = university;
    }

    /// <summary>
    /// Initializes a new instance of Campus class with specified properties. Added for compatibility with existing code.
    /// <param name="name">Unique name (ID) for the Campus</param>
    /// <param name="location">Location of the campus</param>
    /// <param name="university">University to which this campus belongs</param>
    /// </summary>
    public Campus(EntityName name, EntityLocation location, EntityName university)
    {
        Name = name;
        Location = location;
        UniversityName = university;
    }

    /// <summary>
    /// Private parameterless constructor required by Entity Framework Core.
    /// </summary>
    private Campus() { }
}
