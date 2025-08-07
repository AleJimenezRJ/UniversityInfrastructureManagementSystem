using FluentAssertions;
using System;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.Spaces;

/// <summary>
/// Unit tests for the <see cref="LearningSpaceLog"/> domain entity.
/// </summary>
public class LearningSpaceLogTests
{
    // Constants for test data
    private const int DefaultId = 1;
    private const string DefaultName = "Aula Magna";
    private const string DefaultType = "Auditorium";
    private const int DefaultCapacity = 100;
    private const decimal DefaultWidth = 10.5m;
    private const decimal DefaultHeight = 4.2m;
    private const decimal DefaultLength = 15.0m;
    private const string DefaultColorFloor = "Red";
    private const string DefaultColorWalls = "White";
    private const string DefaultColorCeiling = "Gray";
    private static readonly DateTime DefaultModifiedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private const string DefaultAction = "Created";

    // Constants for change test
    private const int ChangedId = 2;
    private const string ChangedName = "Lab 202";
    private const string ChangedType = "Laboratory";
    private const int ChangedCapacity = 25;
    private const decimal ChangedWidth = 7.5m;
    private const decimal ChangedHeight = 3.5m;
    private const decimal ChangedLength = 9.0m;
    private const string ChangedColorFloor = "Blue";
    private const string ChangedColorWalls = "Gray";
    private const string ChangedColorCeiling = "White";
    private static readonly DateTime ChangedModifiedAt = DefaultModifiedAt.AddDays(1);
    private const string ChangedAction = "Updated";

    private readonly LearningSpaceLog _log;
    private readonly LearningSpaceLog _logWithSameProperties;
    private readonly LearningSpaceLog _changedLog;

    /// <summary>
    /// Initializes shared test data for all test methods, using valid property values.
    /// </summary>
    public LearningSpaceLogTests()
    {
        _log = BuildDefaultLog();
        _logWithSameProperties = BuildDefaultLog();
        _changedLog = new LearningSpaceLog
        {
            LearningSpaceLogInternalId = ChangedId,
            Name = ChangedName,
            Type = ChangedType,
            MaxCapacity = ChangedCapacity,
            Width = ChangedWidth,
            Height = ChangedHeight,
            Length = ChangedLength,
            ColorFloor = ChangedColorFloor,
            ColorWalls = ChangedColorWalls,
            ColorCeiling = ChangedColorCeiling,
            ModifiedAt = ChangedModifiedAt,
            Action = ChangedAction
        };
    }

    /// <summary>
    /// Helper method to build a LearningSpaceLog with default values.
    /// </summary>
    private static LearningSpaceLog BuildDefaultLog()
    {
        return new LearningSpaceLog
        {
            LearningSpaceLogInternalId = DefaultId,
            Name = DefaultName,
            Type = DefaultType,
            MaxCapacity = DefaultCapacity,
            Width = DefaultWidth,
            Height = DefaultHeight,
            Length = DefaultLength,
            ColorFloor = DefaultColorFloor,
            ColorWalls = DefaultColorWalls,
            ColorCeiling = DefaultColorCeiling,
            ModifiedAt = DefaultModifiedAt,
            Action = DefaultAction
        };
    }

    /// <summary>
    /// Checks that the Id property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsIdCorrectly() => _log.LearningSpaceLogInternalId.Should().Be(DefaultId);

    /// <summary>
    /// Checks that the Name property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsNameCorrectly() => _log.Name.Should().Be(DefaultName);

    /// <summary>
    /// Checks that the Type property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsTypeCorrectly() => _log.Type.Should().Be(DefaultType);

    /// <summary>
    /// Checks that the MaxCapacity property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsMaxCapacityCorrectly() => _log.MaxCapacity.Should().Be(DefaultCapacity);

    /// <summary>
    /// Checks that the Width property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsWidthCorrectly() => _log.Width.Should().Be(DefaultWidth);

    /// <summary>
    /// Checks that the Height property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsHeightCorrectly() => _log.Height.Should().Be(DefaultHeight);

    /// <summary>
    /// Checks that the Length property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsLengthCorrectly() => _log.Length.Should().Be(DefaultLength);

    /// <summary>
    /// Checks that the ColorFloor property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsColorFloorCorrectly() => _log.ColorFloor.Should().Be(DefaultColorFloor);

    /// <summary>
    /// Checks that the ColorWalls property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsColorWallsCorrectly() => _log.ColorWalls.Should().Be(DefaultColorWalls);

    /// <summary>
    /// Checks that the ColorCeiling property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsColorCeilingCorrectly() => _log.ColorCeiling.Should().Be(DefaultColorCeiling);

    /// <summary>
    /// Checks that the ModifiedAt property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsModifiedAtCorrectly() => _log.ModifiedAt.Should().Be(DefaultModifiedAt);

    /// <summary>
    /// Checks that the Action property is assigned correctly by the constructor.
    /// </summary>
    [Fact]
    public void Constructor_AssignsActionCorrectly() => _log.Action.Should().Be(DefaultAction);

    /// <summary>
    /// Verifies that two LearningSpaceLog instances with the same property values are considered equivalent.
    /// </summary>
    [Fact]
    public void TwoLearningSpaceLogs_WithSameProperties_ShouldBeEquivalent()
    {
        _log.Should().BeEquivalentTo(_logWithSameProperties, because: "two logs with the same property values should be considered equivalent");
    }

    /// <summary>
    /// Ensures that the properties of LearningSpaceLog can be changed after initialization and are correctly updated.
    /// </summary>
    [Fact]
    public void CanChangeProperties_AfterInitialization()
    {
        // Arrange
        var log = new LearningSpaceLog();

        // Act
        log.LearningSpaceLogInternalId = ChangedId;
        log.Name = ChangedName;
        log.Type = ChangedType;
        log.MaxCapacity = ChangedCapacity;
        log.Width = ChangedWidth;
        log.Height = ChangedHeight;
        log.Length = ChangedLength;
        log.ColorFloor = ChangedColorFloor;
        log.ColorWalls = ChangedColorWalls;
        log.ColorCeiling = ChangedColorCeiling;
        log.ModifiedAt = ChangedModifiedAt;
        log.Action = ChangedAction;

        // Assert
        log.LearningSpaceLogInternalId.Should().Be(ChangedId);
        log.Name.Should().Be(ChangedName);
        log.Type.Should().Be(ChangedType);
        log.MaxCapacity.Should().Be(ChangedCapacity);
        log.Width.Should().Be(ChangedWidth);
        log.Height.Should().Be(ChangedHeight);
        log.Length.Should().Be(ChangedLength);
        log.ColorFloor.Should().Be(ChangedColorFloor);
        log.ColorWalls.Should().Be(ChangedColorWalls);
        log.ColorCeiling.Should().Be(ChangedColorCeiling);
        log.ModifiedAt.Should().Be(ChangedModifiedAt);
        log.Action.Should().Be(ChangedAction);
    }
}