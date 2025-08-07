using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="GetBuildingLogHandler"/>, which handles HTTP GET requests
/// </summary>
public class GetBuildingLogHandlerTests
{
    /// <summary>
    /// Mock service for <see cref="IBuildingLogServices"/> to simulate database operations
    /// </summary>
    private readonly Mock<IBuildingLogServices> _logServiceMock;

    /// <summary>
    /// Initializes the mock service used to simulate <see cref="IBuildingLogServices"/> behavior.
    /// </summary>
    public GetBuildingLogHandlerTests()
    {
        _logServiceMock = new Mock<IBuildingLogServices>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingLogHandler.HandleAsync"/> returns a successful
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenLogsExist_ShouldReturnMappedList()
    {
        // Arrange
        var logs = new List<BuildingLog> { TestLog(1), TestLog(2) };
        _logServiceMock.Setup(s => s.ListBuildingLogsAsync())
            .ReturnsAsync(logs);

        // Act
        var result = await GetBuildingLogHandler.HandleAsync(_logServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<List<BuildingLogDto>>>(result.Result);
        okResult.Value.Should().HaveCount(2);
        okResult.Value[0].BuildingLogInternalId.Should().Be(1);
        okResult.Value[1].BuildingLogInternalId.Should().Be(2);
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingLogHandler.HandleAsync"/> returns a message
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenNoLogs_ShouldReturnMessage()
    {
        // Arrange
        _logServiceMock.Setup(s => s.ListBuildingLogsAsync())
            .ReturnsAsync(new List<BuildingLog>());

        // Act
        var result = await GetBuildingLogHandler.HandleAsync(_logServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<string>>(result.Result);
        okResult.Value.Should().Be("There are no registered changes to the buildings.");
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingLogHandler.HandleAsync"/> returns a message
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenLogsIsNull_ShouldReturnMessage()
    {
        // Arrange
        _logServiceMock.Setup(s => s.ListBuildingLogsAsync())
            .ReturnsAsync((List<BuildingLog>?)null!);

        // Act
        var result = await GetBuildingLogHandler.HandleAsync(_logServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<string>>(result.Result);
        okResult.Value.Should().Be("There are no registered changes to the buildings.");
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingLogHandler.HandleAsync"/> correctly maps fields from the entity to the DTO.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldMapFieldsCorrectly()
    {
        // Arrange
        var log = TestLog(42);
        _logServiceMock.Setup(s => s.ListBuildingLogsAsync())
            .ReturnsAsync(new List<BuildingLog> { log });

        // Act
        var result = await GetBuildingLogHandler.HandleAsync(_logServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<List<BuildingLogDto>>>(result.Result);
        var dto = okResult.Value.First();

        dto.BuildingLogInternalId.Should().Be(42);
        dto.Name.Should().Be("Building A");
        dto.X.Should().Be(1);
        dto.Y.Should().Be(2);
        dto.Z.Should().Be(3);
        dto.Width.Should().Be(4);
        dto.Length.Should().Be(5);
        dto.Height.Should().Be(6);
        dto.Color.Should().Be("Red");
        dto.AreaName.Should().Be("Zone 1");
        dto.Action.Should().Be("Update");
        dto.ModifiedAt.Should().Be(log.ModifiedAt);
    }

    /// <summary>
    /// Creates a test instance of <see cref="BuildingLog"/> with predefined values.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private static BuildingLog TestLog(int id)
    {
        return new BuildingLog
        {
            BuildingsLogInternalId = id,
            Name = "Building A",
            X = 1,
            Y = 2,
            Z = 3,
            Width = 4,
            Length = 5,
            Height = 6,
            Color = "Red",
            AreaName = "Zone 1",
            Action = "Update",
            ModifiedAt = new DateTime(2024, 1, 1, 12, 0, 0)
        };
    }
}
