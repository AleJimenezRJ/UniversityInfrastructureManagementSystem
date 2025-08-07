using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

/// <summary>
/// Represents the name of an entity as a value object.
/// Ensures that a valid, non-empty name is provided and does not exceed 100 characters.
/// </summary>
public class EntityName : ValueObject
{
    /// <summary>
    /// Gets the name of the entity.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityName"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the entity. Cannot be null or empty.</param>
    public EntityName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Attempts to create a new instance of <see cref="EntityName"/> from the provided name.
    /// </summary>
    /// <param name="name">The name of the entity to validate and assign.</param>
    /// <param name="entityName">
    /// When this method returns, contains the created <see cref="EntityName"/> if successful; otherwise, null.
    /// </param>
    /// <returns>
    /// <c>true</c> if the building name is valid and the instance is created; otherwise, <c>false</c>.
    /// </returns>
    public static bool TryCreate(string? name, out EntityName? entityName)
    {
        entityName = null;

        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
            return false;

        entityName = new EntityName(name);
        return true;
    }

    /// <summary>
    /// Validates the input and creates a new instance of <see cref="EntityName"/>.
    /// </summary>
    /// <param name="name">The name of the entity provided by the user.</param>
    /// <returns>A valid <see cref="EntityName"/> instance.</returns>
    /// <exception cref="ValidationException">
    /// Thrown when the provided name is null, empty, whitespace, or longer than 100 characters.
    /// </exception>
    public static EntityName Create(string? name)
    {
        if (!TryCreate(name, out EntityName? entityName) || entityName is null)
        {
            throw new ValidationException("The name must be non-empty and no more than 100 characters long.");
        }
        return entityName;
    }

    /// <summary>
    /// Provides the equality components for the value object comparison.
    /// </summary>
    /// <returns>An enumerable containing the components used for equality comparison.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}
