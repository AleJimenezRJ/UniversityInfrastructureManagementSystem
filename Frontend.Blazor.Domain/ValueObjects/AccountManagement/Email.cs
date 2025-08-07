using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;

/// <summary>
/// Represents an email address as a value object.
/// </summary>
public partial class Email : ValueObject
{
    public string Value { get; set; }

    /// <summary>
    ///  Private constructor to create an instance of the Email value object.
    /// </summary>
    /// <param name="value"> The email address value.</param>
    private Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates an Email or throws a ValidationException if the format is invalid.
    /// </summary>
    /// <param name="value">The email to validate and construct.</param>
    /// <returns>A valid Email.</returns>
    /// <exception cref="ValidationException">Thrown when the format is invalid.</exception>
    public static Email Create(string? value)
    {
        if (!TryCreate(value, out Email? email) || email is null)
        {
            throw new ValidationException("Invalid email format. It must contain '@' and end with .domain");
        }

        return email;
    }

    /// <summary>
    /// Creates an instance of the Email value object if the provided email is valid.
    /// </summary>
    /// <param name="email"> The email address to validate and create the value object from.</param>
    /// <param name="result"> The resulting Email value object if the email is valid; otherwise, null.</param>
    /// <returns> True if the email is valid and the value object was created; otherwise, false.</returns>
    public static bool TryCreate(string? email, out Email? result)
    {
        result = null;

        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Validate that it contains a valid email format
        var regex = EmailValidationRegex();

        if (!regex.IsMatch(email))
            return false;

        result = new Email(email);
        return true;
    }


    /// <summary>
    ///  Overrides the GetEqualityComponents method to provide the components used for equality comparison.
    /// </summary>
    /// <returns> An enumerable collection of objects used for equality comparison.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Private regex to validate the email format "abc@dfg.hij".
    /// Implements a regex pattern at compile time for performance optimization.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, "es-CR")]
    private static partial Regex EmailValidationRegex();
}
