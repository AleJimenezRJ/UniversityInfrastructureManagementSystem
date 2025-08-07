using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject;

public class Area2DTests
{
    /// <summary>
    /// Verifies that attempting to create an <see cref="Area2D"/> instance with zero
    /// values for length or height returns false.
    /// </summary>
    /// <param name="length">The length of the area being tested. Expected to be zero in the test cases.</param>
    /// <param name="height">The height of the area being tested. Expected to be zero in the test cases.</param>
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public void TryCreate_WithCeroValue_ReturnsFalse(double length, double height)
    {
        // Act
        var result = Area2D.TryCreate(length, height, out var _);

        // Assert
        result.Should().BeFalse(because: "zero as an input is invalid");
    }

    /// <summary>
    /// Verifies that attempting to create an <see cref="Area2D"/> instance with negative
    /// values for length or height returns false.
    /// </summary>
    /// <param name="length">The negative length of the area being tested.</param>
    /// <param name="height">The negative height of the area being tested.</param>
    [Theory]
    [InlineData(-1, -1)]
    [InlineData(1, -1)]
    [InlineData(-1, 1)]
    [InlineData(-999, -999999)]
    public void TryCreate_WithNegativeValues_ReturnsFalse(double length, double height)
    {
        // Act
        var result = Area2D.TryCreate(length, height, out var _);

        // Assert
        result.Should().BeFalse(because: "negative input is invalid");
    }

    /// <summary>
    /// Verifies that attempting to create an <see cref="Area2D"/> instance with positive
    /// values for both length and height returns true.
    /// </summary>
    [Fact]
    public void TryCreate_WithPositiveValues_ReturnsTrue()
    {
        // Arrange
        double length = 2;
        double height = 3;

        // Act
        var result = Area2D.TryCreate(length, height, out var _);

        // Assert
        result.Should().BeTrue(because: "successfully created");
    }

    /// <summary>
    /// Ensures that attempting to create an <see cref="Area2D"/> instance with zero values
    /// for length or height throws a <see cref="ValidationException"/>.
    /// </summary>
    /// <param name="length">The length of the area being created. Expected to be zero in the test cases.</param>
    /// <param name="height">The height of the area being created. Expected to be zero in the test cases.</param>
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public void Create_WithCeroValues_ThrowsValidationException(double length, double height)
    {
        // Act
        Action act = () => Area2D.Create(length, height);

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage($"Invalid values for projected area: {length}, {height}");
    }

    /// <summary>
    /// Validates that attempting to create an <see cref="Area2D"/> instance with negative
    /// values for length or height results in a <see cref="ValidationException"/> being thrown.
    /// </summary>
    /// <param name="length">The length of the area being tested. Expected to be a negative value in this test.</param>
    /// <param name="height">The height of the area being tested. Expected to be a negative value in this test.</param>
    [Theory]
    [InlineData(-1, -1)]
    [InlineData(1, -1)]
    [InlineData(-1, 1)]
    public void Create_WithNegativeValues_ThrowsValidationException(double length, double height)
    {
        // Act
        Action act = () => Area2D.Create(length, height);

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage($"Invalid values for projected area: {length}, {height}");
    }

    /// <summary>
    /// Verifies that creating an <see cref="Area2D"/> instance with valid positive
    /// values for length and height results in a properly constructed instance.
    /// </summary>
    [Fact]
    public void Create_WithPositiveValues_ReturnsInstance()
    {
        // Arrange
        double length = 2.5;
        double height = 3;

        // Act
        var result = Area2D.Create(length, height);

        // Assert
        result.Length.Should().Be(length);
        result.Height.Should().Be(height);
    }

    /// <summary>
    /// Verifies that two <see cref="Area2D"/> instances with identical length and height values are considered equal.
    /// </summary>
    [Fact]
    public void Area2D_Equality_WithSameValue_ReturnsTrue()
    {
        // Arrange
        var a = Area2D.Create(2.5, 3);
        var b = Area2D.Create(2.5, 3);

        // Act & Assert
        a.Should().Be(b, because: "value objects with same value should be equal");
    }

    /// <summary>
    /// Verifies that two instances of <see cref="Area2D"/> with different values for length or height
    /// are not considered equal.
    /// </summary>
    [Fact]
    public void Area2D_Equality_WithDifferentValue_ReturnsFalse()
    {
        // Arrange
        var a = Area2D.Create(2.5, 3);
        var b = Area2D.Create(5, 10);

        // Act & Assert
        a.Should().NotBe(b, because: "value objects with different value should be different");
    }
}
