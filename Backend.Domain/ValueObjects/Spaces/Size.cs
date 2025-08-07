using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

/// <summary>
/// Represents a size value that must be strictly greater than 0.
/// </summary>
public class Size : ValueObject
{
    /// <summary>
    /// The value of the size.
    /// </summary>
    public double Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Size"/> class.
    /// </summary>
    /// <param name="value">The size value, which must be greater than 0.</param>
    private Size(double value)
    {
        Value = value;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="Size"/> class.
    /// </summary>
    /// <param name="value">The input value to validate as a size.</param>
    /// <param name="size">The resulting <see cref="Size"/> object if validation succeeds; otherwise, null.</param>
    /// <returns>True if the <see cref="Size"/> object is successfully created; otherwise, false.</returns>
    public static bool TryCreate(double value, out Size? size)
    {
        size = null;

        if (value <= 0)
        {
            return false;
        }

        size = new Size(value);
        return true;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="Size"/> class.
    /// </summary>
    /// <param name="value">The input value to validate as a size.</param>
    /// <returns>A valid instance of the <see cref="Size"/> class.</returns>
    /// <exception cref="ValidationException">Thrown if the value is not greater than 0.</exception>
    public static Size Create(double value)
    {
        if (!TryCreate(value, out Size? size))
        {
            throw new ValidationException("The size value must be greater than 0.");
        }

        return size!;
    }

    /// <summary>
    /// Overrides the GetEqualityComponents method to provide a hash code for the object.
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
