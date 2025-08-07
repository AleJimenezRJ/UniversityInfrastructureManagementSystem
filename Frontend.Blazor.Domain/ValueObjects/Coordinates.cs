using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

/// <summary>
/// Represents a set of 3D coordinates (X, Y, Z) for a building as a value object.
/// </summary>
public class Coordinates : ValueObject
{
    public double? X { get; }

    public double? Y { get; }

    public double? Z { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinates"/> class with specified latitude and longitude.
    /// </summary>
    /// <param name="x"> x coordinate of the building.</param>
    /// <param name="y"> y coordinate of the building.</param>
    /// <param name="z"> z coordinate of the building.</param>

    public Coordinates(double? x, double? y, double? z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Attempts to create a new Coordinates object with the specified parameter
    /// </summary>
    /// <param name="x"> x coordinate of the building.</param>
    /// <param name="y"> y coordinate of the building.</param>
    /// <param name="z"> z coordinate of the building.</param>
    /// <returns> 
    /// True 
    /// </returns>
    public static bool TryCreate(double? x, double? y, double? z, out Coordinates? coordinates)
    {
        // Validate the coordinates - pending
        coordinates = new Coordinates(x, y, z);

        return true;
    }


    public static Coordinates Create(double? x, double? y, double? z)
    {
        if (!TryCreate(x, y, z, out Coordinates? coordinates) || coordinates is null)
            throw new ValidationException($"Invalid Position: {x}, {y}, {z}");
        return coordinates;
    }

    /// <summary>
    /// Overrides the GetQualityComponents method to provide a hash code for the object.
    /// </summary>
    /// <returns>
    /// The X, Y and Z coordinates stored in this valueObject.
    /// </returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X!;
        yield return Y!;
        yield return Z!;
    }
}
