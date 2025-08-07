using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject.Spaces;

/// <summary>
/// Unit tests for the <see cref="LearningSpaceType"/> value object,
/// ensuring validation, creation, and equality behaviors are correct.
/// </summary>
public class LearningSpaceTypeTests
{
    /// <summary>
    /// Ensures that TryCreate returns true and produces a valid object
    /// when the input is one of the predefined allowed types.
    /// </summary>
    [Theory]
    [InlineData("Auditorium")]
    [InlineData("Classroom")]
    [InlineData("Laboratory")]
    public void TryCreate_WithValidType_ReturnsTrueAndCorrectObject(string input)
    {
        var result = LearningSpaceType.TryCreate(input, out var type);

        result.Should().BeTrue();                      // Creation should succeed
        type.Should().NotBeNull();                     // The resulting object should not be null
        type!.Value.Should().Be(input);                // The value should match the input
    }

    /// <summary>
    /// Ensures that TryCreate returns false and null when the input is
    /// either invalid, unrecognized, or improperly formatted.
    /// </summary>
    [Theory]
    [InlineData("Gym")]
    [InlineData("Library")]
    [InlineData("")]
    [InlineData("classroom ")]   // Trailing whitespace
    [InlineData(" auditorium")]  // Leading whitespace
    [InlineData("Lab")]          // Similar but not valid
    public void TryCreate_WithInvalidType_ReturnsFalseAndNull(string input)
    {
        var result = LearningSpaceType.TryCreate(input, out var type);

        result.Should().BeFalse();      // Creation should fail
        type.Should().BeNull();         // The resulting object should be null
    }

    /// <summary>
    /// Ensures that Create returns a valid LearningSpaceType object
    /// when the input is a valid predefined type.
    /// </summary>
    [Fact]
    public void Create_WithValidType_ReturnsLearningSpaceType()
    {
        var input = "Classroom";

        var result = LearningSpaceType.Create(input);

        result.Should().NotBeNull();             // The object should be created
        result.Value.Should().Be(input);         // Its value should match the input
    }

    /// <summary>
    /// Ensures that Create throws a ValidationException when the input
    /// is invalid and does not match any allowed type.
    /// </summary>
    [Fact]
    public void Create_WithInvalidType_ThrowsValidationException()
    {
        var input = "Gym";

        Action act = () => LearningSpaceType.Create(input);

        act.Should().Throw<ValidationException>()               // Should throw exception
           .WithMessage("The current type is not valid.");      // With the expected message
    }

    /// <summary>
    /// Verifies that two LearningSpaceType objects with the same value
    /// are considered equal.
    /// </summary>
    [Fact]
    public void LearningSpaceType_WithSameValue_ShouldBeEqual()
    {
        var t1 = LearningSpaceType.Create("Auditorium");
        var t2 = LearningSpaceType.Create("Auditorium");

        t1.Should().Be(t2);                          // Objects should be equal
        t1.Equals(t2).Should().BeTrue();             // Equals should return true
        t1.GetHashCode().Should().Be(t2.GetHashCode()); // Hash codes should match
    }

    /// <summary>
    /// Verifies that two LearningSpaceType objects with different values
    /// are not equal.
    /// </summary>
    [Fact]
    public void LearningSpaceType_WithDifferentValues_ShouldNotBeEqual()
    {
        var t1 = LearningSpaceType.Create("Auditorium");
        var t2 = LearningSpaceType.Create("Classroom");

        t1.Should().NotBe(t2);                       // Objects should not be equal
        t1.Equals(t2).Should().BeFalse();            // Equals should return false
    }

    /// <summary>
    /// Ensures that TryCreate returns false when the input is null.
    /// </summary>
    [Fact]
    public void TryCreate_WithNullInput_ShouldReturnFalse()
    {
        string? input = null;

        var result = LearningSpaceType.TryCreate(input, out var type);

        result.Should().BeFalse();
        type.Should().BeNull();
    }

    /// <summary>
    /// Ensures that TryCreate returns false when the input is an empty string.
    /// </summary>
    [Fact]
    public void TryCreate_WithEmptyString_ShouldReturnFalse()
    {
        string input = "";

        var result = LearningSpaceType.TryCreate(input, out var type);

        result.Should().BeFalse();
        type.Should().BeNull();
    }

    /// <summary>
    /// Ensures that TryCreate returns false when the input is only whitespace.
    /// </summary>
    [Fact]
    public void TryCreate_WithWhitespace_ShouldReturnFalse()
    {
        string input = "   ";

        var result = LearningSpaceType.TryCreate(input, out var type);

        result.Should().BeFalse();
        type.Should().BeNull();
    }

    /// <summary>
    /// Ensures that TryCreate returns false when casing is incorrect.
    /// </summary>
    [Theory]
    [InlineData("auditorium")]
    [InlineData("classroom")]
    [InlineData("LABORATORY")]
    public void TryCreate_WithIncorrectCasing_ShouldReturnFalse(string input)
    {
        var result = LearningSpaceType.TryCreate(input, out var type);

        result.Should().BeFalse(because: "case-sensitive validation should fail");
        type.Should().BeNull();
    }

    /// <summary>
    /// Ensures that Create throws when casing is incorrect even if value is semantically valid.
    /// </summary>
    [Fact]
    public void Create_WithIncorrectCasing_ShouldThrow()
    {
        string input = "auditorium";

        Action act = () => LearningSpaceType.Create(input);

        act.Should().Throw<ValidationException>()
           .WithMessage("The current type is not valid.");
    }

    /// <summary>
    /// Verifies that the Value property holds exactly the given input.
    /// </summary>
    [Fact]
    public void Value_ShouldBeExactlyTheGivenString()
    {
        var input = "Laboratory";

        var type = LearningSpaceType.Create(input);

        type.Value.Should().Be("Laboratory");
    }
}
