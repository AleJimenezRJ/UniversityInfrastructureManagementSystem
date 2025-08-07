using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.ComponentsManagement;

/*
User Stories: The testing for this class corresponds to EPIC ID: CPD-LC-001 and it PBI #21.

Technical tasks performed:

- Implemented various unit tests according to the different constructors for the entity Whiteboard
- Proper XML documentation added
- Handle valid and invalid parameters and validations
- Pair programming with the team to ensure proper implementation of the business logic
- Validation with the Product Owner to verify the prioritized tasks
- Consultation with technical assistants to ensure the correct implementation of the tests

Participants:
    - Ericka Araya Hidalgo. C20553
    - Luis Fonseca Chinchilla. C03035
*/


/// <summary>
/// Contains unit tests for the <see cref="Whiteboard"/> entity.
/// These tests verify correct property assignment and validation logic
/// in the Whiteboard constructors, including handling of null and invalid parameters.
/// </summary>
public class WhiteboardTests
{
    /// <summary>
    /// Default marker color used in tests.
    /// </summary>
    private readonly Colors _markerColor;
    /// <summary>
    /// Default orientation used in tests.
    /// </summary>
    private readonly Orientation _orientation;
    /// <summary>
    /// Default position used in tests.
    /// </summary>
    private readonly Coordinates _position;
    /// <summary>
    /// Default dimension used in tests.
    /// </summary>
    private readonly Dimension _dimension;
    /// <summary>
    /// Valid component ID used in tests.
    /// </summary>
    private readonly int _validId;

    /// <summary>
    /// Initializes test data for each test case.
    /// </summary>
    public WhiteboardTests()
    {
        _markerColor = Colors.Create("blue");
        _orientation = Orientation.Create("North");
        _position = Coordinates.Create(2, 2, 2);
        _dimension = Dimension.Create(100, 50, 8);
        _validId = 1;
    }

    /// <summary>
    /// Verifies that the constructor correctly assigns the MarkerColor property when all parameters are valid.
    /// </summary>
    [Fact]
    public void Constructor_WithAllValidParameters_AssignsMarkerColorCorrectly()
    {
        // Arrange
        var whiteboard = new Whiteboard(_markerColor, _orientation, _position, _dimension);
        // Act & Assert
        whiteboard.MarkerColor.Should().Be(_markerColor,
            because: "constructor should correctly assign the MarkerColor value");
    }

    /// <summary>
    /// Verifies that the constructor correctly assigns the Position property when all parameters are valid.
    /// </summary>
    [Fact]
    public void Constructor_WithAllValidParameters_AssignsPositionsCorrectly()
    {
        // Arrange
        var whiteboard = new Whiteboard(_markerColor, _orientation, _position, _dimension);
        // Act & Assert
        whiteboard.Position.Should().Be(_position,
            because: "constructor should correctly assign the Positions value");
    }

    /// <summary>
    /// Verifies that the constructor correctly assigns the Orientation property when all parameters are valid.
    /// </summary>
    [Fact]
    public void Constructor_WithAllValidParameters_AssignsOrientationCorrectly()
    {
        // Arrange
        var whiteboard = new Whiteboard(_markerColor, _orientation, _position, _dimension);
        // Act & Assert
        whiteboard.Orientation.Should().Be(_orientation,
            because: "constructor should correctly assign the Orientation value");
    }

    /// <summary>
    /// Verifies that the constructor correctly assigns the Dimensions property when all parameters are valid.
    /// </summary>
    [Fact]
    public void Constructor_WithAllValidParameters_AssignsDimensionsCorrectly()
    {
        // Arrange
        var whiteboard = new Whiteboard(_markerColor, _orientation, _position, _dimension);
        // Act & Assert
        whiteboard.Dimensions.Should().Be(_dimension,
            because: "constructor should correctly assign the Dimensions value");
    }

    /// <summary>
    /// Verifies that the constructor does allow a null MarkerColor and assigns a null value.
    /// </summary>
    [Fact]
    public void Constructor_WithNullMarkerColor_ShouldBeNull()
    {
        // Arrange 
        var whiteboard = new Whiteboard(markerColor: null!,
            orientation: _orientation,
            position: _position,
            dimensions: _dimension);
        
        // Act & Assert
        whiteboard.MarkerColor.Should().BeNull(because: "Marker color is defined as nullable.");
    }

    /// <summary>
    /// Verifies that the constructor does allow a null Orientation and assigns a null value.
    /// </summary>
    [Fact]
    public void Constructor_WithNullOrientation_ShouldBeNull()
    {
        // Arrange
        var whiteboard = new Whiteboard(markerColor: _markerColor,
            orientation: null!,
            position: _position,
            dimensions: _dimension);
        
        // Act & Assert
        whiteboard.Orientation.Should().BeNull(because: "constructor doesn't validate null orientation.");
    }

    /// <summary>
    /// Verifies that the constructor does allow a null Position and assigns a null value.
    /// </summary>
    [Fact]
    public void Constructor_WithNullPosition_ShouldBeNull()
    {
        // Arrange
        var whiteboard = new Whiteboard(markerColor: _markerColor,
            orientation: _orientation,
            position: null!,
            dimensions: _dimension);
        
        // Act & Assert
        whiteboard.Position.Should().BeNull(because: "constructor doesn't validate null position.");
    }

    /// <summary>
    /// Verifies that the constructor does allow a null Dimension and assigns a null value.
    /// </summary>
    [Fact]
    public void Constructor_WithNullDimension_ShouldBeNull()
    {
        // Arrange
        var whiteboard = new Whiteboard(markerColor: _markerColor,
            orientation: _orientation,
            position: _position,
            dimensions: null!);
        
        // Act & Assert
        whiteboard.Dimensions.Should().BeNull(because: "constructor doesn't validate null dimension.");
    }

    /// <summary>
    /// Verifies that the constructor with an ID correctly assigns the ComponentId property.
    /// </summary>
    [Fact]
    public void ConstructorWithId_WithAllValidParameters_AssignsIdCorrectly()
    {
        // Arrange
        var whiteboard = new Whiteboard(_markerColor, _validId, _orientation, _position, _dimension);
        // Act & Assert
        whiteboard.ComponentId.Should().Be(1,
            because: "constructor should correctly assign the ComponentId value");
    }
}
