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
/// Provides unit tests for verifying the functionality of the <see cref="ProjectorDtoMapper"/> class, including mapping
/// between <c>Projector</c> and <c>ProjectorDto</c> objects.
/// </summary>
/// <remarks>This test class ensures the integrity of the mapping process between domain entities and DTOs,
/// covering scenarios such as property mapping, validation, and exception handling.</remarks>
public class ProjectorDtoMappersTests
{

    /// <summary>
    /// Tests that the <see cref="ProjectorDtoMapper.ToDto"/> method correctly maps a <c>Projector</c> object  to a
    /// <c>ListProjectorDto</c> object, ensuring all properties are accurately transferred.
    /// </summary>
    /// <remarks>This test verifies the mapping of various properties, including content, dimensions,
    /// position,  orientation, and projection area, to ensure the integrity of the mapping process.</remarks>
    [Fact]
    public void ToDto_ShouldMapProjectorToProjectorDto()
    {
        // Arrange
        var projector = CreateTestProjector();
        var mapper = new ProjectorDtoMapper();

        // Act
        var dto = mapper.ToDto(projector);

        // Assert
        dto.Should().NotBeNull();
        dto.ProjectedContent.Should().Be("Content");
        dto.ProjectionArea.ProjectedHeight.Should().Be(5.0);
        dto.ProjectionArea.ProjectedWidth.Should().Be(5.0);
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
    /// Tests that the <see cref="ProjectorDtoMapper.ToEntity(ProjectorDto)"/> method correctly maps a <see
    /// cref="ProjectorDto"/> instance to a <see cref="Projector"/> entity.
    /// </summary>
    /// <remarks>This test verifies that all properties of the <see cref="ProjectorDto"/> object are
    /// accurately mapped to the corresponding properties of the <see cref="Projector"/> entity, including nested
    /// objects such as <see cref="ProjectionAreaDto"/> and <see cref="PositionDto"/>.</remarks>
    [Fact]
    public void ToEntity_ShouldMapDtoToProjector()
    {
        // Arrange
        var mapper = new ProjectorDtoMapper();
        var dto = new ProjectorDto
        (
            Id: 1,
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
        projector.ComponentId.Should().Be(1);
        projector.Orientation.Value.Should().Be("North");
        projector.Position.X.Should().Be(2.5);
        projector.Position.Y.Should().Be(3.0);
        projector.Position.Z.Should().Be(4.0);
        projector.Dimensions.Width.Should().Be(15);
        projector.Dimensions.Length.Should().Be(20);
        projector.Dimensions.Height.Should().Be(10);
    }


    /// <summary>
    /// Tests that the <see cref="ProjectorDtoMapper.ToEntity(ProjectorDto)"/> method throws a <see
    /// cref="ValidationException"/> when the provided <see cref="ProjectorDto"/> contains invalid values.
    /// </summary>
    /// <remarks>This test verifies that the <see cref="ValidationException"/> includes specific error details
    /// for invalid parameters, such as "Orientation", "Dimensions", and "Projection Area". It ensures that the
    /// validation logic in the mapping process correctly identifies and reports invalid input data.</remarks>
    [Fact]
    public void ToEntity_ShouldThrowException_WhenInvalid()
    {
        // Arrange
        var mapper = new ProjectorDtoMapper();
        var dto = new ProjectorDto
        (
            Id: 1,
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
