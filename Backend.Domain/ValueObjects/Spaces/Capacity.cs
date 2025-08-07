using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

public class Capacity : ValueObject
{
    /// <summary>
    /// Represents the maximum capacity of a learning space.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Capacity"/> class with specified properties.
    /// </summary>
    /// <param name="value">Represents the maximum capacity of a learning space.</param>
    private Capacity(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="Capacity"/> class. It needs to be a positive integer.
    /// </summary>
    /// <param name="value">The input value to validate as the maximum capacity.</param>
    /// <param name="capacity">The resulting <see cref="Capacity"/> object if validation succeeds; otherwise, null.</param>
    /// <returns>True if the <see cref="Capacity"/> object is successfully created; otherwise, false.</returns>
    public static bool TryCreate(int? value, out Capacity? capacity)
    {
        capacity = null;

        if (value is null || value <= 0)
            return false;

        capacity = new Capacity(value.Value);
        return true;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="Capacity"/> class. It needs to be a positive integer.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>
    /// A valid instance of capacity entity
    /// </returns>
    /// <exception cref="ValidationException"> In case, the entity doesn't complies the business rules</exception>
    public static Capacity Create(int? value)
    {
        if (!TryCreate(value, out Capacity? capacity))
        {
            throw new ValidationException("The capacity must be an integer greater than zero.");
        }

        return capacity!;
    }

    /// <summary>
    /// Overrides the GetQualityComponents method to provide a hash code for the object.
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

}

