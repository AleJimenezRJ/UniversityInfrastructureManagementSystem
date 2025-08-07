/*
Story: This task corresponds to the epic EPIC ID: SQL-LS-001 and is associated with PBIs #8, #10, #11, and #12.

Technical Tasks Performed:
- Implementation of unit tests
- Follow Clean Code Principles
- Define Domain Entities
- Commented using XML style
- Handle Input Parameters and Validation

Participants:
- **Rolando Villavicencio**: Implemented constructor tests with ID, validating the correct assignment of `Type`, `Width`, and `ColorWalls`, as well as equivalence tests between objects.
- **Bryan Ávila**: Focused on constructor tests with and without ID, verifying attributes such as `Name`, `MaxCapacity`, `Height`, and equivalence tests between objects.
- **Emmanuel Valenciano**: Focused on constructor tests without ID, validating the correct assignment of `Type`, `Width`, and `ColorWalls`, as well as equivalence tests between objects.
*/


using FluentAssertions; // Fluent assertion library for expressive unit tests
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.Spaces;

/// <summary>
/// Unit tests for the <see cref="LearningSpace"/> domain entity.
/// </summary>
public class LearningSpaceTests
{
    private const int DefaultId = 0;
    private readonly EntityName _name;
    private readonly LearningSpaceType _type;
    private readonly Capacity _capacity;
    private readonly Size _height;
    private readonly Size _width;
    private readonly Size _length;
    private readonly Colors _colorFloor;
    private readonly Colors _colorWalls;
    private readonly Colors _colorCeiling;

    /// <summary>
    /// Initializes shared test data for all test methods, using valid value objects.
    /// </summary>
    public LearningSpaceTests()
    {

        _name = EntityName.Create("Lab 101");
        _type = LearningSpaceType.Create("Laboratory");
        _capacity = Capacity.Create(30);
        _height = Size.Create(3.0);
        _width = Size.Create(5.0);
        _length = Size.Create(6.0);
        _colorFloor = Colors.Create("Gray");
        _colorWalls = Colors.Create("White");
        _colorCeiling = Colors.Create("Red");
    }

