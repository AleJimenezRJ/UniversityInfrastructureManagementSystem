using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject.UniversityManagement;

/// <summary>
/// Unit tests for the <see cref="EntityLocation"/> value object,
/// verifying both its validation logic and equality semantics.
/// </summary>

public class EntityLocationTests
{
    /// <summary>
    /// Ensures that <see cref="EntityLocation.TryCreate"/> returns false when given a null string.
    /// </summary>
    [Fact]
    public void TryCreateWithNullStringReturnsFalse()
    {
        string? input = null;
       
        var result = EntityLocation.TryCreate(input, out var _);

        result.Should().BeFalse(because: "validation should fail when given a null string");
    }

    /// <summary>
    /// Ensures that <see cref="EntityLocation.TryCreate"/> returns false when given an empty string.
    /// </summary>
    [Fact]
    public void TryCreateWithEmptyStringReturnsFalse()
    {
        var input = string.Empty;

        var result = EntityLocation.TryCreate(input, out var _);

        result.Should().BeFalse(because: "validation should fail when given an empty string");
    }

    /// <summary>
    /// Ensures that <see cref="EntityLocation.TryCreate"/> returns false when given only whitespace.
    /// </summary>
    [Fact]
    public void TryCreateWithWhitespaceOnlyReturnsFalse()
    {
        string input = "   ";
 
        var result = EntityLocation.TryCreate(input, out var _);

        result.Should().BeFalse(because: "validation should fail when given only whitespace");
    }

    /// <summary>
    /// Ensures that <see cref="EntityLocation.TryCreate"/> returns false when the input exceeds 100 characters.
    /// </summary>
    [Fact]
    public void TryCreateWithTooLongStringReturnsFalse()
    {
        string input = new string('d', 101);
 
        var result = EntityLocation.TryCreate(input, out var _);

        result.Should().BeFalse(because: "validation should fail when name exceeds 100 characters");
    }

    /// <summary>
    /// Ensures that <see cref="EntityLocation.TryCreate"/> returns true and outputs a valid object when given a valid name.
    /// </summary>
    [Theory]
    [InlineData("Finca 1")]
    [InlineData("  Finca   2")]
    [InlineData("Finca3")]
    [InlineData("Finca   ")]
    public void TryCreateWithValidNameReturnsTrue(string input)
    { 
        var result = EntityLocation.TryCreate(input, out var entityLocation);

        result.Should().BeTrue(because: "a properly formatted name should pass validation");
        entityLocation.Should().NotBeNull();
        entityLocation!.Location.Should().Be(input);
    }


    /// <summary>
    /// Ensures that <see cref="EntityLocation.Create"/> throws <see cref="ValidationException"/> for null input.
    /// </summary>
    [Fact]
    public void CreateWithNullStringThrowsValidationException()
    {
        string? input = null;

        Action act = () => EntityLocation.Create(input);

        act.Should().Throw<ValidationException>().WithMessage("The location must be non-empty and no more than 100 characters long.");
    }

    /// <summary>
    /// Ensures that <see cref="EntityLocation.Create"/> throws <see cref="ValidationException"/> for empty input.
    /// </summary>
    [Fact]
    public void CreateWithEmptyStringThrowsValidationException()
    {
        string input = "";

        Action act = () => EntityLocation.Create(input);

        act.Should().Throw<ValidationException>().WithMessage("The location must be non-empty and no more than 100 characters long.");
    }

    /// <summary>
    /// Ensures that <see cref="EntityLocation.Create"/> throws <see cref="ValidationException"/> for overly long input.
    /// </summary>
    [Fact]
    public void CreateWithTooLongStringThrowsValidationException()
    {
        string input = new string('z', 101);

        Action act = () => EntityLocation.Create(input);

        act.Should().Throw<ValidationException>().WithMessage("The location must be non-empty and no more than 100 characters long.");
    }

    /// <summary>
    /// Ensures that <see cref="EntityLocation.Create"/> returns a valid <see cref="EntityLocation"/> object when given valid input.
    /// </summary>
    [Theory]
    [InlineData("Finca 1")]
    [InlineData("  Finca   2")]
    [InlineData("Finca3")]
    [InlineData("Finca   ")]
    public void CreateWithValidNameReturnsEntityLocation(string input)
    {
        var result = EntityLocation.Create(input);

        result.Should().NotBeNull();
        result.Location.Should().Be(input);
    }

    /// <summary>
    /// Verifies that two <see cref="EntityLocation"/> instances with the same value are considered equal.
    /// </summary>
    [Fact]
    public void EntityLocationsWithSameValueShouldBeEqual()
    {
        var location1 = EntityLocation.Create("Finca 1");
        var location2 = EntityLocation.Create("Finca 1");
        
        location1.Should().Be(location2, because: "value objects with same value should be equal");
    }

    /// <summary>
    /// Verifies that two <see cref="EntityLocation"/> instances with different values are not considered equal.
    /// </summary>
    [Fact]
    public void EntityLocationsWithDifferentValuesShouldNotBeEqual()
    {
        var location1 = EntityLocation.Create("Finca 1");
        var location2 = EntityLocation.Create("Finca 2");

        location1.Should().NotBe(location2, because: "value objects with different values should not be equal");
    }
}
