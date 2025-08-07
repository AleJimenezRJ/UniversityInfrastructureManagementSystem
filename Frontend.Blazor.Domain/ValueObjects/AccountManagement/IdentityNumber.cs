using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;

/// <summary>
/// Represents an identity number as a value object.
/// </summary>
public partial class IdentityNumber : ValueObject
{
    /// <summary>
    /// Gets the underlying value of the identity number.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Private constructor to create an instance of the IdentityNumber value object.
    /// </summary>
    /// <param name="value">The identity number value.</param>
    private IdentityNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates an IdentityNumber or throws a ValidationException if the format is invalid.
    /// </summary>
    /// <param name="identity">The identity number to validate and construct.</param>
    /// <returns>A valid IdentityNumber.</returns>
    /// <exception cref="ValidationException">Thrown when the format is invalid.</exception>
    public static IdentityNumber Create(string? identity)
    {
        if (!TryCreate(identity, out IdentityNumber? result) || result is null)
        {
            throw new ValidationException("The identity number format is invalid. Use the format: 1-1234-5678.");
        }

        return result;
    }


    /// <summary>
    /// Creates an instance of the IdentityNumber value object if the provided identity number is valid.
    /// </summary>
    /// <param name="identity">The identity number to validate and create the value object from.</param>
    /// <param name="result">The resulting IdentityNumber value object if the identity number is valid; otherwise, null.</param>
    /// <returns>True if the identity number is valid and the value object was created; otherwise, false.</returns>
    public static bool TryCreate(string? identity, out IdentityNumber? result)
    {
        result = null;

        if (string.IsNullOrWhiteSpace(identity))
            return false;

        // Validate format type "1-1234-5678"
        var regex = IdentityFormatRegex();

        if (!regex.IsMatch(identity))
            return false;

        result = new IdentityNumber(identity);
        return true;
    }

    /// <summary>
    /// Overrides the GetEqualityComponents method to provide the components used for equality comparison.
    /// </summary>
    /// <returns>An enumerable collection of objects used for equality comparison.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Private regex to validate the identity number format "1-1234-5678".
    /// Implements a regex pattern at compile time for performance optimization.
    /// </summary>
    [GeneratedRegex(@"^\d{1}-\d{4}-\d{4}$")]
    private static partial Regex IdentityFormatRegex();
}