    /// <summary>
    /// Ensures that the <see cref="LearningSpace"/> constructor without ID assigns the correct type.
    /// </summary>
    [Fact]
    public void Constructor_WithoutId_AssignsTypeCorrectly()
    {
        // Act
        var space = new LearningSpace(_name, _type, _capacity, _height, _width, _length, _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        space.Type.Should().Be(_type, because: "constructor should correctly assign the LearningSpaceType");
    }

    /// <summary>
    /// Ensures that the <see cref="LearningSpace"/> constructor without ID assigns the correct width.
    /// </summary>
    [Fact]
    public void Constructor_WithoutId_AssignsWidthCorrectly()
    {
        // Act
        var space = new LearningSpace(_name, _type, _capacity, _height, _width, _length, _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        space.Width.Should().Be(_width, because: "constructor should correctly assign the Width of the learning space");
    }

    /// <summary>
    /// Verifies that the constructor without id assigns the Name property correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithoutId_AssignsNameCorrectly()
    {
        // Act
        var learningSpace = new LearningSpace(
            _name, _type, _capacity,
            _height, _width, _length,
            _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        learningSpace.Name.Should().Be(_name, because: "the constructor should assign the Name property correctly when given value objects to construct");
    }

    /// <summary>
    /// Verifies that the constructor without id assigns the Height property correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithoutId_AssignsHeightCorrectly()
    {
        // Act
        var learningSpace = new LearningSpace(
            _name, _type, _capacity,
            _height, _width, _length,
            _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        learningSpace.Height.Should().Be(_height, because: "the constructor should assign the Height property correctly when given value objects to construct");
    }

    /// <summary>
    /// Verifies that the constructor without id assigns the MaxCapacity property correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithoutId_AssignsMaxCapacityCorrectly()
    {
        // Act
        var learningSpace = new LearningSpace(
            _name, _type, _capacity,
            _height, _width, _length,
            _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        learningSpace.MaxCapacity.Should().Be(_capacity, because: "the constructor should assign the MaxCapacity property correctly when given value objects to construct");
    }

    /// <summary>
    /// Ensures that the <see cref="LearningSpace"/> constructor without ID assigns the correct height.
    /// </summary>
    [Fact]
    public void Constructor_WithId_AssignsLearningSpaceTypeCorrectly()
    {
        // Act
        var learningSpace = new LearningSpace(
            DefaultId, _name, _type, _capacity,
            _height, _width, _length,
            _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        learningSpace.Type.Should().Be(_type, because: "the constructor should assign the type property correctly when given value objects to construct");
    }

    /// <summary>
    /// Ensures that the <see cref="LearningSpace"/> constructor without ID assigns the correct width.
    /// </summary>
    [Fact]
    public void Constructor_WithId_AssignsWidthCorrectly()
    {
        // Act
        var learningSpace = new LearningSpace(
            DefaultId, _name, _type, _capacity,
            _height, _width, _length,
            _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        learningSpace.Width.Should().Be(_width, because: "the constructor should assign the Width property correctly when given value objects to construct");
    }
    /// <summary>
    ///     Ensures that the <see cref="LearningSpace"/> constructor without ID assigns the correct height.
    /// </summary>
    [Fact]
    public void Constructor_WithId_AssignsColorWallsCorrectly()
    {
        // Act
        var learningSpace = new LearningSpace(
            DefaultId, _name, _type, _capacity,
            _height, _width, _length,
            _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        learningSpace.ColorWalls.Should().Be(_colorWalls, because: "the constructor should assign the ColorWalls property correctly when given value objects to construct");
    }

    /// <summary>
    /// Ensures that the <see cref="LearningSpace"/> constructor without ID assigns the correct wall color.
    /// </summary>
    [Fact]
    public void Constructor_WithoutId_AssignsColorWallsCorrectly()
    {
        // Act
        var space = new LearningSpace(_name, _type, _capacity, _height, _width, _length, _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        space.ColorWalls.Should().Be(_colorWalls, because: "constructor should correctly assign the wall color");
    }

    /// <summary>
    /// Verifies that two <see cref="LearningSpace"/> instances with the same ID and properties are equivalent.
    /// </summary>
    [Fact]
    public void TwoLearningSpaces_WithId_WithSameProperties_ShouldBeEquivalent()
    {
        // Act
        var learningSpace1 = new LearningSpace(
            DefaultId, _name, _type, _capacity,
            _height, _width, _length,
            _colorFloor, _colorWalls, _colorCeiling);

        var learningSpace2 = new LearningSpace(
            DefaultId, _name, _type, _capacity,
            _height, _width, _length,
            _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        learningSpace1.Should().BeEquivalentTo(learningSpace2, because: "two learning spaces with the same properties should be equal");
    }

    /// <summary>
    /// Verifies that the constructor with id assigns the Name property correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithId_AssignsNameCorrectly()
    {
        // Act
        var learningSpace = new LearningSpace(
            DefaultId, _name, _type, _capacity,
            _height, _width, _length,
            _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        learningSpace.Name.Should().Be(_name, because: "the constructor should assign the Name property correctly when given value objects to construct");
    }

    /// <summary>
    /// Verifies that the constructor with id assigns the Height property correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithId_AssignsHeightCorrectly()
    {
        // Act
        var learningSpace = new LearningSpace(
            DefaultId, _name, _type, _capacity,
            _height, _width, _length,
            _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        learningSpace.Height.Should().Be(_height, because: "the constructor should assign the Height property correctly when given value objects to construct");
    }

    /// <summary>
    /// Verifies that the constructor with id assigns the MaxCapacity property correctly.
    /// </summary>
    [Fact]
    public void Constructor_WithId_AssignsMaxCapacityCorrectly()
    {
        // Act
        var learningSpace = new LearningSpace(
            DefaultId, _name, _type, _capacity,
            _height, _width, _length,
            _colorFloor, _colorWalls, _colorCeiling);

        // Assert
        learningSpace.MaxCapacity.Should().Be(_capacity, because: "the constructor should assign the MaxCapacity property correctly when given value objects to construct");
    }

    /// <summary>
    /// Verifies that two <see cref="LearningSpace"/> instances with identical parameters are equivalent.
    /// </summary>
    [Fact]
    public void TwoLearningSpaces_WithoutId_WithSameProperties_ShouldBeEquivalent()
    {
        // Arrange
        var space1 = new LearningSpace(_name, _type, _capacity, _height, _width, _length, _colorFloor, _colorWalls, _colorCeiling);
        var space2 = new LearningSpace(_name, _type, _capacity, _height, _width, _length, _colorFloor, _colorWalls, _colorCeiling);
        

        // Assert
        space1.Should().BeEquivalentTo(space2,
            because: "two learning spaces with the same property values should be considered equivalent");
    }
}
