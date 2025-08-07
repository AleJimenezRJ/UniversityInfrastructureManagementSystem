using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations;

/// <summary>
/// Provides unit tests for the <see cref="BuildingLogServices"/> class,
/// ensuring it interacts correctly with a mocked <see cref="IBuildingLogRepository"/>.
/// </summary>
public class BuildingLogServicesTests
{
    private readonly Mock<IBuildingLogRepository> _buildingLogRepositoryMock;
    private readonly BuildingLogServices _serviceUnderTest;
    private readonly BuildingLog _buildingLog;

    /// <summary>
    /// Initializes the test environment, creating a mocked repository and sample <see cref="BuildingLog"/>.
    /// </summary>
    public BuildingLogServicesTests()
    {
        _buildingLogRepositoryMock = new Mock<IBuildingLogRepository>(MockBehavior.Strict);
        _serviceUnderTest = new BuildingLogServices(_buildingLogRepositoryMock.Object);

        _buildingLog = new BuildingLog
        {
            BuildingsLogInternalId = 101,
            Name = "Edificio Test",
            X = 10.0m,
            Y = -84.0m,
            Z = 5.0m,
            Height = 15.0m,
            Width = 12.0m,
            Length = 25.0m,
            Color = "Gray",
            AreaName = "Área Test",
            ModifiedAt = new DateTime(2025, 7, 8, 14, 0, 0),
            Action = "Create"
        };
    }

