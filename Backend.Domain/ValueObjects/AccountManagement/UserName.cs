using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

/// <summary>
/// Represents an UserName address as a value object.
/// </summary>
public partial class UserName : ValueObject
{
    public string Value { get; set; }

    /// <summary>
    ///  Private constructor to create an instance of the UserName value object.
    /// </summary>
    /// <param name="value"> The UserName value.</param>
    private UserName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates an UserName or throws a ValidationException if the format is invalid.
    /// </summary>
    /// <param name="value">The UserName to validate and construct.</param>
    /// <returns>A valid UserName.</returns>
    /// <exception cref="ValidationException">Thrown when the format is invalid.</exception>
    public static UserName Create(string? value)
    {
        if (!TryCreate(value, out UserName? UserName) || UserName is null)
        {
            throw new ValidationException("Invalid UserName format. It must not contain spaces or uppercase letters and be less than 50 characters.");
        }

        return UserName;
    }
    
    /// <summary>
    /// Creates an instance of the UserName value object if the provided UserName is valid.
    /// </summary>
    /// <param name="UserName"> The UserName address to validate and create the value object from.</param>
    /// <param name="result"> The resulting UserName value object if the UserName is valid; otherwise, null.</param>
    /// <returns> True if the UserName is valid and the value object was created; otherwise, false.</returns>
    public static bool TryCreate(string? UserName, out UserName? result)
    {
        result = null;

        if (string.IsNullOrWhiteSpace(UserName))
            return false;

        // Validate that it contains a valid UserName format
        var regex = UserNameValidationRegex();

        if (!regex.IsMatch(UserName))
            return false;

        result = new UserName(UserName);
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
    /// Private regex to validate the UserName format, no uppercase letters or spaces.
    /// Implements a regex pattern at compile time for performance optimization.
    /// Allows up to 50 alphanumeric characters, including "." and "_"
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(@"^[a-z0-9._]{1,50}$")]
    private static partial Regex UserNameValidationRegex();
}
