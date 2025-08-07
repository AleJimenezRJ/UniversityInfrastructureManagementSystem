using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

public class FloorNumber : ValueObject
{
    /// <summary>
    /// Represents the FloorNumber of a building.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FloorNumber"/> class with specified properties.
    /// </summary>
    /// <param name="value">Represents the maximum FloorNumber of a building.</param>
    private FloorNumber(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="FloorNumber"/> class. It needs to be a positive integer.
    /// </summary>
    /// <param name="value">The input value to validate as the maximum FloorNumber.</param>
    /// <param name="FloorNumber">The resulting <see cref="FloorNumber"/> object if validation succeeds; otherwise, null.</param>
    /// <returns>True if the <see cref="FloorNumber"/> object is successfully created; otherwise, false.</returns>
    public static bool TryCreate(int? value, out FloorNumber? FloorNumber)
    {
        FloorNumber = null;

        if (value is null || value <= 0)
            return false;

        FloorNumber = new FloorNumber(value.Value);
        return true;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="FloorNumber"/> class. It needs to be a positive integer.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>
    /// A valid instance of FloorNumber entity
    /// </returns>
    /// <exception cref="ValidationException"> In case, the entity doesn't complies the business rules</exception>
    public static FloorNumber Create(int? value)
    {
        if (!TryCreate(value, out FloorNumber? FloorNumber))
        {
            throw new ValidationException("The FloorNumber must be an integer greater than zero.");
        }

        return FloorNumber!;
    }

    /// <summary>
    /// Overrides the GetQualityComponents method to provide a hash code for the object.
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

}

