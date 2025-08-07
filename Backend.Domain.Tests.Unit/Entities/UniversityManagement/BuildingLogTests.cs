using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.UniversityManagement;

/// <summary>
/// Provides unit tests for the <see cref="BuildingLog"/> entity.
/// </summary>
/// <remarks>
/// This test suite ensures the correct behavior of the <see cref="BuildingLog"/> properties and validates
/// boundary and special input cases. It is intended to be expanded as domain logic evolves.
/// </remarks>
public class BuildingLogTests
{
    private readonly BuildingLog _buildingLog;

    /// <summary>
    /// Initializes a sample instance of <see cref="BuildingLog"/> with valid values for use in property tests.
    /// </summary>
    public BuildingLogTests()
    {
        _buildingLog = new BuildingLog
        {
            BuildingsLogInternalId = 101,
            Name = "Biblioteca Central",
            X = 10.5m,
            Y = -84.1m,
            Z = 5.0m,
            Height = 20.0m,
            Width = 15.0m,
            Length = 30.0m,
            Color = "Red",
            AreaName = "Finca 1",
            ModifiedAt = new DateTime(2025, 7, 8, 12, 0, 0),
            Action = "Update"
        };
    }

    /// <summary>
    /// Verifies that the <see cref="BuildingLog.BuildingsLogInternalId"/> property is set correctly.
    /// </summary>
    [Fact]
    public void BuildingLog_ShouldSetInternalIdCorrectly()
    {
        _buildingLog.BuildingsLogInternalId.Should().Be(101);
    }

    /// <summary>
    /// Verifies that the <see cref="BuildingLog.Name"/> property is set correctly.
    /// </summary>
    [Fact]
    public void BuildingLog_ShouldSetNameCorrectly()
    {
        _buildingLog.Name.Should().Be("Biblioteca Central");
    }

    /// <summary>
    /// Verifies that the X, Y, and Z <see cref="BuildingLog"/> coordinate properties are set correctly.
    /// </summary>
    [Fact]
    public void BuildingLog_ShouldSetCoordinatesCorrectly()
    {
        _buildingLog.X.Should().Be(10.5m);
        _buildingLog.Y.Should().Be(-84.1m);
        _buildingLog.Z.Should().Be(5.0m);
    }

    /// <summary>
    /// Verifies that the <see cref="BuildingLog.Height"/>, <see cref="BuildingLog.Width"/>, and <see cref="BuildingLog.Length"/> are set correctly.
    /// </summary>
    [Fact]
    public void BuildingLog_ShouldSetDimensionsCorrectly()
    {
        _buildingLog.Height.Should().Be(20.0m);
        _buildingLog.Width.Should().Be(15.0m);
        _buildingLog.Length.Should().Be(30.0m);
    }

    /// <summary>
    /// Verifies that the <see cref="BuildingLog.Color"/> property is set correctly.
    /// </summary>
    [Fact]
    public void BuildingLog_ShouldSetColorCorrectly()
    {
        _buildingLog.Color.Should().Be("Red");
    }

    /// <summary>
    /// Verifies that the <see cref="BuildingLog.AreaName"/> property is set correctly.
    /// </summary>
    [Fact]
    public void BuildingLog_ShouldSetAreaNameCorrectly()
    {
        _buildingLog.AreaName.Should().Be("Finca 1");
    }

    /// <summary>
    /// Verifies that the <see cref="BuildingLog.ModifiedAt"/> property is set correctly.
    /// </summary>
    [Fact]
    public void BuildingLog_ShouldSetModifiedAtCorrectly()
    {
        _buildingLog.ModifiedAt.Should().Be(new DateTime(2025, 7, 8, 12, 0, 0));
    }

    /// <summary>
    /// Verifies that the <see cref="BuildingLog.Action"/> property is set correctly.
    /// </summary>
    [Fact]
    public void BuildingLog_ShouldSetActionCorrectly()
    {
        _buildingLog.Action.Should().Be("Update");
    }

    /// <summary>
    /// Ensures the <see cref="BuildingLog"/> allows negative values for coordinates.
    /// </summary>
    [Fact]
    public void BuildingLog_ShouldAllowNegativeCoordinates()
    {
        var log = new BuildingLog { X = -999.99m, Y = -999.99m, Z = -999.99m };
        log.X.Should().BeNegative();
        log.Y.Should().BeNegative();
        log.Z.Should().BeNegative();
    }

    /// <summary>
    /// Ensures the <see cref="BuildingLog"/> allows unusually large dimensions.
    /// </summary>
    [Fact]
    public void BuildingLog_ShouldAllowLargeDimensions()
    {
        var log = new BuildingLog { Height = 10000m, Width = 5000m, Length = 7500m };
        log.Height.Should().BeGreaterThan(1000);
        log.Width.Should().BeGreaterThan(1000);
        log.Length.Should().BeGreaterThan(1000);
    }

    /// <summary>
    /// Confirms that an empty <see cref="BuildingLog.Name"/> is stored as is.
    /// </summary>
    [Fact]
    public void BuildingLog_WhenNameIsEmpty_ShouldStoreItAsIs()
    {
        var log = new BuildingLog { Name = "" };
        log.Name.Should().BeEmpty();
    }

    /// <summary>
    /// Confirms that accessing a null <see cref="BuildingLog.Color"/> throws a <see cref="NullReferenceException"/>.
    /// </summary>
    [Fact]
    public void BuildingLog_WhenColorIsNull_ShouldThrowOnAccess()
    {
        var log = new BuildingLog { Color = null! };
        Action act = () => _ = log.Color.Length;

        act.Should().Throw<NullReferenceException>();
    }

    /// <summary>
    /// Confirms that an empty <see cref="BuildingLog.Action"/> is stored as is.
    /// </summary>
    [Fact]
    public void BuildingLog_WhenActionIsEmpty_ShouldStoreItAsIs()
    {
        var log = new BuildingLog { Action = "" };
        log.Action.Should().BeEmpty();
    }
}
