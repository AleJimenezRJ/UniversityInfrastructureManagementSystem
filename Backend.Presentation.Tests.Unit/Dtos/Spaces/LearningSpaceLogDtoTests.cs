using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Dtos.Spaces;

/// <summary>
/// Unit tests for the <see cref="LearningSpaceLogDto"/> record.
/// Verifies that the constructor assigns all properties correctly and that equality operations behave as expected.
/// </summary>
public class LearningSpaceLogDtoTests
{
    /// <summary>
    /// Represents the unique internal identifier for the log entry.
    /// </summary>
    private int _logId = 100;

    /// <summary>
    /// Represents the name of the learning space at the time of the logged change.
    /// </summary>
    private string _name = "Physics Lab";

    /// <summary>
    /// Represents the maximum capacity at the time of the change.
    /// </summary>
    private int? _maxCapacity = 30;

    /// <summary>
    /// Represents the type of the learning space.
    /// </summary>
    private string _type = "Lab";

    /// <summary>
    /// Represents the width of the space in meters.
    /// </summary>
    private decimal? _width = 8.0m;

    /// <summary>
    /// Represents the height of the space in meters.
    /// </summary>
    private decimal? _height = 3.5m;

    /// <summary>
    /// Represents the length of the space in meters.
    /// </summary>
    private decimal? _length = 10.0m;

    /// <summary>
    /// Represents the color of the floor.
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
    /// Represents the timestamp indicating when the modification or action took place.
    /// </summary>
    private DateTime _modifiedAt = new DateTime(2024, 1, 1, 12, 0, 0);

    /// <summary>
    /// Represents the type of action performed.
    /// </summary>
    private string _action = "UPDATED";

    /// <summary>
    /// Represents the data transfer object for a learning space log.
    /// </summary>
    private LearningSpaceLogDto _dto;

    /// <summary>
    /// Represents a data transfer object for a learning space log. Copy from <see cref="_dto"/> to test equality.
    /// </summary>
    private LearningSpaceLogDto _dto2;

    /// <summary>
    /// Represents a data transfer object for a learning space log. Different values from <see cref="_dto"/> to test inequality.
    /// </summary>
    private LearningSpaceLogDto _dto3;

    /// <summary>
    /// Initializes test data for <see cref="LearningSpaceLogDto"/> tests.
    /// </summary>
    public LearningSpaceLogDtoTests()
    {
        _dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        _dto2 = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        _dto3 = new LearningSpaceLogDto(
            101,
            "Chemistry Lab",
            25,
            "Classroom",
            7.0m,
            3.0m,
            9.0m,
            "White",
            "Gray",
            "Gray",
            _modifiedAt.AddDays(1),
            "CREATED"
        );
    }

    /// <summary>
    /// Verifies that the LearningSpaceLogInternalId property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsLearningSpaceLogInternalIdCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.LearningSpaceLogInternalId.Should().Be(_logId, because: "constructor should assign LearningSpaceLogInternalId");
    }

    /// <summary>
    /// Verifies that the Name property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsNameCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.Name.Should().Be(_name, because: "constructor should assign Name");
    }

    /// <summary>
    /// Verifies that the MaxCapacity property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsMaxCapacityCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.MaxCapacity.Should().Be(_maxCapacity, because: "constructor should assign MaxCapacity");
    }

    /// <summary>
    /// Verifies that the Type property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsTypeCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.Type.Should().Be(_type, because: "constructor should assign Type");
    }

    /// <summary>
    /// Verifies that the Width property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsWidthCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.Width.Should().Be(_width, because: "constructor should assign Width");
    }

    /// <summary>
    /// Verifies that the Height property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsHeightCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.Height.Should().Be(_height, because: "constructor should assign Height");
    }

    /// <summary>
    /// Verifies that the Length property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsLengthCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.Length.Should().Be(_length, because: "constructor should assign Length");
    }

    /// <summary>
    /// Verifies that the ColorFloor property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsColorFloorCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.ColorFloor.Should().Be(_colorFloor, because: "constructor should assign ColorFloor");
    }

    /// <summary>
    /// Verifies that the ColorWalls property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsColorWallsCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.ColorWalls.Should().Be(_colorWalls, because: "constructor should assign ColorWalls");
    }

    /// <summary>
    /// Verifies that the ColorCeiling property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsColorCeilingCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.ColorCeiling.Should().Be(_colorCeiling, because: "constructor should assign ColorCeiling");
    }

    /// <summary>
    /// Verifies that the ModifiedAt property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsModifiedAtCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.ModifiedAt.Should().Be(_modifiedAt, because: "constructor should assign ModifiedAt");
    }

    /// <summary>
    /// Verifies that the Action property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignsActionCorrectly()
    {
        var dto = new LearningSpaceLogDto(
            _logId,
            _name,
            _maxCapacity,
            _type,
            _width,
            _height,
            _length,
            _colorFloor,
            _colorWalls,
            _colorCeiling,
            _modifiedAt,
            _action
        );

        dto.Action.Should().Be(_action, because: "constructor should assign Action");
    }

    /// <summary>
    /// Verifies that two <see cref="LearningSpaceLogDto"/> instances with the same property values are considered equal.
    /// </summary>
    [Fact]
    public void Equality_WithSameValues_ReturnsTrue()
    {
        _dto.Should().Be(_dto2, because: "records with same values should be equal");
        (_dto == _dto2).Should().BeTrue(because: "record equality operator should return true for same values");
    }

    /// <summary>
    /// Verifies that two <see cref="LearningSpaceLogDto"/> instances with different property values are not considered equal.
    /// </summary>
    [Fact]
    public void Equality_WithDifferentValues_ReturnsFalse()
    {
        _dto.Should().NotBe(_dto3, because: "records with different values should not be equal");
        (_dto != _dto3).Should().BeTrue(because: "record inequality operator should return true for different values");
    }
}
