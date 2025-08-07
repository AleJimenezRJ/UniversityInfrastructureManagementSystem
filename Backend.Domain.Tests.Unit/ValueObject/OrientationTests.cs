using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="Orientation"/> value object,
/// </summary>
public class OrientationTests
{
    /// <summary>
    /// Ensures that <see cref="Orientation.TryCreate"/> returns false when given a null string.
    /// </summary>
    [Fact]
    public void TryCreate_WithNullValue_ReturnsFalse()
    {
        // Arrange
        string? input = null;

        // Act
        var result = Orientation.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "null input is invalid");
    }
    /// <summary>
    /// Ensures that <see cref="Orientation.TryCreate"/> returns false when given an empty string.
    /// </summary>
    [Fact]
    public void TryCreate_WithEmptyString_ReturnsFalse()
    {
        // Arrange
        var input = "";

        // Act
        var result = Orientation.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "empty string is invalid");
    }
    /// <summary>
    /// Ensures that <see cref="Orientation.TryCreate"/> returns false when given an invalid value for orientation.
    /// </summary>
    [Fact]
    public void TryCreate_WithInvalidOrientation_ReturnsFalse()
    {
        // Arrange
        var input = "Up";

        // Act
        var result = Orientation.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "only 'North', 'South', 'East', or 'West' are valid orientations");
    }
    /// <summary>
    /// Ensures that <see cref="Orientation.TryCreate"/> returns false when given a string with invalid capitalization.
    /// </summary>
    /// <param name="input"> Different values with capitalization errors </param>
    /// 
    [Theory]
    [InlineData("NORTH")]
    [InlineData("north")]
    [InlineData("nOrth")]
    [InlineData("NoRth")]
    public void TryCreate_WithInvalidCapitalization_ReturnsFalse(string input)
    {
        var result = Orientation.TryCreate(input, out var _);
        // Assert
        result.Should().BeFalse(because: "Orientation must start with one capital letter followed by lowercase letters.");
    }
    /// <summary>
    /// Ensures that <see cref="Orientation.TryCreate"/> returns true when given a valid orientation.
    /// </summary>
    /// <param name="input">Accepted values</param>
    [Theory]
    [InlineData("North")]
    [InlineData("South")]
    [InlineData("East")]
    [InlineData("West")]
    public void TryCreate_WithValidOrientation_ReturnsTrue(string input)
    {
        // Act
        var result = Orientation.TryCreate(input, out var orientation);

        // Assert
        result.Should().BeTrue(because: "valid orientation strings should create the object");
        orientation!.Value.Should().Be(input);
    }
    /// <summary>
    /// Ensures that <see cref="Orientation.TryCreate"/> returns true when given a valid orientation with extra whitespace.
    /// </summary>
    [Fact]
    public void TryCreate_WithExtraWhitespace_StillValid()
    {
        // Arrange
        var input = "  North  ";

        // Act
        var result = Orientation.TryCreate(input, out var orientation);

        // Assert
        result.Should().BeTrue(because: "the program cleans the extra whitespace so it is a valid orientation and should be accepted");
        orientation!.Value.Should().Be("North");
    }

    /// <summary>
    /// Ensures that <see cref="Orientation.Create"/> throws <see cref="ValidationException"/> for null input.
    /// </summary>
    [Fact]
    public void Create_WithNullValue_ThrowsValidationException()
    {
        // Arrange
        string? input = null;
        // Act
        Action act = () => Orientation.Create(input);
        // Assert
        act.Should().Throw<ValidationException>()
           .WithMessage($"Invalid orientation: {input}");
    }
    /// <summary>
    /// Ensures that <see cref="Orientation.Create"/> throws <see cref="ValidationException"/> for empty string input.
    /// </summary>
    [Fact]
    public void Create_WithEmptyString_ThrowsValidationException()
    {
        // Arrange
        var input = "";
        // Act
        Action act = () => Orientation.Create(input);
        // Assert
        act.Should().Throw<ValidationException>()
           .WithMessage($"Invalid orientation: {input}");
    }

    /// <summary>
    /// Ensures that <see cref="Orientation.Create"/> throws <see cref="ValidationException"/> for invalid orientation.
    /// </summary>
    [Fact]
    public void Create_WithInvalidOrientation_ThrowsValidationException()
    {
        // Arrange
        var input = "Diagonal";

        // Act
        Action act = () => Orientation.Create(input);

        // Assert
        act.Should().Throw<ValidationException>()
           .WithMessage($"Invalid orientation: {input}");
    }

    /// <summary>
    /// Ensures that <see cref="Orientation.Create"/> throws <see cref="ValidationException"/> for invalid capitalization.
    /// </summary>
    /// <param name="input"></param>
    [Theory]
    [InlineData("NORTH")]
    [InlineData("north")]
    [InlineData("nOrth")]
    [InlineData("NoRth")]
    public void Create_WithInvalidCapitalization_ThrowsValidationException(string input)
    {
        // Act
        Action act = () => Orientation.Create(input);
        // Assert
        act.Should().Throw<ValidationException>()
           .WithMessage($"Invalid orientation: {input}");
    }
    /// <summary>
    /// Ensures that <see cref="Orientation.Create"/> returns an instance with the correct value when given a valid orientation.
    /// </summary>
    [Fact]
    public void Create_WithValidOrientation_ReturnsInstance()
    {
        // Arrange
        var input = "West";

        // Act
        var orientation = Orientation.Create(input);

        // Assert
        orientation.Value.Should().Be("West");
    }
    /// <summary>
    /// Ensures that <see cref="Orientation.Create"/> returns an instance with the correct value when given a valid orientation with extra whitespace.
    /// </summary>
    [Fact]
    public void Create_WithExtraWhitespace_ReturnsInstance()
    {
        // Arrange
        var input = "  South  ";
        // Act
        var orientation = Orientation.Create(input);
        // Assert
        orientation.Value.Should().Be("South");
    }

    /// <summary>
    /// Verifies that two <see cref="Orientation"/> instances with the same value are considered equal.
    /// </summary>
    [Fact]
    public void Orientation_Equality_WithSameValue_ReturnsTrue()
    {
        // Arrange
        var a = Orientation.Create("South");
        var b = Orientation.Create("South");

        // Act & Assert
        a.Should().Be(b, "value objects with same value should be equal");
    }

    /// <summary>
    /// Verifies that two <see cref="Orientation"/> instances with different values are not considered equal.
    /// </summary>
    [Fact]
    public void Orientation_Equality_WithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var a = Orientation.Create("East");
        var b = Orientation.Create("West");

        // Act & Assert
        a.Should().NotBe(b, "value objects with different values should not be equal");
    }

    /// <summary>
    /// Ensures that <see cref="Orientation.ToString"/> returns the orientation value as a string.
    /// </summary>
    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        var orientation = Orientation.Create("North");

        // Act
        var result = orientation.ToString();

        // Assert
        result.Should().Be("North");
    }
}