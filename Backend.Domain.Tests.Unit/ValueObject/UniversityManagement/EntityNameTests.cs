using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject.UniversityManagement;

/// <summary>
/// Unit tests for the <see cref="EntityName"/> value object,
/// verifying both its validation logic and equality semantics.
/// </summary>
public class EntityNameTests
{
    /// <summary>
    /// Ensures that <see cref="EntityName.TryCreate"/> returns false when given a null string.
    /// </summary>
    [Fact]
    public void TryCreateWithNullStringReturnsFalse()
    {
        // Arrange
        string? input = null;

        // Act
        bool result = EntityName.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given a null string");
    }

    /// <summary>
    /// Ensures that <see cref="EntityName.TryCreate"/> returns false when given an empty string.
    /// </summary>
    [Fact]
    public void TryCreateWithEmptyStringReturnsFalse()
    {
        // Arrange
        string input = string.Empty;

        // Act
        bool result = EntityName.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given an empty string");
    }

    /// <summary>
    /// Ensures that <see cref="EntityName.TryCreate"/> returns false when given only whitespace.
    /// </summary>
    [Fact]
    public void TryCreateWithWhitespaceOnlyReturnsFalse()
    {
        // Arrange
        string input = "   ";

        // Act
        bool result = EntityName.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given only whitespace");
    }

    /// <summary>
    /// Ensures that <see cref="EntityName.TryCreate"/> returns false when the input exceeds 100 characters.
    /// </summary>
    [Fact]
    public void TryCreateWithTooLongStringReturnsFalse()
    {
        // Arrange
        string input = new string('a', 101);

        // Act
        bool result = EntityName.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when name exceeds 100 characters");
    }

    /// <summary>
    /// Ensures that <see cref="EntityName.TryCreate"/> returns true and outputs a valid object when given a valid name.
    /// </summary>
    [Fact]
    public void TryCreateWithValidNameReturnsTrue()
    {
        // Arrange
        string input = "Valid Name";

        // Act
        bool result = EntityName.TryCreate(input, out var entityName);

        // Assert
        result.Should().BeTrue(because: "a properly formatted name should pass validation");
        entityName.Should().NotBeNull();
        entityName!.Name.Should().Be(input);
    }

    /// <summary>
    /// Ensures that <see cref="EntityName.Create"/> throws <see cref="ValidationException"/> for null input.
    /// </summary>
    [Fact]
    public void CreateWithNullStringThrowsValidationException()
    {
        // Arrange
        string? input = null;

        // Act
        Action act = () => EntityName.Create(input);

        // Assert
        act.Should().Throw<ValidationException>()
           .WithMessage("The name must be non-empty and no more than 100 characters long.");
    }

    /// <summary>
    /// Ensures that <see cref="EntityName.Create"/> throws <see cref="ValidationException"/> for empty input.
    /// </summary>
    [Fact]
    public void CreateWithEmptyStringThrowsValidationException()
    {
        // Arrange
        string input = "";

        // Act
        Action act = () => EntityName.Create(input);

        // Assert
        act.Should().Throw<ValidationException>()
           .WithMessage("The name must be non-empty and no more than 100 characters long.");
    }

    /// <summary>
    /// Ensures that <see cref="EntityName.Create"/> throws <see cref="ValidationException"/> for overly long input.
    /// </summary>
    [Fact]
    public void CreateWithTooLongStringThrowsValidationException()
    {
        // Arrange
        string input = new string('x', 101);

        // Act
        Action act = () => EntityName.Create(input);

        // Assert
        act.Should().Throw<ValidationException>()
           .WithMessage("The name must be non-empty and no more than 100 characters long.");
    }

    /// <summary>
    /// Ensures that <see cref="EntityName.Create"/> returns a valid <see cref="EntityName"/> object when given valid input.
    /// </summary>
    [Fact]
    public void CreateWithValidNameReturnsEntityName()
    {
        // Arrange
        string input = "Theme Park Entrance";

        // Act
        var result = EntityName.Create(input);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(input);
    }

    /// <summary>
    /// Verifies that two <see cref="EntityName"/> instances with the same value are considered equal.
    /// </summary>
    [Fact]
    public void EntityNamesWithSameValueShouldBeEqual()
    {
        // Arrange
        var name1 = EntityName.Create("Attraction Zone A");
        var name2 = EntityName.Create("Attraction Zone A");

        // Act & Assert
        name1.Should().Be(name2, because: "value objects with same value should be equal");
    }

    /// <summary>
    /// Verifies that two <see cref="EntityName"/> instances with different values are not considered equal.
    /// </summary>
    [Fact]
    public void EntityNamesWithDifferentValuesShouldNotBeEqual()
    {
        // Arrange
        var name1 = EntityName.Create("Roller Coaster");
        var name2 = EntityName.Create("Ferris Wheel");

        // Act & Assert
        name1.Should().NotBe(name2, because: "value objects with different values should not be equal");
    }
}
