using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

/// <summary>
/// Rrepresents dimensions with width, length, and height as a value object.
/// Can be used to validate and create instances of dimensions.
/// </summary>
public class Dimension : ValueObject
{
    public double Width { get; }

    public double Length { get; }

    public double Height { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dimension"/> class with specified width, length, and height.
    /// </summary>
    /// <param name="width">The width of the dimensions.</param>
    /// <param name="length">The length of the dimensions.</param>
    /// <param name="height">The height of the dimensions.</param>
    public Dimension(double width, double length, double height)
    {
        Width = width;
        Length = length;
        Height = height;
    }

    /// <summary>
    /// Tries to create a new instance of the <see cref="Dimension"/> class with specified width, length, and height.
    /// </summary>
    /// <param name="width">Has to be positive, bigger than 0</param>
    /// <param name="length">Has to be positive, bigger than 0</param>
    /// <param name="height">Has to be positive, bigger than 0</param>
    /// <param name="dimensions">Used to try to create the dimension</param>
    /// <returns>
    /// True or false depending on whether the conditions are met
    /// </returns>
    public static bool TryCreate(double width, double length, double height, out Dimension? dimensions)
    {
        dimensions = null;

        // Validate dimensions: all must be positive
        if (width <= 0 || length <= 0 || height <= 0)
            return false;

        dimensions = new Dimension(width, length, height);
        return true;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="Id"/> class. It consists of 1 to 5 digits.
    /// </summary>
    /// <param name="width"> The width of a learning space</param>
    /// <param name="length"></param>
    /// <param name="height"></param>
    /// <returns>
    /// A valid instance of Id entity
    /// </returns>
    /// <exception cref="ValidationException"> In case, the entity doesn't complies the business rules</exception>
    public static Dimension Create(double width, double length, double height)
    {
        if (!TryCreate(width, length, height, out Dimension? dimensions) || dimensions is null)
            throw new ValidationException($"Invalid Dimension: {width}, {length}, {height}");
        return dimensions;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Width;
        yield return Length;
        yield return Height;
    }

    /// <summary>
    /// Returns a string representation of the <see cref="Dimension"/> object, displaying its length, height, and width.
    /// </summary>
    /// <returns>
    /// A string in the format "Length:{Length}, Height:{Height}, Width:{Width}".
    /// </returns>
    public override string ToString()
    {
        return $"Length:{Length}, Height:{Height}, Width:{Width}";
    }
}
