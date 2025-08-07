using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;

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

    public string Value { get; }

    public Colors(string value)
    {
        Value = value;
    }

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

    public static Colors Create(string color)
    {
        if (!TryCreate(color, out Colors? colors) || colors is null)
            throw new ValidationException($"Invalid color: {color}");
        return colors;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

}