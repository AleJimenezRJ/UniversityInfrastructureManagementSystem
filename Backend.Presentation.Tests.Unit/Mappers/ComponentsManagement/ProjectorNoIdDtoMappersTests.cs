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
/// Provides unit tests for verifying the functionality of the <see cref="ProjectorNoIdDtoMapper"/> class, including
/// mapping between <see cref="Projector"/> entities and <see cref="ProjectorNoIdDto"/> objects.
/// </summary>
/// <remarks>This test class ensures the correctness of mapping operations performed by the <see
/// cref="ProjectorNoIdDtoMapper"/>. It includes tests for mapping properties, handling invalid input, and verifying
/// exceptions.</remarks>
public class ProjectorNoIdDtoMappersTests
{

    /// <summary>
    /// Tests the mapping of a <see cref="Projector"/> object to a <see cref="ProjectorNoIdDto"/> object.
    /// </summary>
    /// <remarks>This test verifies that all properties of the <see cref="Projector"/> object are correctly
    /// mapped to the corresponding properties of the <see cref="ProjectorNoIdDto"/> object. It ensures that the mapping
    /// logic in <see cref="ProjectorNoIdDtoMapper.ToDto(Projector)"/> produces the expected results.</remarks>
    [Fact]
    public void ToDto_ShouldMapProjectorToProjectorNoIdDto()
    {
        // Arrange
        var mapper = new ProjectorNoIdDtoMapper();
        var projector = CreateTestProjector();

        // Act
        var dto = mapper.ToDto(projector);

        // Assert
        dto.Should().NotBeNull();
        dto.ProjectedContent.Should().Be("Content");
        dto.ProjectionArea.ProjectedHeight.Should().Be(5.0);
        dto.ProjectionArea.ProjectedWidth.Should().Be(5.0);
        dto.Orientation.Should().Be("North");
        dto.Position.X.Should().Be(2.5);
        dto.Position.Y.Should().Be(3.0);
        dto.Position.Z.Should().Be(4.0);
        dto.Dimensions.Width.Should().Be(15);
        dto.Dimensions.Length.Should().Be(20);
        dto.Dimensions.Height.Should().Be(10);
    }


    /// <summary>
    /// Tests that the <see cref="ProjectorNoIdDtoMapper.ToEntity"/> method correctly maps a  <see
    /// cref="ProjectorNoIdDto"/> instance to a <see cref="Projector"/> entity.
    /// </summary>
    /// <remarks>This test verifies that all properties of the <see cref="ProjectorNoIdDto"/> object,
    /// including  orientation, position, dimensions, projection area, and projected content, are accurately mapped  to
    /// the corresponding properties of the <see cref="Projector"/> entity.</remarks>
    [Fact]
    public void ToEntity_ShouldMapDtoToProjector()
    {
        // Arrange
        var mapper = new ProjectorNoIdDtoMapper();
        var dto = new ProjectorNoIdDto
        (
            Orientation: "North",
            Position: new PositionDto(2.5, 3.0, 4.0),
            Dimensions: new DimensionsDto(15, 20, 10),
            ProjectionArea: new ProjectionAreaDto(5.0, 5.0),
            ProjectedContent: "Content"
        );

        // Act
        var projector = mapper.ToEntity(dto);

        // Assert
        projector.Should().NotBeNull();
        projector.ProjectedContent.Should().Be("Content");
        projector.ProjectionArea.Should().NotBeNull();
        projector.ProjectionArea.Height.Should().Be(5.0);
        projector.ProjectionArea.Length.Should().Be(5.0);
        projector.Orientation.Value.Should().Be("North");
        projector.Position.X.Should().Be(2.5);
        projector.Position.Y.Should().Be(3.0);
        projector.Position.Z.Should().Be(4.0);
        projector.Dimensions.Width.Should().Be(15);
        projector.Dimensions.Length.Should().Be(20);
        projector.Dimensions.Height.Should().Be(10);
    }


    /// <summary>
    /// Tests that the <see cref="ProjectorNoIdDtoMapper.ToEntity"/> method throws a <see cref="ValidationException"/> 
    /// when the provided <see cref="ProjectorNoIdDto"/> contains invalid values.
    /// </summary>
    /// <remarks>This test verifies that the <see cref="ProjectorNoIdDtoMapper.ToEntity"/> method performs
    /// validation on the input  DTO and throws a <see cref="ValidationException"/> when any of the following conditions
    /// are met: <list type="bullet"> <item><description>The <c>Orientation</c> property contains an invalid
    /// value.</description></item> <item><description>The <c>Dimensions</c> property contains negative
    /// values.</description></item> <item><description>The <c>ProjectionArea</c> property contains negative
    /// values.</description></item> </list> The test ensures that the validation errors include the specific parameters
    /// that failed validation.</remarks>
    [Fact]
    public void ToEntity_ShouldThrowException_WhenInvalid()
    {
        // Arrange
        var mapper = new ProjectorNoIdDtoMapper();
        var dto = new ProjectorNoIdDto
        (
            Orientation: "hello world",
            Position: new PositionDto(2.5, 3.0, 4.0),
            Dimensions: new DimensionsDto(-1, -1, -1),
            ProjectionArea: new ProjectionAreaDto(-1, -1),
            ProjectedContent: "Content"
        );

        // Act
        var act = () => mapper.ToEntity(dto);

        // Assert
        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain(e => e.Parameter == "Orientation")
            .And.Contain(e => e.Parameter == "Dimensions")
            .And.Contain(e => e.Parameter == "Projection Area");
    }



    /// <summary>
    /// Creates a new instance of a <see cref="Projector"/> configured for testing purposes.
    /// </summary>
    /// <returns>A <see cref="Projector"/> instance initialized with predefined values, including content type,  area dimensions,
    /// orientation, coordinates, and size.</returns>
    private static Projector CreateTestProjector()
    {
        var projector = new Projector(
            "Content",
            Area2D.Create(5.0, 5.0),
            1,
            Orientation.Create("North"),
            new Coordinates(2.5, 3.0, 4.0),
            new Dimension(15, 20, 10)
        );

        return projector;
    }
}
