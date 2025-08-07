using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

/// <summary>
/// Represents a value object for a physical location within the theme park.
/// </summary>
public class EntityLocation : ValueObject
{
    /// <summary>
    /// Gets or sets the string representation of the location.
    /// Must be a non-empty string with a maximum length of 100 characters.
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityLocation"/> class with the specified location string.
    /// </summary>
    /// <param name="locate">The string value representing the location.</param>
    public EntityLocation(string location)
    {
        Location = location;
    }

    /// <summary>
    /// Tries to create a new <see cref="Location"/> instance from the given string.
    /// </summary>
    /// <param name="locate">The input string to be validated and used as a location.</param>
    /// <param name="physicalLocation">When this method returns, contains the created <see cref="Location"/> if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the location string is valid and a <see cref="Location"/> was created; otherwise, <c>false</c>.</returns>
    public static bool TryCreate(string? location, out EntityLocation? entityLocation)
    {
        entityLocation = null;

        if (string.IsNullOrWhiteSpace(location) || location.Length > 100)
            return false;

        entityLocation = new EntityLocation(location);
        return true;
    }

    /// <summary>
    /// Creates a new <see cref="Location"/> instance after validating the input string.
    /// </summary>
    /// <param name="locate">The input string to be validated and used as a location.</param>
    /// <returns>A valid <see cref="Location"/> object.</returns>
    /// <exception cref="ValidationException">Thrown if the input string is null, empty, or exceeds 100 characters.</exception>
    public static EntityLocation Create(string? location)
    {
        if (!TryCreate(location, out EntityLocation? entityLocation) || entityLocation is null)
        {
            throw new ValidationException("The location must be non-empty and no more than 100 characters long.");
        }
        return entityLocation;
    }

    /// <summary>
    /// Returns the atomic values of the object used for equality comparison.
    /// </summary>
    /// <returns>An enumerable of objects used to compare equality.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Location;
    }
}
