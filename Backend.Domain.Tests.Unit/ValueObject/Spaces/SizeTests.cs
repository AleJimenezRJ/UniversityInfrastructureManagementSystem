using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject.Spaces;

public class SizeTests
{
    /// <summary>
    /// Ensures that TryCreate returns true for valid positive values
    /// </summary>
    /// <param name="input">A positive double value</param>
    [Theory]
    [InlineData(0.999)]
    [InlineData(1)]
    [InlineData(10.99)]
    [InlineData(1000.9999)]
    [InlineData(int.MaxValue)]
    [InlineData(double.MaxValue)]
    [InlineData(double.PositiveInfinity)]
    public void TryCreate_WithPositiveValue_ReturnsTrue(double input)
    {
        // Act
        bool result = Size.TryCreate(input, out var _);

        // Assert
        result.Should().BeTrue(because: "validation should succeed for positive values");
    }

    /// <summary>
    /// Ensures that TryCreate saves the value correctly when the input is a positive double
    /// </summary>
    /// <param name="input">A positive double value</param>
    [Theory]
    [InlineData(0.999)]
    [InlineData(1)]
    [InlineData(10.99)]
    [InlineData(1000.9999)]
    [InlineData(int.MaxValue)]
    [InlineData(double.MaxValue)]
    [InlineData(double.PositiveInfinity)]
    public void TryCreate_WithPositiveValue_SavesValue(double input)
    {
        // Act
        Size.TryCreate(input, out var size);

        // Assert
        size!.Value.Should().Be(input, because: "the Size's value should match the input double when validation passes");
    }

    /// <summary>
    /// Ensures that TryCreate returns false for zero or negative values
    /// </summary>
    /// <param name="input">A zero or negative double value</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10.99)]
    [InlineData(-1000.9999)]
    [InlineData(int.MinValue)]
    [InlineData(double.MinValue)]
    [InlineData(double.NegativeInfinity)]
    public void TryCreate_WithZeroOrNegativeValue_ReturnsFalse(double input)
    {
        // Act
        bool result = Size.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given a value of <= 0");
    }

    /// <summary>
    /// Ensures that TryCreate returns null for zero or negative values
    /// </summary>
    /// <param name="input">A zero or negative double value</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10.99)]
    [InlineData(-1000.9999)]
    [InlineData(int.MinValue)]
    [InlineData(double.MinValue)]
    [InlineData(double.NegativeInfinity)]
    public void TryCreate_WithZeroOrNegativeValue_ReturnsNull(double input)
    {
        // Act
        Size.TryCreate(input, out var size);

        // Assert
        size.Should().BeNull(because: "the out variable should be null when validation fails");
    }

    /// <summary>
    /// Ensures that Create creates a Size object when given a positive value
    /// </summary>
    /// <param name="input">A positive double value</param>
    [Theory]
    [InlineData(0.999)]
    [InlineData(1)]
    [InlineData(10.99)]
    [InlineData(1000.9999)]
    [InlineData(int.MaxValue)]
    [InlineData(double.MaxValue)]
    [InlineData(double.PositiveInfinity)]
    public void Create_WithPositiveValue_DoesntReturnANullSize(double input)
    {
        // Act
        var result = Size.Create(input);

        // Assert
        result.Should().NotBeNull(because: "the Size should be created successfully when the input is valid");
    }

    /// <summary>
    /// Ensures that Create returns a Size object with the correct value when given a positive value
    /// </summary>
    /// <param name="input">A positive double value</param>
    [Theory]
    [InlineData(0.999)]
    [InlineData(1)]
    [InlineData(10.99)]
    [InlineData(1000.9999)]
    [InlineData(int.MaxValue)]
    [InlineData(double.MaxValue)]
    [InlineData(double.PositiveInfinity)]
    public void Create_WithPositiveValue_ReturnsSizeWithCorrectValue(double input)
    {
        // Act
        var result = Size.Create(input);

        // Assert
        result.Value.Should().Be(input, because: "the Size's value should match the input double when validation passes");
    }

    /// <summary>
    /// Ensures that Create throws a ValidationException for zero or negative values
    /// </summary>
    /// <param name="input">A zero or negative double value</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10.99)]
    [InlineData(-1000.9999)]
    [InlineData(int.MinValue)]
    [InlineData(double.MinValue)]
    [InlineData(double.NegativeInfinity)]
    public void Create_WithZeroOrNegativeValue_ThrowsValidationException(double input)
    {
        // Act
        Action act = () => Size.Create(input);

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("the size value must be greater than 0.");
    }

    /// <summary>
    /// Ensures that two Size objects with the same value are considered equal
    /// </summary>
    [Fact]
    public void EqualityComparison_WithSameValue_ReturnsTrue()
    {
        // Arrange
        double input = 11.1;
        var size1 = Size.Create(input);
        var size2 = Size.Create(input);

        // Act
        var areEqual = size1.Equals(size2);

        // Assert
        areEqual.Should().BeTrue(because: "sizes with the same value should be considered equal");
    }

    /// <summary>
    /// Ensures that two Size objects with different values are not considered equal
    /// </summary>
    [Fact]
    public void EqualityComparison_WithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var size1 = Size.Create(11.1);
        var size2 = Size.Create(22.2);

        // Act
        var areEqual = size1.Equals(size2);

        // Assert
        areEqual.Should().BeFalse(because: "sizes with different values should not be considered equal");
    }
}
