
using System.Collections.Generic;
using System.Linq;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Helpers;

/// <summary>
/// Provides functionality for translating color names between English, Spanish, and hexadecimal formats.
/// </summary>
/// <remarks>The <see cref="ColorTranslator"/> class offers methods to translate color names to different formats
/// and retrieve detailed information about colors, including their English name, Spanish name, and hexadecimal code. It
/// supports case-insensitive lookups and handles invalid or unsupported inputs gracefully.</remarks>
public static class ColorTranslator
{
    /// <summary>
    /// Represents a collection of predefined color data, including English and Spanish names,  as well as their
    /// corresponding hexadecimal color codes.
    /// </summary>
    private static readonly (string English, string Spanish, string Hex)[] ColorData = new[]
    {
        // BLACK & WHITE
        ("Black",   "Negro",         "#000000"),
        ("White",   "Blanco",        "#FFFFFF"),
        ("Gray",    "Gris",          "#9E9E9E"),
        ("Silver",  "Plateado",      "#C0C0C0"),
        ("Brown",   "Marrón",        "#795548"),

        // REDS
        ("Maroon",     "Granate",        "#800000"),
        ("DarkRed",    "Rojo Oscuro",    "#B71C1C"),
        ("Crimson",    "Carmesí",        "#DC143C"),
        ("Red",        "Rojo",           "#F44336"),
        ("Salmon",     "Salmón",         "#FA8072"),

        // ORANGES & YELLOWS
        ("DarkOrange", "Naranja Oscuro", "#FF5722"),
        ("Orange",     "Naranja",        "#FF9800"),
        ("Gold",       "Dorado",         "#FFD700"),
        ("Yellow",     "Amarillo",       "#FFEB3B"),
        ("Peach",      "Durazno",        "#FFDAB9"),

        // GREENS
        ("DarkGreen",  "Verde Oscuro",   "#1B5E20"),
        ("Green",      "Verde",          "#388E3C"),
        ("Emerald",    "Esmeralda",      "#50C878"),
        ("Lime",       "Lima",           "#CDDC39"),
        ("Olive",      "Oliva",          "#808000"),

        // CYANS & TEALS
        ("Teal",       "Verde Azulado",  "#009688"),
        ("Cyan",       "Cian",           "#00BCD4"),
        ("Aqua",       "Aguamarina",     "#00FFFF"),
        ("Turquoise",  "Turquesa",       "#40E0D0"),
        ("Mint",       "Menta",          "#98FF98"),

        // BLUES
        ("Navy",       "Azul Marino",    "#001F54"),
        ("DarkBlue",   "Azul Oscuro",    "#0D47A1"),
        ("Blue",       "Azul",           "#2196F3"),
        ("SteelBlue",  "Azul Acero",     "#4682B4"),
        ("SkyBlue",    "Celeste",        "#87CEEB"),

        // PURPLES & PINKS
        ("Indigo",     "Índigo",         "#3F51B5"),
        ("Violet",     "Violeta",        "#8F00FF"),
        ("Purple",     "Morado",         "#9C27B0"),
        ("Pink",       "Rosa",           "#E91E63"),
        ("Magenta",    "Fucsia",         "#FF00FF")
    };

    /// <summary>
    /// Provides a lookup dictionary for color data, allowing retrieval of color information by English name, Spanish
    /// name, or hexadecimal code.
    /// </summary>
    private static readonly Dictionary<string, (string English, string Spanish, string Hex)> Lookup =
        ColorData
            .SelectMany(c => new[] { c.English, c.Spanish, c.Hex }
                .Select(key => (Key: key.ToLowerInvariant(), Value: c)))
            .ToDictionary(x => x.Key, x => x.Value);

    /// <summary>
    /// Translates a color name to the specified language or format.
    /// </summary>
    /// <param name="color">The name of the color to translate. This parameter is case-insensitive.</param>
    /// <param name="to">The target language or format for the translation. Supported values are: <list type="bullet">
    /// <item><term>"english"</term><description>Translates the color name to English.</description></item>
    /// <item><term>"spanish"</term><description>Translates the color name to Spanish.</description></item>
    /// <item><term>"hex"</term><description>Returns the hexadecimal representation of the color.</description></item>
    /// </list> If an unsupported value is provided, the original <paramref name="color"/> is returned.</param>
    /// <returns>The translated color name in the specified language or format, or the original <paramref name="color"/>  if the
    /// translation is not available or the <paramref name="to"/> parameter is invalid. Returns an empty string if
    /// <paramref name="color"/> is <see langword="null"/>.</returns>
    public static string Translate(string color, string to)
    {
        if (color is null) return string.Empty;
        if (Lookup.TryGetValue(color.ToLowerInvariant(), out var c))
        {
            return to.ToLowerInvariant() switch
            {
                "english" => c.English,
                "spanish" => c.Spanish,
                "hex" => c.Hex,
                _ => color
            };
        }
        return color;
    }

    /// <summary>
    /// Retrieves the English name, Spanish name, and hexadecimal code for a specified color.
    /// </summary>
    /// <param name="color">The name of the color to look up. This parameter is case-insensitive and cannot be null.</param>
    /// <returns>A tuple containing the English name, Spanish name, and hexadecimal code of the color if found;  otherwise, <see
    /// langword="null"/>.</returns>
    public static (string English, string Spanish, string Hex)? GetAll(string color)
    {
        if (color is null) return null;
        if (Lookup.TryGetValue(color.ToLowerInvariant(), out var c))
            return c;
        return null;
    }

    /// <summary>
    /// Retrieves a collection of colors, each represented by their English name, Spanish name, and hexadecimal color
    /// code.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of tuples, where each tuple contains the English name, Spanish name, and
    /// hexadecimal color code of a color.</returns>
    public static IEnumerable<(string English, string Spanish, string Hex)> GetAllColors()
    {
        return ColorData;
    }
}
