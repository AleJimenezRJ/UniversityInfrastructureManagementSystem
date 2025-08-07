using FluentAssertions;
using System.Reflection.Metadata;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Dtos.Spaces;

public class LearningSpaceDtoTests
{
    /// <summary>
    /// Represents the name of the laboratory.
    /// </summary>
    private string _name = "Physics Lab";

    /// <summary>
    /// Represents the type of the entity, defaulting to "Lab".
    /// </summary>
    private string _type = "Lab";

    /// <summary>
    /// Represents the maximum capacity allowed for the collection.
    /// </summary>
    private int _maxCapacity = 30;

    /// <summary>
    /// Represents the height of the object.
    /// </summary>
    private double _height = 3.5;

    /// <summary>
    /// Represents the width of the object.
    /// </summary>
    private double _width = 8.0;

    /// <summary>
    /// Represents the length of the object.
    /// </summary>
    private double _length = 10.0;

    /// <summary>
    /// Represents the default color of the floor.
    /// </summary>
    private string _colorFloor = "Gray";

    /// <summary>
    /// Represents the color of the walls.
    /// </summary>
    private string _colorWalls = "White";

    /// <summary>
    /// Represents the color of the ceiling.
    /// </summary>
    private string _colorCeiling = "White";

    /// <summary>
    /// Represents the data transfer object for a learning space.
    /// </summary>
    private LearningSpaceDto _dto;

    /// <summary>
    /// Represents a data transfer object for a learning space. Copy from <see cref="_dto"/> to test equality.
    /// </summary>
    private LearningSpaceDto _dto2;

    /// <summary>
    /// Represents a data transfer object for a learning space. Different values from <see cref="_dto"/> to test inequality.
    /// </summary>
    private LearningSpaceDto _dto3;


    /// <summary>
    /// Initializes test data for <see cref="LearningSpaceDto"/> tests.
    /// </summary>
    public LearningSpaceDtoTests()
    {
        _dto = new LearningSpaceDto(
            _name,
            _type,
            _maxCapacity,
            _height,
            _width,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling
        );

        _dto2 = new LearningSpaceDto(
            _name,
            _type,
            _maxCapacity,
            _height,
            _width,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling
        );

        _dto3 = new LearningSpaceDto(
            Name: "Chemistry Lab",
            Type: "Lab",
            MaxCapacity: 25,
            Height: 3.0,
            Width: 7.0,
            Length: 9.0,
            ColorFloor: "White",
            ColorWalls: "Gray",
            ColorCeiling: "Gray"
        );
    }

    /// <summary>
    /// Verifies that the Name property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsNameCorrectly()
    {
        var dto = new LearningSpaceDto(
            _name,
            _type,
            _maxCapacity,
            _height,
            _width,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling
        );

        dto.Name.Should().Be(_name, because: "constructor should assign Name");
    }

    /// <summary>
    /// Verifies that the Type property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsTypeCorrectly()
    {
        var dto = new LearningSpaceDto(
            _name,
            _type,
            _maxCapacity,
            _height,
            _width,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling
        );

        dto.Type.Should().Be(_type, because: "constructor should assign Type");
    }

    /// <summary>
    /// Verifies that the MaxCapacity property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsMaxCapacityCorrectly()
    {
        var dto = new LearningSpaceDto(
            _name,
            _type,
            _maxCapacity,
            _height,
            _width,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling
        );

        dto.MaxCapacity.Should().Be(_maxCapacity, because: "constructor should assign MaxCapacity");
    }

    /// <summary>
    /// Verifies that the Height property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsHeightCorrectly()
    {
        var dto = new LearningSpaceDto(
            _name,
            _type,
            _maxCapacity,
            _height,
            _width,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling
        );

        dto.Height.Should().Be(_height, because: "constructor should assign Height");
    }

    /// <summary>
    /// Verifies that the Width property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsWidthCorrectly()
    {
        var dto = new LearningSpaceDto(
            _name,
            _type,
            _maxCapacity,
            _height,
            _width,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling
        );

        dto.Width.Should().Be(_width, because: "constructor should assign Width");
    }

    /// <summary>
    /// Verifies that the Length property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsLengthCorrectly()
    {
        var dto = new LearningSpaceDto(
            _name,
            _type,
            _maxCapacity,
            _height,
            _width,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling
        );

        dto.Length.Should().Be(_length, because: "constructor should assign Length");
    }

    /// <summary>
    /// Verifies that the ColorFloor property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsColorFloorCorrectly()
    {
        var dto = new LearningSpaceDto(
            _name,
            _type,
            _maxCapacity,
            _height,
            _width,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling
        );

        dto.ColorFloor.Should().Be(_colorFloor, because: "constructor should assign ColorFloor");
    }

    /// <summary>
    /// Verifies that the ColorWalls property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsColorWallsCorrectly()
    {
        var dto = new LearningSpaceDto(
            _name,
            _type,
            _maxCapacity,
            _height,
            _width,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling
        );

        dto.ColorWalls.Should().Be(_colorWalls, because: "constructor should assign ColorWalls");
    }

    /// <summary>
    /// Verifies that the ColorCeiling property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsColorCeilingCorrectly()
    {
        var dto = new LearningSpaceDto(
            _name,
            _type,
            _maxCapacity,
            _height,
            _width,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling
        );

        dto.ColorCeiling.Should().Be(_colorCeiling, because: "constructor should assign ColorCeiling");
    }

    /// <summary>
    /// Verifies that two <see cref="LearningSpaceDto"/> instances with the same property values are considered equal.
    /// </summary>
    [Fact]
    public void Equality_WithSameValues_ReturnsTrue()
    {
        _dto.Should().Be(_dto2, because: "records with same values should be equal");
        (_dto == _dto2).Should().BeTrue(because: "record equality operator should return true for same values");
    }

    /// <summary>
    /// Verifies that two <see cref="LearningSpaceDto"/> instances with different property values are not considered equal.
    /// </summary>
    [Fact]
    public void Equality_WithDifferentValues_ReturnsFalse()
    {
        _dto.Should().NotBe(_dto3, because: "records with different values should not be equal");
        (_dto != _dto3).Should().BeTrue(because: "record inequality operator should return true for different values");
    }
}
