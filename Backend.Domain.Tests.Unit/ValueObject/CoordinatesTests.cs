using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject;

/// <summary>
/// Unit tests for the <see cref="Coordinates"/> value object,
/// verifying its creation and logic.
/// </summary>
public class CoordinatesTests
{
    /// <summary>
    /// Ensures that <see cref="Coordinates.Create"/> creates the object correctly when inputs are OK.
    /// </summary>
    [Fact]
    public void Create_WithValidValues_ShouldReturnCoordinates()
    {
        double x = 10.0, y = 20.0, z = 30.0;

        var coordinates = Coordinates.Create(x, y, z);

        coordinates.X.Should().Be(x);
        coordinates.Y.Should().Be(y);
        coordinates.Z.Should().Be(z);
    }


    /// <summary>
    /// Ensures that <see cref="Coordinates.Create"/> allows all negative values.
    /// </summary>
    [Fact]
    public void Create_WithValidNegativeValues_ShouldReturnCoordinates()
    {
        double x = -10.1, y = -20.3, z = -30.2;

        var coordinates = Coordinates.Create(x, y, z);

        coordinates.X.Should().Be(x);
        coordinates.Y.Should().Be(y);
        coordinates.Z.Should().Be(z);
    }


    /// <summary>
    /// Ensures that <see cref="Coordinates.TryCreate"/> returns true and outputs a valid <see cref="Coordinates"/> 
    /// object for any set of double values.
    /// </summary>
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1.5, 2.6, 3.7)]
    [InlineData(-5.5, 0.0, 12.1)]
    [InlineData(double.MaxValue, double.MinValue, 0.0)]
    public void TryCreate_WithValidValues_Outvar_ShouldReturnTrue(double x, double y, double z)
    {
        var result = Coordinates.TryCreate(x, y, z, out var coordinates);

        result.Should().BeTrue();
        coordinates.Should().NotBeNull();
        coordinates!.X.Should().Be(x);
        coordinates.Y.Should().Be(y);
        coordinates.Z.Should().Be(z);
    }


    /// <summary>
    /// Ensures that <see cref="Coordinates.TryCreate"/> returns true and outputs a valid <see cref="Coordinates"/> 
    /// object when all the inputs are 0 or negative.
    /// </summary>
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(-1.5, -2.6, -3.7)]
    [InlineData(-5.5, 0.0, -12.1)]
    [InlineData(double.MaxValue, double.MinValue, 0.0)]
    public void TryCreate_WithValidNegativeValues_Outvar_ShouldReturnTrue(double x, double y, double z)
    {
        var result = Coordinates.TryCreate(x, y, z, out var coordinates);

        result.Should().BeTrue();
        coordinates.Should().NotBeNull();
        coordinates!.X.Should().Be(x);
        coordinates.Y.Should().Be(y);
        coordinates.Z.Should().Be(z);
    }


    /// <summary>
    /// Verifies that two <see cref="Coordinates"/> instances with the same values are considered equal.
    /// </summary>
    [Fact]
    public void Coordinates_WithSameValues_ShouldBeEqual()
    {
        var c1 = Coordinates.Create(1.1, 2.2, 3.3);
        var c2 = Coordinates.Create(1.1, 2.2, 3.3);

        c1.Should().Be(c2);
    }

    /// <summary>
    /// Verifies that two <see cref="Coordinates"/> instances with the same values are considered equal.
    /// </summary>
    [Fact]
    public void Coordinates_WithSameNegativeValues_ShouldBeEqual()
    {
        var c1 = Coordinates.Create(-1.1, -2.2, -3.3);
        var c2 = Coordinates.Create(-1.1, -2.2, -3.3);

        c1.Should().Be(c2);
    }

    /// <summary>
    /// Verifies that two <see cref="Coordinates"/> instances with different values are not considered equal.
    /// </summary>
    [Fact]
    public void Coordinates_WithDifferentValues_ShouldNotBeEqual()
    {
        var c1 = Coordinates.Create(1.1, 2.2, 3.3);
        var c2 = Coordinates.Create(3.3, 2.2, 1.1);

        c1.Should().NotBe(c2);
    }
}
