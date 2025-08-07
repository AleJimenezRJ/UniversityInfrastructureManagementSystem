using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Dtos.Spaces;

public class FloorDtoTests
{
    /// <summary>
    /// Tests that the <see cref="FloorDto"/> constructor correctly assigns the <c>FloorId</c> property when valid
    /// arguments are provided.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsFloorIdCorrectly()
    {
        // Arrange
        int expectedId = 1;
        int expectedNumber = 2;

        // Act
        var dto = new FloorDto(expectedId, expectedNumber);

        // Assert
        dto.FloorId.Should().Be(expectedId, because: "constructor should assign FloorId");
    }

    /// <summary>
    /// Tests that the <see cref="FloorDto"/> constructor correctly assigns the floor number when provided with valid
    /// arguments.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsFloorNumberCorrectly()
    {
        // Arrange
        int expectedId = 1;
        int expectedNumber = 2;

        // Act
        var dto = new FloorDto(expectedId, expectedNumber);

        // Assert
        dto.FloorNumber.Should().Be(expectedNumber, because: "constructor should assign FloorNumber");
    }

    /// <summary>
    /// Tests that two <see cref="FloorDto"/> instances with the same property values are equal.
    /// </summary>
    [Fact]
    public void Equality_WithSameValues_ReturnsTrue()
    {
        // Arrange
        var dto1 = new FloorDto(1, 2);
        var dto2 = new FloorDto(1, 2);

        // Act & Assert
        dto1.Should().Be(dto2, because: "records with same values should be equal");
        (dto1 == dto2).Should().BeTrue(because: "record equality operator should return true for same values");
    }

    /// <summary>
    /// Tests that two <see cref="FloorDto"/> instances with different property values are not equal.
    /// </summary>
    [Fact]
    public void Equality_WithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var dto1 = new FloorDto(1, 2);
        var dto2 = new FloorDto(2, 3);

        // Act & Assert
        dto1.Should().NotBe(dto2, because: "records with different values should not be equal");
        (dto1 != dto2).Should().BeTrue(because: "record inequality operator should return true for different values");
    }
}
