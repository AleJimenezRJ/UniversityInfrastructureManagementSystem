using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.ComponentsManagement;

/// <summary>
/// Provides unit tests for verifying the functionality of the <see cref="WhiteboardDtoMapper"/> class.
/// </summary>
/// <remarks>This test class includes methods to validate the mapping between <see cref="Whiteboard"/> entities
/// and  <see cref="WhiteboardDto"/> objects, ensuring correctness and handling of edge cases.</remarks>
public class WhiteboardDtoMappersTests
{
    /// <summary>
    /// Tests that the <see cref="WhiteboardDtoMapper.ToDto"/> method correctly maps a <c>Whiteboard</c> object      to
    /// a <c>WhiteboardDto</c> object, ensuring all properties are accurately transferred.
    /// </summary>
    /// <remarks>This test verifies that the mapping process preserves the values of key properties such as   
    /// <c>MarkerColor</c>, <c>Id</c>, <c>Orientation</c>, <c>Position</c>, and <c>Dimensions</c>.      It ensures that
    /// the resulting <c>WhiteboardDto</c> object is not null and matches the expected values.</remarks>
    [Fact]
    public void ToDto_ShouldMapWhiteboardToWhiteboardDto()
    {
        // Arrange
        var whiteboard = CreateTestWhiteboard();
        var mapper = new WhiteboardDtoMapper();

        // Act
        var dto = mapper.ToDto(whiteboard);

        // Assert
        dto.Should().NotBeNull();
        dto.MarkerColor.Should().Be("Red");
        dto.Id.Should().Be(1);
        dto.Orientation.Should().Be("North");
        dto.Position.X.Should().Be(2.5);
        dto.Position.Y.Should().Be(3.0);
        dto.Position.Z.Should().Be(4.0);
        dto.Dimensions.Width.Should().Be(15);
        dto.Dimensions.Length.Should().Be(20);
        dto.Dimensions.Height.Should().Be(10);
    }


    /// <summary>
    /// Tests that the <see cref="WhiteboardDtoMapper.ToEntity"/> method correctly maps a <see cref="WhiteboardDto"/> to
    /// a <see cref="Whiteboard"/> entity.
    /// </summary>
    /// <remarks>This test verifies that all properties of the <see cref="WhiteboardDto"/> object, including
    /// nested objects such as <see cref="PositionDto"/> and <see cref="DimensionsDto"/>, are accurately mapped to their
    /// corresponding properties in the <see cref="Whiteboard"/> entity. It ensures that the mapping logic preserves
    /// data integrity and handles all fields correctly.</remarks>
    [Fact]
    public void ToEntity_ShouldMapDtoToWhiteboard()
    {
        // Arrange
        var mapper = new WhiteboardDtoMapper();
        var dto = new WhiteboardDto
        (
            Id: 1,
            Orientation: "North",
            Position: new PositionDto(2.5, 3.0, 4.0),
            Dimensions: new DimensionsDto(15, 20, 10),
            MarkerColor: "Red"
        );

        // Act
        var whiteboard = mapper.ToEntity(dto);

        // Assert
        whiteboard.Should().NotBeNull();
        whiteboard.ComponentId.Should().Be(1);
        whiteboard.Orientation.Value.Should().Be("North");
        whiteboard.Position.X.Should().Be(2.5);
        whiteboard.Position.Y.Should().Be(3.0);
        whiteboard.Position.Z.Should().Be(4.0);
        whiteboard.Dimensions.Width.Should().Be(15);
        whiteboard.Dimensions.Length.Should().Be(20);
        whiteboard.Dimensions.Height.Should().Be(10);
        whiteboard.MarkerColor!.Value.Should().Be("Red");
    }


    /// <summary>
    /// Tests that the <see cref="WhiteboardDtoMapper.ToEntity"/> method throws a <see cref="ValidationException"/> 
    /// when the provided <see cref="WhiteboardDto"/> contains invalid values.
    /// </summary>
    /// <remarks>This test verifies that the <see cref="WhiteboardDtoMapper.ToEntity"/> method performs
    /// validation on the  <see cref="WhiteboardDto"/> properties and throws a <see cref="ValidationException"/> when
    /// any property  contains invalid data. Specifically, it checks for validation errors related to the "Orientation",
    /// "Dimensions", and "Color" properties.</remarks>
    [Fact]
    public void ToEntity_ShouldThrowException_WhenInvalid()
    {
        // Arrange
        var mapper = new WhiteboardDtoMapper();
        var dto = new WhiteboardDto
        (
            Id: 1,
            Orientation: "hello world",
            Position: new PositionDto(2.5, 3.0, 4.0),
            Dimensions: new DimensionsDto(-1, -1, -1),
            MarkerColor: "Hello world" 
        );

        // Act
        var act = () => mapper.ToEntity(dto);

        // Assert
        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain(e => e.Parameter == "Orientation")
            .And.Contain(e => e.Parameter == "Dimensions")
            .And.Contain(e => e.Parameter == "Color");
    }


    /// <summary>
    /// Creates a new instance of a <see cref="Whiteboard"/> object configured with test values.
    /// </summary>
    /// <returns>A <see cref="Whiteboard"/> instance initialized with predefined test parameters, including marker color,
    /// orientation, coordinates, and dimensions.</returns>
    private static Whiteboard CreateTestWhiteboard()
    {
        var whiteboard = new Whiteboard(
            markerColor: Colors.Create("Red"),
            1,
            Orientation.Create("North"),
            new Coordinates(2.5, 3.0, 4.0),
            new Dimension(15, 20, 10)
        );

        return whiteboard;
    }
}
