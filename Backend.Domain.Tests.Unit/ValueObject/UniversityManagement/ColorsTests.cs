using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject.UniversityManagement;

/// <summary>
/// Unit tests for the <see cref="Colors"/> value object,
/// verifying both its validation logic and equality semantics.
/// </summary>
public class ColorsTests
{

    /// <summary>
    /// Ensures that <see cref="Colors.TryCreate"/> returns true and outputs a valid <see cref="Colors"/> 
    /// object when the input matches a color from the allowed list, regardless of casing.
    /// </summary>
    [Theory]
    [InlineData("red")]
    [InlineData("RED")]
    [InlineData("ReD")]
    [InlineData("Red")]
    [InlineData("Green")]
    [InlineData("Blue")]
    [InlineData("Yellow")]
    [InlineData("Black")]
    [InlineData("White")]
    [InlineData("Orange")]
    [InlineData("Purple")]
    [InlineData("Gray")]
    [InlineData("Brown")]
    [InlineData("Pink")]
    [InlineData("Cyan")]
    [InlineData("Lime")]
    [InlineData("Teal")]
    public void TryCreate_WithAllowedColor_ShouldReturnTrue(string input)
    {
        var result = Colors.TryCreate(input, out var color);

        result.Should().BeTrue();
        color.Should().NotBeNull();
        color!.Value.Should().Be(input);
    }


    /// <summary>
    /// Ensures that <see cref="Colors.TryCreate"/> returns false and outputs null 
    /// when the input is not in the list of allowed colors.
    /// </summary>
    [Theory]
    [InlineData("AzulRey")]
    [InlineData("LightBlue")]
    [InlineData("fuchsia")]
    [InlineData("Pinkish")]
    [InlineData("123")]
    [InlineData("Perú")]
    public void TryCreate_WithInvalidColor_ShouldReturnFalse(string input)
    {
        var result = Colors.TryCreate(input, out var color);

        result.Should().BeFalse();
        color.Should().BeNull();
    }


    /// <summary>
    /// Ensures that <see cref="Colors.Create"/> throws a <see cref="ValidationException"/> 
    /// when the input is not in the list of allowed colors.
    /// </summary>
    [Fact]
    public void Create_WithInvalidColor_ShouldThrowValidationException()
    {
        
        string input = "LightBlue";

        Action act = () => Colors.Create(input);

        act.Should().Throw<ValidationException>().WithMessage("Invalid color: LightBlue");
    }

    /// <summary>
    /// Ensures that <see cref="Colors.Create"/> returns a valid <see cref="Colors"/> object 
    /// when given a valid input color from the allowed list.
    /// </summary>
    [Fact]
    public void Create_WithValidColor_ShouldReturnColorObject()
    {
        
        string input = "Purple";

        var result = Colors.Create(input);

        result.Should().NotBeNull();
        result.Value.Should().Be(input);
    }

    /// <summary>
    /// Verifies that two <see cref="Colors"/> instances created from the same color value with different casing 
    /// are considered equal.
    /// </summary>
    [Theory]
    [InlineData("Green", "green")]
    [InlineData("Silver", "SILVER")]
    [InlineData("black", "BlAcK")]
    public void Colors_WithSameValueDifferentCasing_ShouldBeEqual(string color1, string color2)
    {
        
        var c1 = Colors.Create(color1);
        var c2 = Colors.Create(color2);

        c1.Should().Be(c2);
    }

    /// <summary>
    /// Verifies that two <see cref="Colors"/> instances created from different color values, 
    /// even with different casing, are considered unequal.
    /// </summary>
    [Theory]
    [InlineData("Green", "red")]
    [InlineData("Silver", "purple")]
    [InlineData("black", "yellow")]
    public void Colors_WithDifferentValueDifferentCasing_ShouldBeEqual(string color1, string color2)
    {
        
        var c1 = Colors.Create(color1);
        var c2 = Colors.Create(color2);

        c1.Should().NotBe(c2);
    }

    /// <summary>
    /// Ensures that <see cref="Colors"/> returns false when given a null string.
    /// </summary>
    [Fact]
    public void TryCreateWithNullStringReturnsFalse()
    {
        
        string? input = null;

        var result = Colors.TryCreate(input, out var _);

        result.Should().BeFalse(because: "validation should fail when given a null string");
    }

    /// <summary>
    /// Ensures that <see cref="Colors"/> returns false when given an empty string.
    /// </summary>
    [Fact]
    public void TryCreateWithEmptyStringReturnsFalse()
    {
        
        var input = string.Empty;

        var result = Colors.TryCreate(input, out var _);

        result.Should().BeFalse(because: "validation should fail when given an empty string");
    }

    /// <summary>
    /// Ensures that <see cref="Colors"/> returns false when given only whitespace.
    /// </summary>
    [Fact]
    public void TryCreateWithWhitespaceOnlyReturnsFalse()
    {
        
        string input = "   ";

        var result = Colors.TryCreate(input, out var _);
    
        result.Should().BeFalse(because: "validation should fail when given only whitespace");
    }
}
