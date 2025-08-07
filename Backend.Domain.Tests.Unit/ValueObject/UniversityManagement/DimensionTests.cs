using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject.UniversityManagement;

/// <summary>
/// Unit tests for the <see cref="Dimension"/> value object,
/// verifying its creation and logic.
/// </summary>
public class DimensionTests
{
    /// <summary>
    /// Ensures that <see cref="Dimension.TryCreate"/> returns true and outputs a valid <see cref="Dimension"/> 
    /// object given a correct input.
    /// </summary>
    [Theory]
    [InlineData(1.0, 2.0, 3.0)]
    [InlineData(0.1, 0.1, 0.1)]
    [InlineData(1000.0, 1000.0, 1000.0)]
    public void TryCreate_WithValidDimensions_ReturnsTrue(double width, double length, double height)
    {
        var result = Dimension.TryCreate(width, length, height, out var dimensions);

        result.Should().BeTrue();
        dimensions.Should().NotBeNull();
        dimensions!.Width.Should().Be(width);
        dimensions.Length.Should().Be(length);
        dimensions.Height.Should().Be(height);
    }

    /// <summary>
    /// Ensures that <see cref="Dimension.TryCreate"/> returns false when inputs are invalid
    /// </summary>
    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(3, 1, 0)]
    [InlineData(-1, 1, 1)]
    [InlineData(1, -1, 1)]
    [InlineData(1, 1, -1)]
    [InlineData(3, 0, -0)]
    public void TryCreate_WithInvalidDimensions_ReturnsFalse(double width, double length, double height)
    {
        var result = Dimension.TryCreate(width, length, height, out var dimensions);

        result.Should().BeFalse();
        dimensions.Should().BeNull();
    }

    /// <summary>
    /// Ensures that <see cref="Dimension.Create"/> returns true and outputs a valid <see cref="Dimension"/> 
    /// object given a correct input.
    /// </summary>
    [Fact]
    public void Create_WithValidDimensions_ReturnsDimension()
    {
        var dimension = Dimension.Create(2.5, 3.5, 4.5);

        dimension.Width.Should().Be(2.5);
        dimension.Length.Should().Be(3.5);
        dimension.Height.Should().Be(4.5);
    }

    /// <summary>
    /// Ensures that <see cref="Dimension.Create"/> returns false when inputs are invalid.
    /// </summary>
    [Fact]
    public void Create_WithInvalidDimensions_ThrowsValidationException()
    {
        Action act = () => Dimension.Create(0, 1, 1);

        act.Should().Throw<ValidationException>()
            .WithMessage("Invalid Dimension: 0, 1, 1");
    }

    /// <summary>
    /// Ensures that <see cref="Dimension.Create"/> returns true when inputs are valid and equal.
    /// </summary>
    [Fact]
    public void Equality_WithSameValues_ShouldBeEqual()
    {
        var dim1 = Dimension.Create(5, 6, 7);
        var dim2 = Dimension.Create(5, 6, 7);

        dim1.Should().Be(dim2);
        dim1.GetHashCode().Should().Be(dim2.GetHashCode());
    }

    /// <summary>
    /// Ensures that <see cref="Dimension.Create"/> returns false when inputs are valid and not equal.
    /// </summary>
    [Fact]
    public void Equality_WithDifferentValues_ShouldNotBeEqual()
    {
        var dim1 = Dimension.Create(5, 6, 7);
        var dim2 = Dimension.Create(5, 6, 8);

        dim1.Should().NotBe(dim2);
    }
}
