using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject;

public class CapacityTests
{
    [Fact]
    public void TryCreate_WithNullValue_ReturnsFalse()
    {
        // Arrange
        int? input = null;

        // Act
        bool result = Capacity.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given a null value");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void TryCreate_WithNonPositiveValue_ReturnsFalse(int input)
    {
        // Act
        bool result = Capacity.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given a non-positive value");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public void TryCreate_WithPositiveValue_ReturnsTrueAndInstance(int input)
    {
        // Act
        bool result = Capacity.TryCreate(input, out var capacity);

        // Assert
        result.Should().BeTrue(because: "validation should succeed for positive values");
        capacity.Should().NotBeNull();
        capacity!.Value.Should().Be(input);
    }

    [Fact]
    public void Create_WithNullValue_ThrowsValidationException()
    {
        // Arrange
        int? input = null;

        // Act
        Action act = () => Capacity.Create(input);

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("The capacity must be an integer greater than zero.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Create_WithNonPositiveValue_ThrowsValidationException(int input)
    {
        // Act
        Action act = () => Capacity.Create(input);

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("The capacity must be an integer greater than zero.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(999)]
    public void Create_WithPositiveValue_ReturnsCapacity(int input)
    {
        // Act
        var result = Capacity.Create(input);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(input);
    }

    [Fact]
    public void EqualityComparison_WithSameValue_ReturnsTrue()
    {
        // Arrange
        int value = 42;
        var c1 = Capacity.Create(value);
        var c2 = Capacity.Create(value);

        // Act & Assert
        c1.Should().Be(c2, because: "value objects with the same value should be equal");
    }

    [Fact]
    public void EqualityComparison_WithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var c1 = Capacity.Create(10);
        var c2 = Capacity.Create(20);

        // Act & Assert
        c1.Should().NotBe(c2, because: "value objects with different values should not be equal");
    }
}
