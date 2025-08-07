using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.ComponentsManagement;

/// <summary>
/// Uses the <see cref="ValueObject"/> base class to represent a 2D area with length and height.
/// Can be used to validate and create instances of 2D areas.
/// </summary>
public class Area2D : ValueObject
{
    public double Length { get; }
    public double Height { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Area2D"/> class with specified length and height.
    /// </summary>
    /// <param name="length">The length of the area. Must be greater than 0.</param>
    /// <param name="height">The height of the area. Must be greater than 0.</param>
    public Area2D(double length, double height)
    {
        Length = length;
        Height = height;
    }

    /// <summary>
    /// Tries to create a new instance of the <see cref="Area2D"/> class with specified length and height.
    /// </summary>
    /// <param name="length">Must be positive, greater than 0.</param>
    /// <param name="height">Must be positive, greater than 0.</param>
    /// <param name="area">Used to output the created area if valid.</param>
    /// <returns>True if the conditions are met and the area is created; otherwise, false.</returns>
    public static bool TryCreate(double length, double height, out Area2D? area)
    {
        area = null;

        // Validate dimensions: both must be positive
        if (length <= 0 || height <= 0)
            return false;

        area = new Area2D(length, height);
        return true;
    }

    public static Area2D Create(double length, double height)
    {
        if (!TryCreate(length, height, out Area2D? dimensions) || dimensions is null)
            throw new ValidationException($"Invalid values for projected area: {length}, {height}");
        return dimensions;
    }

    /// <summary>
    /// Provides components used for equality comparison.
    /// </summary>
    /// <returns>An enumerable of the object's defining values.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Length;
        yield return Height;
    }
}
