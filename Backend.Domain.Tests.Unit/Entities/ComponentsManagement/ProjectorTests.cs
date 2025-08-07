using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.ComponentsManagement;

/*
Supervised Activity: PI June 11th
Eleni Gougani and Angelica Vargas

PBIs:
-CPD-LC-001-005 Update a projector (Issue #24)
-CPD-LC-001-004 Create a projector (Issue #22)
-CPD-LC-001-001 List components in a learning space (Issue #9)

Completed tasks:
-Unit tests cases for ProjectorServices class, including(GetProjectorAsync, AddProjectorAsync, UpdateProjectorAsync methods).
- Unit tests cases for Projector class, including constructor and properties (Dimensions, Coordinates, Orientation, Area2D, ProjectedContent).
*/

/// <summary>
/// Contains unit tests for the <see cref="Projector"/> class, ensuring that its constructor
/// correctly assigns the provided values to its properties, including <see cref="Dimension"/>,
/// <see cref="Coordinates"/>, <see cref="Orientation"/>, <see cref="Area2D"/>, and the projected content.
/// </summary>
public class ProjectorTests
{
    private readonly Dimension _dimensions;
    private readonly Coordinates _position;
    private readonly Orientation _orientation;
    private readonly Area2D _projectionArea;

    /// <summary>
    /// Initializes test data for the <see cref="ProjectorTests"/> class.
    /// </summary>
    public ProjectorTests()
    {
        _dimensions = Dimension.Create(100, 50, 30);
        _position = Coordinates.Create(1, 20, 30);
        _orientation = Orientation.Create("North");
        _projectionArea = Area2D.Create(2, 6);
    }

    /// <summary>
    /// Verifies that the <see cref="Projector"/> constructor assigns the <see cref="Dimension"/> property correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsDimensionCorrectly()
    {
        // Arrange
        string projectedContent = "Sample Content";

        // Act
        var projector = new Projector(projectedContent, _projectionArea, _orientation, _position, _dimensions);

        // Assert
        projector.Dimensions.Should().Be(_dimensions, because: "the constructor should assign the provided dimensions");
    }

    /// <summary>
    /// Verifies that the <see cref="Projector"/> constructor assigns the <see cref="Coordinates"/> property correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsPositionCorrectly()
    {
        // Arrange
        string projectedContent = "Sample Content";

        // Act
        var projector = new Projector(projectedContent, _projectionArea, _orientation, _position, _dimensions);

        // Assert
        projector.Position.Should().Be(_position, because: "the constructor should assign the provided position");
    }

    /// <summary>
    /// Verifies that the <see cref="Projector"/> constructor assigns the <see cref="Orientation"/> property correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsOrientationCorrectly()
    {
        // Arrange
        string projectedContent = "Sample Content";

        // Act
        var projector = new Projector(projectedContent, _projectionArea, _orientation, _position, _dimensions);

        // Assert
        projector.Orientation.Should().Be(_orientation, because: "the constructor should assign the provided orientation");
    }

    /// <summary>
    /// Verifies that the <see cref="Projector"/> constructor assigns the <see cref="Area2D"/> property correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsProjectionAreaCorrectly()
    {
        // Arrange
        string projectedContent = "Sample Content";

        // Act
        var projector = new Projector(projectedContent, _projectionArea, _orientation, _position, _dimensions);

        // Assert
        projector.ProjectionArea.Should().Be(_projectionArea, because: "the constructor should assign the provided projection area");
    }

    /// <summary>
    /// Verifies that the <see cref="Projector"/> constructor assigns the projected content correctly for various input values.
    /// </summary>
    /// <param name="input">The projected content to test.</param>
    [Theory]
    [InlineData("netflix.com")]
    [InlineData("A")]
    [InlineData("r")]
    public void Constructor_WithValidArguments_AssignsProjectedContentCorrectly(string input)
    {
        // Arrange

        // Act
        var projector = new Projector(input, _projectionArea, _orientation, _position, _dimensions);

        // Assert
        projector.ProjectedContent.Should().Be(input, because: "the constructor should assign the provided projectiona area");
    }

    /// <summary>
    /// Verifies that the constructor does allow a null Orientation and assigns a null value.
    /// </summary>
    [Fact]
    public void Constructor_WithNullOrientation_ShouldBeNull()
    {
        // Arrange
        var projector = new Projector(projectedContent: "sample",
            projectionArea: _projectionArea,
            orientation: null!,
            position: _position,
            dimensions: _dimensions);

        // Act & Assert
        projector.Orientation.Should().BeNull(because: "constructor doesn't validate null orientation.");
    }

    /// <summary>
    /// Verifies that the constructor does allow a null Dimension and assigns a null value.
    /// </summary>
    [Fact]
    public void Constructor_WithNullDimension_ShouldBeNull()
    {
        // Arrange
        var projector = new Projector(projectedContent: "sample",
            projectionArea: _projectionArea,
            orientation: _orientation,
            position: _position,
            dimensions: null!);

        // Act & Assert
        projector.Dimensions.Should().BeNull(because: "constructor doesn't validate null dimension.");
    }

    /// <summary>
    /// Verifies that the constructor does allow a null Position and assigns a null value.
    /// </summary>
    [Fact]
    public void Constructor_WithNullPosition_ShouldBeNull()
    {
        // Arrange
        var projector = new Projector(projectedContent: "sample",
            projectionArea: _projectionArea,
            orientation: _orientation,
            position: null!,
            dimensions: _dimensions);

        // Act & Assert
        projector.Position.Should().BeNull(because: "constructor doesn't validate null position.");
    }

    /// <summary>
    /// Verifies that the constructor does allow a null ProjectionArea and assigns a null value.
    /// </summary>
    [Fact]
    public void Constructor_WithNullProjectionArea_ShouldBeNull()
    {
        // Arrange
        var projector = new Projector(projectedContent: "sample",
            projectionArea: null!,
            orientation: _orientation,
            position: _position,
            dimensions: _dimensions);

        // Act & Assert
        projector.ProjectionArea.Should().BeNull(because: "constructor doesn't validate null projection area.");
    }

    /// <summary>
    /// Verifies that the constructor does allow an empty ProjectedContent and assigns a null value.
    /// </summary>
    [Fact]
    public void Constructor_WithEmptyProjectedContent_ShouldBeEmpty()
    {
        // Arrange
        var projector = new Projector("",
            projectionArea: _projectionArea,
            orientation: _orientation,
            position: _position,
            dimensions: _dimensions);

        // Act & Assert
        projector.ProjectedContent.Should().BeEmpty(because: "constructor should allow empty projected content.");
    }

}
