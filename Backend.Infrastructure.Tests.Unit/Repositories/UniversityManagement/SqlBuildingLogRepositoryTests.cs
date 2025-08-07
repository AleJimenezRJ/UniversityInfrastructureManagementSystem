using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="SqlBuildingLogRepository"/>, which retrieves building logs from the database.
/// </summary>
public class SqlBuildingLogRepositoryTests
{
    private readonly List<BuildingLog> _buildingLogData;
    private readonly Mock<DbSet<BuildingLog>> _mockBuildingLogs;
    private readonly Mock<ThemeParkDataBaseContext> _mockContext;
    private readonly Mock<ILogger<SqlBuildingLogRepository>> _mockLogger;
    private readonly SqlBuildingLogRepository _repository;

    /// <summary>
    /// Initializes mock data, DbContext, logger and repository instance.
    /// </summary>
    public SqlBuildingLogRepositoryTests()
    {
        _buildingLogData = new List<BuildingLog>();
        _mockBuildingLogs = _buildingLogData.AsQueryable().BuildMockDbSet();

        _mockContext = new Mock<ThemeParkDataBaseContext>();
        _mockContext.Setup(c => c.BuildingLog).Returns(_mockBuildingLogs.Object);

        _mockLogger = new Mock<ILogger<SqlBuildingLogRepository>>();

        _repository = new SqlBuildingLogRepository(_mockContext.Object, _mockLogger.Object);
    }

    /// <summary>
    /// Tests that <see cref="SqlBuildingLogRepository.ListBuildingLogsAsync"/> returns all logs ordered by ModifiedAt descending.
    /// </summary>
    [Fact]
    public async Task ListBuildingLogsAsync_ShouldReturnLogsOrderedByModifiedAtDescending()
    {
        // Arrange
        _buildingLogData.Add(new BuildingLog { BuildingsLogInternalId = 1, Name = "A", ModifiedAt = DateTime.UtcNow.AddDays(-1), Action = "Create" });
        _buildingLogData.Add(new BuildingLog { BuildingsLogInternalId = 2, Name = "B", ModifiedAt = DateTime.UtcNow, Action = "Update" });

        // Act
        var result = await _repository.ListBuildingLogsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("B");
        result.Last().Name.Should().Be("A");
    }

    /// <summary>
    /// Tests that <see cref="SqlBuildingLogRepository.ListBuildingLogsAsync"/> returns an empty list when no logs exist.
    /// </summary>
    [Fact]
    public async Task ListBuildingLogsAsync_ShouldReturnEmptyList_WhenNoLogsExist()
    {
        // Act
        var result = await _repository.ListBuildingLogsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that <see cref="SqlBuildingLogRepository.ListBuildingLogsAsync"/> catches exceptions and logs the error.
    /// </summary>
    [Fact]
    public async Task ListBuildingLogsAsync_ShouldReturnEmptyList_WhenExceptionOccurs()
    {
        // Arrange: force an exception
        _mockContext.Setup(c => c.BuildingLog).Throws(new InvalidOperationException("DB access error"));

        // Act
        var result = await _repository.ListBuildingLogsAsync();

        // Assert
        result.Should().BeEmpty();

        // Manual inspection of invocations
        var logInvocation = _mockLogger.Invocations.FirstOrDefault(i =>
            i.Method.Name == nameof(ILogger.Log) &&
            i.Arguments.Count >= 2 &&
            i.Arguments[2]?.ToString()?.Contains("Error listing building logs") == true
        );

        logInvocation.Should().NotBeNull("LogError should be called with message about listing error");
    }


    /// <summary>  _logger.LogError(ex, "Error listing building logs");
    /// Tests that the logs returned include all fields populated as expected.
    /// </summary>
    [Fact]
    public async Task ListBuildingLogsAsync_ShouldIncludeAllLogFields()
    {
        // Arrange
        var expectedLog = new BuildingLog
        {
            BuildingsLogInternalId = 10,
            Name = "Edificio F",
            X = 12.5m,
            Y = -84.1m,
            Z = 4.0m,
            Height = 15.0m,
            Width = 20.0m,
            Length = 30.0m,
            Color = "Green",
            AreaName = "Zona 1",
            ModifiedAt = new DateTime(2025, 7, 8),
            Action = "Delete"
        };

        _buildingLogData.Add(expectedLog);

        // Act
        var result = await _repository.ListBuildingLogsAsync();

        // Assert
        result.Should().ContainSingle();
        result.First().Should().BeEquivalentTo(expectedLog);
    }

    /// <summary>
    /// Tests that the order of logs is preserved when multiple logs have the same ModifiedAt date.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListBuildingLogsAsync_ShouldPreserveInsertionOrder_WhenDatesAreEqual()
    {
        // Arrange
        var date = DateTime.UtcNow;
        _buildingLogData.Add(new BuildingLog { BuildingsLogInternalId = 1, Name = "Primero", ModifiedAt = date, Action = "Create" });
        _buildingLogData.Add(new BuildingLog { BuildingsLogInternalId = 2, Name = "Segundo", ModifiedAt = date, Action = "Create" });

        // Act
        var result = await _repository.ListBuildingLogsAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0].BuildingsLogInternalId.Should().Be(1); // Se espera estabilidad en orden
        result[1].BuildingsLogInternalId.Should().Be(2);
    }

    /// <summary>
    /// Tests that <see cref="SqlBuildingLogRepository.ListBuildingLogsAsync"/> handles null fields gracefully.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListBuildingLogsAsync_ShouldHandleNullFields_Gracefully()
    {
        // Arrange
        _buildingLogData.Add(new BuildingLog
        {
            BuildingsLogInternalId = 3,
            Name = null!,
            Action = null!,
            ModifiedAt = DateTime.UtcNow
            
        });

        // Act
        var result = await _repository.ListBuildingLogsAsync();

        // Assert
        result.Should().ContainSingle();
        result.First().BuildingsLogInternalId.Should().Be(3);
        result.First().Name.Should().BeNull();
        result.First().Action.Should().BeNull();
    }
    /// <summary>
    /// Tests that <see cref="SqlBuildingLogRepository.ListBuildingLogsAsync"/> returns logs sorted by ModifiedAt date in descending order, even when the data is unordered.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListBuildingLogsAsync_ShouldReturnLogsSortedByDate_WhenUnorderedData()
    {
        // Arrange
        _buildingLogData.Add(new BuildingLog { BuildingsLogInternalId = 100, Name = "Antiguo", ModifiedAt = DateTime.UtcNow.AddDays(-10), Action = "Create" });
        _buildingLogData.Add(new BuildingLog { BuildingsLogInternalId = 101, Name = "Reciente", ModifiedAt = DateTime.UtcNow, Action = "Update" });
        _buildingLogData.Add(new BuildingLog { BuildingsLogInternalId = 102, Name = "Intermedio", ModifiedAt = DateTime.UtcNow.AddDays(-5), Action = "Modify" });

        // Act
        var result = await _repository.ListBuildingLogsAsync();

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Reciente");
        result[1].Name.Should().Be("Intermedio");
        result[2].Name.Should().Be("Antiguo");
    }


}
