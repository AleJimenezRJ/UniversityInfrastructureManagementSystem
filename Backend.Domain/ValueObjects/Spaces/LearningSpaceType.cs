using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

public class LearningSpaceType : ValueObject
{
    /// <summary>
    /// Actual learning space types.
    /// </summary>
    private const string Auditorium = "Auditorium";
    private const string Classroom = "Classroom";
    private const string Laboratory = "Laboratory";

    /// <summary>
    /// List of valid learning space types.
    /// </summary>
    private static readonly HashSet<string> ValidTypes =
    [
        Auditorium,
        Classroom,
        Laboratory
    ];

    /// <summary>
    /// Represents the type of a learning space.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpaceType"/> class with specified properties.
    /// </summary>
    /// <param name="value"></param>
    private LearningSpaceType(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="LearningSpaceType"/> class.
    /// </summary>
    /// <param name="value">The input string to validate as the type of a learning space. It must match one of the predefined valid types.</param>
    /// <param name="learningSpaceType">The resulting <see cref="LearningSpaceType"/> object if validation succeeds; otherwise, null.</param>
    /// <returns>True if the <see cref="LearningSpaceType"/> object is successfully created; otherwise, false.</returns>
    public static bool TryCreate(string? value, out LearningSpaceType? learningSpaceType)
    {
        learningSpaceType = null;

        // Check if the value is null or empty
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        // Check if the value is one of the valid types
        if (!ValidTypes.Contains(value))
        {
            return false;
        }

        learningSpaceType = new LearningSpaceType(value);
        return true;
    }

    /// <summary>
    /// Validates and creates a new instance of the <see cref="LearningSpaceType"/> class. It consists of 1 to 5 digits.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>
    /// A valid instance of Id entity
    /// </returns>
    /// <exception cref="ValidationException"> In case, the entity doesn't complies the business rules</exception>
    public static LearningSpaceType Create(string? value)
    {
        if (!TryCreate(value, out LearningSpaceType? type))
        {
            throw new ValidationException("The current type is not valid.");
        }

        return type!;
    }

    /// <summary>
    /// Overrides the GetQualityComponents method to provide a hash code for the object.
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
