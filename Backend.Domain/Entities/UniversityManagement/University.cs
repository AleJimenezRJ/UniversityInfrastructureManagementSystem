using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

/// <summary>
/// Represents a university, including its name and country of location.
/// </summary>
public class University
{
    /// <summary>
    /// Gets or sets the name of the University.
    /// Uses the <see cref="EntityName"/> value object to ensure validation rules are enforced.
    /// </summary>
    public EntityName? Name { get; set; }

    public EntityLocation? Country { get; set; }

    /// <summary>
    /// Initializes a new instance of University class with specified properties.
    /// <param name="name">Unique name (ID) for the University</param>
    /// <param name="country">The country where the university is located</param>
    /// </summary>
    public University(EntityName name, EntityLocation country)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Country = country ?? throw new ArgumentNullException(nameof(country));
    }

    public University(EntityName name)
    {
        Name = name;
    }

    public University() { }
}