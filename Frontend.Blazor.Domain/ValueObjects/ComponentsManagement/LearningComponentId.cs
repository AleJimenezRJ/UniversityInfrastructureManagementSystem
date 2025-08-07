using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.ComponentsManagement;

/// <summary>
/// Represents a unique identifier for a learning component.
/// </summary>
public class LearningComponentId : ValueObject
{
    public string Value { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningComponentId"/> class with specified properties.
    /// </summary>
    /// <param name="id"></param>
    private LearningComponentId(string id)
    {
        Value = id;
    }

    /// <summary>
    /// Tries to create a valid LearningComponentId.
    /// Format:  short type - digits (e.g. "PROJ-001")
    /// </summary>
    /// How to call it: if (LearningComponentId.TryCreate("PROJ-001", "Projector", out var id1))
    public static bool TryCreate(string? id, string componentType, out LearningComponentId? componentId)
    {
        componentId = null;

        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(componentType))
            return false;

        // Diccionario de patrones por tipo
        var patterns = new Dictionary<string, string>
        {
            { "Projector", @"^PROJ-\d{3}$" },
            { "Whiteboard", @"^WB-\d{3}$" },
            { "LearningComponent", @"^(PROJ-\d{3}|WB-\d{3})$" }
        };

        if (!patterns.TryGetValue(componentType, out var pattern))
            return false;

        if (!Regex.IsMatch(id, pattern))
            return false;

        componentId = new LearningComponentId(id);
        return true;
    }


    /// <summary>
    /// Overrides the GetHashCode method to provide a hash code for the object.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