    /// <summary>
    /// Tests that <see cref="BuildingLogServices.ListBuildingLogsAsync"/> returns a list of building logs
    /// when the repository returns results.
    /// </summary>
    /// <remarks>
    /// This test verifies that the service delegates to the repository and returns the same list.
    /// </remarks>
    [Fact]
    public async Task ListBuildingLogsAsync_ReturnsListOfBuildingLogs()
    {
        // Arrange
        var buildingLogs = new List<BuildingLog> { _buildingLog };
        _buildingLogRepositoryMock
            .Setup(repo => repo.ListBuildingLogsAsync())
            .ReturnsAsync(buildingLogs);

        // Act
        var result = await _serviceUnderTest.ListBuildingLogsAsync();

        // Assert
        result.Should().ContainSingle()
              .Which.Should().BeEquivalentTo(_buildingLog);
        _buildingLogRepositoryMock.Verify(repo => repo.ListBuildingLogsAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="BuildingLogServices.ListBuildingLogsAsync"/> returns an empty list
    /// when the repository has no building logs.
    /// </summary>
    /// <remarks>
    /// This ensures the service handles empty results gracefully.
    /// </remarks>
    [Fact]
    public async Task ListBuildingLogsAsync_ReturnsEmptyList_WhenNoLogsExist()
    {
        // Arrange
        _buildingLogRepositoryMock
            .Setup(repo => repo.ListBuildingLogsAsync())
            .ReturnsAsync(new List<BuildingLog>());

        // Act
        var result = await _serviceUnderTest.ListBuildingLogsAsync();

        // Assert
        result.Should().BeEmpty();
        _buildingLogRepositoryMock.Verify(repo => repo.ListBuildingLogsAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="BuildingLogServices.ListBuildingLogsAsync"/> throws an exception
    /// when the repository layer fails.
    /// </summary>
    [Fact]
    public async Task ListBuildingLogsAsync_ThrowsException_WhenRepositoryFails()
    {
        // Arrange
        _buildingLogRepositoryMock
            .Setup(repo => repo.ListBuildingLogsAsync())
            .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        Func<Task> act = async () => await _serviceUnderTest.ListBuildingLogsAsync();

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error");
        _buildingLogRepositoryMock.Verify(repo => repo.ListBuildingLogsAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="BuildingLogServices.ListBuildingLogsAsync"/> returns multiple building logs
    /// when the repository returns more than one item.
    /// </summary>
    [Fact]
    public async Task ListBuildingLogsAsync_ReturnsMultipleLogs_WhenRepositoryReturnsMultiple()
    {
        // Arrange
        var log2 = new BuildingLog
        {
            BuildingsLogInternalId = 102,
            Name = "Biblioteca Nueva",
            X = 12,
            Y = -85,
            Z = 7,
            Height = 18,
            Width = 14,
            Length = 20,
            Color = "White",
            AreaName = "Área 2",
            ModifiedAt = new DateTime(2025, 7, 8, 15, 0, 0),
            Action = "Update"
        };

        var logs = new List<BuildingLog> { _buildingLog, log2 };

        _buildingLogRepositoryMock
            .Setup(repo => repo.ListBuildingLogsAsync())
            .ReturnsAsync(logs);

        // Act
        var result = await _serviceUnderTest.ListBuildingLogsAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0].Should().BeEquivalentTo(_buildingLog);
        result[1].Should().BeEquivalentTo(log2);
        _buildingLogRepositoryMock.Verify(repo => repo.ListBuildingLogsAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that a log with empty strings for name and area is still returned correctly by the service.
    /// </summary>
    [Fact]
    public async Task ListBuildingLogsAsync_AllowsEmptyStringsInNameAndArea()
    {
        // Arrange
        var logWithEmptyFields = new BuildingLog
        {
            BuildingsLogInternalId = 103,
            Name = "",
            X = 0,
            Y = 0,
            Z = 0,
            Height = 0,
            Width = 0,
            Length = 0,
            Color = "",
            AreaName = "",
            ModifiedAt = DateTime.UtcNow,
            Action = "Delete"
        };

        _buildingLogRepositoryMock
            .Setup(repo => repo.ListBuildingLogsAsync())
            .ReturnsAsync(new List<BuildingLog> { logWithEmptyFields });

        // Act
        var result = await _serviceUnderTest.ListBuildingLogsAsync();

        // Assert
        result.Should().ContainSingle();
        result.First().Name.Should().BeEmpty();
        result.First().AreaName.Should().BeEmpty();
        _buildingLogRepositoryMock.Verify(repo => repo.ListBuildingLogsAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that all logs returned by the service contain a valid ModifiedAt date in the past or present.
    /// </summary>
    [Fact]
    public async Task ListBuildingLogsAsync_LogsShouldHaveValidModifiedAtDates()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var logs = new List<BuildingLog>
    {
        _buildingLog,
        new BuildingLog
        {
            BuildingsLogInternalId = 104,
            Name = "Edificio Histórico",
            X = 5,
            Y = 10,
            Z = 2,
            Height = 10,
            Width = 5,
            Length = 8,
            Color = "Red",
            AreaName = "Centro",
            ModifiedAt = now,
            Action = "Delete"
        }
    };

        _buildingLogRepositoryMock
            .Setup(repo => repo.ListBuildingLogsAsync())
            .ReturnsAsync(logs);

        // Act
        var result = await _serviceUnderTest.ListBuildingLogsAsync();

        // Assert
        result.Should().OnlyContain(log => log.ModifiedAt <= DateTime.UtcNow);
        _buildingLogRepositoryMock.Verify(repo => repo.ListBuildingLogsAsync(), Times.Once);
    }

    /// <summary>
    /// Verifies that all logs returned by <see cref="BuildingLogServices.ListBuildingLogsAsync"/>
    /// contain non-null and non-empty action values.
    /// </summary>
    [Fact]
    public async Task ListBuildingLogsAsync_AllLogsShouldHaveNonEmptyAction()
    {
        // Arrange
        var logs = new List<BuildingLog>
    {
        _buildingLog,
        new BuildingLog
        {
            BuildingsLogInternalId = 105,
            Name = "Anexo Este",
            X = 3,
            Y = -83,
            Z = 2,
            Height = 12,
            Width = 10,
            Length = 18,
            Color = "Green",
            AreaName = "Zona B",
            ModifiedAt = DateTime.UtcNow,
            Action = "Update"
        }
    };

        _buildingLogRepositoryMock
            .Setup(repo => repo.ListBuildingLogsAsync())
            .ReturnsAsync(logs);

        // Act
        var result = await _serviceUnderTest.ListBuildingLogsAsync();

        // Assert
        result.Should().OnlyContain(log => !string.IsNullOrWhiteSpace(log.Action));
        _buildingLogRepositoryMock.Verify(repo => repo.ListBuildingLogsAsync(), Times.Once);
    }

    /// <summary>
    /// Ensures that at least one log returned by <see cref="BuildingLogServices.ListBuildingLogsAsync"/>
    /// has the action type 'Delete'.
    /// </summary>
    [Fact]
    public async Task ListBuildingLogsAsync_AtLeastOneLogShouldHaveActionDelete()
    {
        // Arrange
        var logs = new List<BuildingLog>
    {
        _buildingLog,
        new BuildingLog
        {
            BuildingsLogInternalId = 106,
            Name = "Edificio Sur",
            X = 4,
            Y = -82,
            Z = 1,
            Height = 8,
            Width = 9,
            Length = 16,
            Color = "Black",
            AreaName = "Sur",
            ModifiedAt = DateTime.UtcNow,
            Action = "Delete"
        }
    };

        _buildingLogRepositoryMock
            .Setup(repo => repo.ListBuildingLogsAsync())
            .ReturnsAsync(logs);

        // Act
        var result = await _serviceUnderTest.ListBuildingLogsAsync();

        // Assert
        result.Should().Contain(log => log.Action == "Delete");
        _buildingLogRepositoryMock.Verify(repo => repo.ListBuildingLogsAsync(), Times.Once);
    }

}
