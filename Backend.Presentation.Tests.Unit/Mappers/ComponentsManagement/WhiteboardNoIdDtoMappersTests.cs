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
/// Provides unit tests for the <see cref="WhiteboardNoIdDtoMapper"/> class, ensuring correct mapping between
/// <c>Whiteboard</c> entities and <c>WhiteboardNoIdDto</c> objects.
/// </summary>
/// <remarks>This test class includes methods to verify the accuracy and integrity of mapping operations performed
/// by the <see cref="WhiteboardNoIdDtoMapper"/>. It ensures that properties such as orientation, position, dimensions,
/// and marker color are correctly transferred between entities and DTOs. Additionally, it validates error handling and
/// exception scenarios for invalid input data.</remarks>
public class WhiteboardNoIdDtoMappersTests
{
    /// <summary>
    /// Tests the mapping of a <see cref="Whiteboard"/> object to a <see cref="WhiteboardNoIdDto"/> object.
    /// </summary>
    /// <remarks>This test verifies that all properties of the <see cref="Whiteboard"/> object are correctly
    /// mapped to the corresponding properties of the <see cref="WhiteboardNoIdDto"/> object, including nested objects
    /// such as position and dimensions.</remarks>
    [Fact]
    public void ToDto_ShouldMapWhiteboardToWhiteboardNoIdDto()
    {
        // Arrange
        var mapper = new WhiteboardNoIdDtoMapper();
        var whiteboard = CreateTestWhiteboard();

        // Act
        var dto = mapper.ToDto(whiteboard);

        // Assert
        dto.Should().NotBeNull();
        dto.MarkerColor.Should().Be("Red");
        dto.Orientation.Should().Be("North");
        dto.Position.X.Should().Be(2.5);
        dto.Position.Y.Should().Be(3.0);
        dto.Position.Z.Should().Be(4.0);
        dto.Dimensions.Width.Should().Be(15);
        dto.Dimensions.Length.Should().Be(20);
        dto.Dimensions.Height.Should().Be(10);
    }


    /// <summary>
    /// Tests that the <see cref="WhiteboardNoIdDtoMapper.ToEntity"/> method correctly maps a  <see
    /// cref="WhiteboardNoIdDto"/> instance to a <see cref="Whiteboard"/> entity.
    /// </summary>
    /// <remarks>This test verifies that all properties of the <see cref="WhiteboardNoIdDto"/> object,
    /// including  orientation, position, dimensions, and marker color, are accurately transferred to the corresponding 
    /// properties of the <see cref="Whiteboard"/> entity.</remarks>
    [Fact]
    public void ToEntity_ShouldMapDtoToWhiteboard()
    {
        // Arrange
        var mapper = new WhiteboardNoIdDtoMapper();
        var dto = new WhiteboardNoIdDto
        (
            Orientation: "North",
            Position: new PositionDto(2.5, 3.0, 4.0),
            Dimensions: new DimensionsDto(15, 20, 10),
            MarkerColor: "Red"
        );

        // Act
        var whiteboard = mapper.ToEntity(dto);

        // Assert
        whiteboard.Should().NotBeNull();
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
    /// validation on the      <see cref="WhiteboardDto"/> properties and throws a <see cref="ValidationException"/>
    /// when any property      contains invalid data. Specifically, it checks for validation errors related to the
    /// "Orientation",      "Dimensions", and "Color" properties.</remarks>
    [Fact]
    public void ToEntity_ShouldThrowException_WhenInvalid()
    {
        // Arrange
        var mapper = new WhiteboardNoIdDtoMapper();
        var dto = new WhiteboardNoIdDto
        (
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
    /// <remarks>This method is intended for testing purposes and returns a <see cref="Whiteboard"/> object
    /// with hardcoded values.</remarks>
    /// <returns>A <see cref="Whiteboard"/> instance initialized with predefined marker color, orientation, coordinates, and
    /// dimensions.</returns>
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
