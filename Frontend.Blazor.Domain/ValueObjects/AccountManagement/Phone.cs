using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;

/// <summary>
/// Represents a phone number as a value object.
/// </summary>
public partial class Phone : ValueObject
{
    public string Value { get; set; }

    /// <summary>
    /// Private constructor to create an instance of the Phone value object.
    /// </summary>
    /// <param name="value"> The phone number value.</param>
    private Phone(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates an Phone  or throws a ValidationException if the format is invalid.
    /// </summary>
    /// <param name="value">The phone number to validate and construct.</param>
    /// <returns>A valid Phone.</returns>
    /// <exception cref="ValidationException">Thrown when the format is invalid.</exception>
    public static Phone Create(string? value)
    {
        if (!TryCreate(value, out Phone? phone) || phone is null)
        {
            throw new ValidationException("Invalid phone number. Expected format: 1234-5678");
        }

        return phone;
    }

    /// <summary>
    /// Creates an instance of the Phone value object if the provided phone number is valid.
    /// </summary>
    /// <param name="phone"> The phone number to validate and create the value object from.</param>
    /// <param name="result"> The resulting Phone value object if the phone number is valid; otherwise, null.</param>
    /// <returns> True if the phone number is valid and the value object was created; otherwise, false.</returns>
    public static bool TryCreate(string? phone, out Phone? result)
    {
        result = null;

        if (string.IsNullOrWhiteSpace(phone))
            return false;

        // Validate that it contains exactly 8 digits
        var regex = PhoneNumberRegex();

        if (!regex.IsMatch(phone))
            return false;

        result = new Phone(phone);
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
    /// Private regex to validate the phone format "1234-5678".
    /// Implements a regex pattern at compile time for performance optimization.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^\d{4}-\d{4}$")]
    private static partial Regex PhoneNumberRegex();

}
