using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

/// <summary>
/// Represents a value object used as a unique identifier for domain entities.
/// Supports either a string of 1 to 5 digits or a non-negative integer up to 99999.
/// </summary>
public class Id : ValueObject
{

    /// <summary>
    /// Regular expression to validate the Id format.
    /// </summary>
    private static readonly Regex _idRegex = new Regex(@"^\d{1,5}$", RegexOptions.Compiled);

    /// <summary>
    /// Gets the string representation of the unique identifier.
    /// </summary>
    public string Value { get; }


    /// <summary>
    /// Gets the integer representation of the identifier if it was created from an integer or is a numeric string; otherwise, null.
    /// </summary>
    public int? ValueInt { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Id"/> class with the specified value.
    /// </summary>
    /// <param name="value">The string representation of the unique identifier.</param>
    private Id(string value)
    {
        Value = value;
        ValueInt = int.TryParse(value, out var result) ? result : null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Id"/> class with the specified integer value.
    /// </summary>
    /// <param name="value">The integer value of the unique identifier.</param>
    private Id(int value)
    {
        Value = value.ToString();
        ValueInt = value;
    }


    /// <summary>
    /// Validates and creates a new instance of the <see cref="Id"/> class from a string.
    /// The value must consist of 1 to 5 digits.
    /// </summary>
    public static bool TryCreate(string? value, out Id? id)
    {
        id = null;
        if (string.IsNullOrWhiteSpace(value) || !_idRegex.IsMatch(value))
            return false;

        id = new Id(value);
        return true;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="Id"/> class from an integer.
    /// The value must be between 0 and 99999 inclusive.
    /// </summary>
    public static bool TryCreate(int? value, out Id? id)
    {
        id = null;

        if (!value.HasValue || value < 0 || value > 99999)
            return false;

        id = new Id((int)value);
        return true;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="Id"/> class from a string.
    /// The value must consist of 1 to 5 digits.
    /// </summary>
    public static Id Create(string? value)
    {
        if (!TryCreate(value, out Id? id))
            throw new ValidationException("The Id is not valid. It must be 1 to 5 digits.");

        return id!;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="Id"/> class from a non-negative integer.
    /// The value must be between 0 and 99999 inclusive.
    /// </summary>
    public static Id Create(int? value)
    {
        if (!TryCreate(value, out Id? id))
            throw new ValidationException("The Id must be a non-negative integer with up to 5 digits.");

        return id!;
    }

    /// <summary>
    /// Overrides the equality components for value-based equality.
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
