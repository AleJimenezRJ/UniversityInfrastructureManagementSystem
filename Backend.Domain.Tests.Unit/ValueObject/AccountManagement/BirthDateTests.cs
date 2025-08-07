using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject.AccountManagement;

/// <summary>
/// Unit tests for the <see cref="BirthDate"/> value object.
/// </summary>
public class BirthDateTests
{
    /// <summary>
    /// Tests that TryCreate returns false when the input date is null.
    /// </summary>
    [Fact]
    public void TryCreate_WithNullDate_ReturnsFalse()
    {
        // Arrange
        DateOnly? input = null;

        // Act
        bool result = BirthDate.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "a null date is not valid");
    }

    /// <summary>
    /// Tests that TryCreate returns false when the input date is invalid (not in "yyyy-MM-dd" format).
    /// </summary>
    [Fact]
    public void TryCreate_WithValidDate_ReturnsTrue()
    {
        // Arrange
        DateOnly input = new(2000, 01, 01);

        // Act
        bool result = BirthDate.TryCreate(input, out var output);

        // Assert
        result.Should().BeTrue();
        output!.Value.Should().Be(input);
    }

    /// <summary>
    /// Tests that TryCreate returns true for boundary dates (minimum and maximum valid dates).
    /// </summary>
    /// <param name="year"> The year of the date to test.</param>
    /// <param name="month"> The month of the date to test.</param>
    /// <param name="day"> The day of the date to test.</param>
    [Theory]
    [InlineData(1, 1, 1)]
    [InlineData(9999, 12, 31)]
    public void TryCreate_WithBoundaryDates_ReturnsTrue(int year, int month, int day)
    {
        // Arrange
        var date = new DateOnly(year, month, day);

        // Act
        var result = BirthDate.TryCreate(date, out var output);

        // Assert
        result.Should().BeTrue();
        output!.Value.Should().Be(date);
    }

    /// <summary>
    /// Tests that Create throws a ValidationException when the input date is null.
    /// </summary>
    [Fact]
    public void Create_WithNullDate_ThrowsValidationException()
    {
        // Arrange
        DateOnly? input = null;

        // Act
        Action action = () => BirthDate.Create(input);

        // Assert
        action.Should().ThrowExactly<ValidationException>(because: "null is not a valid birth date");
    }

    /// <summary>
    /// Tests that Create returns a BirthDate instance when given a valid date.
    /// </summary>
    /// <param name="year"> The year of the date to test.</param>
    /// <param name="month"> The month of the date to test.</param>
    /// <param name="day"> The day of the date to test.</param>
    [Theory]
    [InlineData(2001, 5, 21)]
    [InlineData(1990, 12, 31)]
    [InlineData(2020, 1, 1)]
    [InlineData(2024, 2, 29)]
    public void Create_WithValidDate_ReturnsBirthDateInstance(int year, int month, int day)
    {
        // Arrange
        DateOnly input = new(year, month, day);

        // Act
        var result = BirthDate.Create(input);

        // Assert
        result.Should().NotBeNull(because: "a valid date should return a BirthDate instance");
        result.Value.Should().Be(input, because: "the value inside BirthDate should match the input");
    }

}
