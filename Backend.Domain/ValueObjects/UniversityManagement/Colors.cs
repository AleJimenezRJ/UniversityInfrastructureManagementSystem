using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

/// <summary>
/// Represents a color value that is restricted to a predefined set of allowed colors.
/// </summary>
/// <remarks>The <see cref="Colors"/> class ensures that only valid color values from a predefined set are used. 
/// Color values are case-insensitive, and the class provides methods to validate and create instances  safely. Use <see
/// cref="TryCreate(string?, out Colors?)"/> to attempt creating a color without throwing  exceptions, or <see
/// cref="Create(string)"/> to create a color with validation.</remarks>
public class Colors : ValueObject
{
    /// <summary>
    /// A set of allowed colors for the building. It makes sure it is not sensitive case.
    /// </summary>
    private static readonly HashSet<string> AllowedColors = new(StringComparer.OrdinalIgnoreCase)
    {
        "Red",
        "Green",
        "Blue",
        "Yellow",
        "Black",
        "White",
        "Orange",
        "Purple",
        "Gray",
        "Brown",
        "Pink",
        "Cyan",
        "Magenta",
        "Lime",
        "Teal",
        "Olive",
        "Navy",
        "Maroon",
        "Silver",
        "Gold",
        "DarkRed",
        "Crimson",
        "Salmon",
        "DarkOrange",
        "Peach",
        "DarkGreen",
        "Emerald",
        "Aqua",
        "Turquoise",
        "Mint",
        "DarkBlue",
        "SteelBlue",
        "SkyBlue",
        "Indigo",
        "Violet"
    };

    /// <summary>
    /// Gets the value represented by this instance.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Colors"/> class with the specified color value.
    /// </summary>
    /// <param name="value">The string representation of the color. This value cannot be null or empty.</param>
    public Colors(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Attempts to create a <see cref="Colors"/> instance from the specified string value.
    /// </summary>
    /// <remarks>The method validates that the <paramref name="value"/> is not null, empty, or whitespace, and
    /// that it matches  an allowed color. If these conditions are not met, the method returns <see langword="false"/>
    /// and sets  <paramref name="color"/> to <see langword="null"/>.</remarks>
    /// <param name="value">The string representation of the color to create. Must not be null, empty, or whitespace.</param>
    /// <param name="color">When this method returns, contains the created <see cref="Colors"/> instance if the operation succeeded; 
    /// otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Colors"/> instance was successfully created; otherwise, <see
    /// langword="false"/>.</returns>
    public static bool TryCreate(string? value, out Colors? color)
    {
        color = null;

        // Validate color: not empty or null
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        // Validate color is in allowed list
        if (!AllowedColors.Contains(value))
        {
            return false;
        }

        color = new(value);
        return true;
    }

    /// <summary>
    /// Creates a <see cref="Colors"/> instance from the specified color name.
    /// </summary>
    /// <param name="color">The name of the color to create. This value must be a valid color name.</param>
    /// <returns>A <see cref="Colors"/> instance representing the specified color.</returns>
    /// <exception cref="ValidationException">Thrown if the specified <paramref name="color"/> is invalid.</exception>
    public static Colors Create(string color)
    {
        if (!TryCreate(color, out Colors? colors) || colors is null)
            throw new ValidationException($"Invalid color: {color}");
        return colors;
    }

    /// <summary>
    /// Provides the components used to determine equality for the current object.
    /// </summary>
    /// <remarks>This method returns an enumerable of objects that represent the significant  components of
    /// the object for equality comparison. The components are used  to evaluate equality in a case-insensitive
    /// manner.</remarks>
    /// <returns>An <see cref="IEnumerable{T}"/> of objects representing the equality components of the current object. In this
    /// implementation, the returned value is the lowercase invariant form of the <c>Value</c> property.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }


    /// <summary>
    /// Returns the string representation of the current <see cref="Colors"/> instance.
    /// </summary>
    /// <remarks>
    /// This method returns the original color value as provided during creation.
    /// </remarks>
    /// <returns>
    /// The string value of the color represented by this instance.
    /// </returns>
    public override string ToString() => Value;

}