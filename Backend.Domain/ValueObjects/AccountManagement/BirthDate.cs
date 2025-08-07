using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

/// <summary>
/// Represents a birth date as a value object.
/// </summary>
public partial class BirthDate : ValueObject
{
    public DateOnly Value { get; set; }

    /// <summary>
    /// Private constructor to create an instance of the BirthDate value object.
    /// </summary>
    /// <param name="value"> The birth date value.</param>
    private BirthDate(DateOnly value)
    {
        Value = value;
    }


    /// <summary>
    /// Creates an BirthDate or throws a ValidationException if the format is invalid.
    /// </summary>
    /// <param name="value">The birthdate to validate and construct.</param>
    /// <returns>A valid BirthDate.</returns>
    /// <exception cref="ValidationException">Thrown when the format is invalid.</exception>
    public static BirthDate Create(DateOnly? value)
    {
        if (!TryCreate(value, out BirthDate? birthDate) || birthDate is null)
        {
            throw new ValidationException("Invalid birth date. Expected format: yyyy-MM-dd");
        }

        return birthDate;
    }


    /// <summary>
    /// Creates an instance of the BirthDate value object if the provided birth date is valid.
    /// </summary>
    /// <param name="date"> The birth date to validate and create the value object from.</param>
    /// <param name="result"> The resulting BirthDate value object if the birth date is valid; otherwise, null.</param>
    /// <returns> True if the birth date is valid and the value object was created; otherwise, false.</returns>
    public static bool TryCreate(DateOnly? date, out BirthDate? result)
    {
        result = null;

        if (date is null)
            return false;

        // Convert date to string
        var formattedDate = date.Value.ToString("yyyy-MM-dd");

        // Validate format type "yyyy-MM-dd"
        var regex = CreateDateRegex();
        if (!regex.IsMatch(formattedDate))
            return false;

        result = new BirthDate(date.Value);
        return true;
    }

    /// <summary>
    /// Overrides the GetEqualityComponents method to provide the components used for equality comparison.
    /// </summary>
    /// <returns> An enumerable collection of objects used for equality comparison.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Private regex to validate the date format "yyyy-MM-dd".
    /// Implements a regex pattern at compile time for performance optimization.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^\d{4}-\d{2}-\d{2}$")]
    private static partial Regex CreateDateRegex();    
}
