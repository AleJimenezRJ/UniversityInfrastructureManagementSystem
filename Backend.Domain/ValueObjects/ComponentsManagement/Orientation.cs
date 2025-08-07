using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;

/// <summary>
/// Represents the orientation of a learning component.
/// </summary>
public class Orientation : ValueObject
{

    /// <summary>
    /// The direction of the orientation.
    /// </summary>
    private static readonly HashSet<string> ValidOrientations =
    [
        "North", 
        "South", 
        "East", 
        "West"
    ];

    public string Value { get; }

    private Orientation(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates an instance of <see cref="Orientation"/> from a string value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="orientation"></param>
    /// <returns></returns>
    public static bool TryCreate(string? value, out Orientation? orientation)
    {
        orientation = null;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var normalized = value.Trim();

        if (!ValidOrientations.Contains(normalized))
            return false;

        orientation = new Orientation(normalized);
        return true;
    }

    public static Orientation Create(string? value)
    {
        if (!TryCreate(value, out Orientation? orientation) || orientation is null)
            throw new ValidationException($"Invalid orientation: {value}");
        return orientation;
    }

    public override string ToString() => Value;
   

    /// <summary>
    /// Overrides the GetHashCode method to provide a hash code for the object.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

