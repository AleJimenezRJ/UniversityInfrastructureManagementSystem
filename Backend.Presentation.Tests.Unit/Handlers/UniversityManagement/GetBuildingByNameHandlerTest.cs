using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;
/// <summary>
/// Unit tests for <see cref="GetBuildingByIdHandler"/>, which handles HTTP GET requests
/// </summary>
public class GetBuildingByIdHandlerTests
{
    /// <summary>
    /// Mock service for <see cref="IBuildingsServices"/> to simulate database operations
    /// </summary>
    private readonly Mock<IBuildingsServices> _mockService;

    /// <summary>
    /// Initializes the mock service used to simulate <see cref="IBuildingsServices"/> behavior.
    /// </summary>
    public GetBuildingByIdHandlerTests()
    {
        _mockService = new Mock<IBuildingsServices>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingByIdHandler.HandleAsync"/> returns a successful
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenBuildingExists()
    {
        // Arrange
        var testBuilding = TestBuilding();

        _mockService.Setup(s => s.DisplayBuildingAsync(testBuilding.BuildingInternalId))
            .ReturnsAsync(testBuilding);

        // Act
        var result = await GetBuildingByIdHandler.HandleAsync(_mockService.Object, testBuilding.BuildingInternalId);

        // Assert
        var okResult = Assert.IsType<Ok<GetBuildingByNameResponse>>(result.Result);
        okResult.Value.Building.Name.Should().Be("Engineering");
        okResult.Value.Building.Color.Should().Be("Blue");
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingByIdHandler.HandleAsync"/> returns a NotFound result
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenBuildingDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.DisplayBuildingAsync(999)).ReturnsAsync((Building?)null);

        // Act
        var result = await GetBuildingByIdHandler.HandleAsync(_mockService.Object, 999);

        // Assert
        var notFoundResult = Assert.IsType<NotFound<string>>(result.Result);
        notFoundResult.Value.Should().Be("The requested building was not found.");
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingByIdHandler.HandleAsync"/> returns a NotFound result
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenExceptionThrown()
    {
        // Arrange
        _mockService.Setup(s => s.DisplayBuildingAsync(1))
            .ThrowsAsync(new Exception("Database connection error"));

        // Act
        var result = await GetBuildingByIdHandler.HandleAsync(_mockService.Object, 1);

        // Assert
        var notFoundResult = Assert.IsType<NotFound<string>>(result.Result);
        notFoundResult.Value.Should().Contain("Database connection error");
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingByIdHandler.HandleAsync"/> maps all building properties correctly
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldMapAllBuildingPropertiesCorrectly()
    {
        // Arrange
        var building = TestBuilding();

        _mockService.Setup(s => s.DisplayBuildingAsync(building.BuildingInternalId))
            .ReturnsAsync(building);

        // Act
        var result = await GetBuildingByIdHandler.HandleAsync(_mockService.Object, building.BuildingInternalId);

        // Assert
        var okResult = Assert.IsType<Ok<GetBuildingByNameResponse>>(result.Result);
        var dto = okResult.Value.Building;

        dto.Id.Should().Be(building.BuildingInternalId);
        dto.Name.Should().Be("Engineering");
        dto.X.Should().Be(1);
        dto.Y.Should().Be(2);
        dto.Z.Should().Be(3);
        dto.Width.Should().Be(10);
        dto.Length.Should().Be(20);
        dto.Height.Should().Be(30);
        dto.Color.Should().Be("Blue");
        dto.Area.Name.Should().Be("F1");
        dto.Area.Campus.Name.Should().Be("RF");
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingByIdHandler.HandleAsync"/> calls the service method once with the correct ID
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldCallDisplayBuildingAsyncOnce_WithCorrectId()
    {
        // Arrange
        var building = TestBuilding();

        _mockService.Setup(s => s.DisplayBuildingAsync(building.BuildingInternalId))
            .ReturnsAsync(building);

        // Act
        var result = await GetBuildingByIdHandler.HandleAsync(_mockService.Object, building.BuildingInternalId);

        // Assert
        _mockService.Verify(s => s.DisplayBuildingAsync(building.BuildingInternalId), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingByIdHandler.HandleAsync"/> returns NotFound when ID is negative
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenIdIsNegative()
    {
        // Arrange
        int invalidId = -1;
        _mockService.Setup(s => s.DisplayBuildingAsync(invalidId))
            .ReturnsAsync((Building?)null);

        // Act
        var result = await GetBuildingByIdHandler.HandleAsync(_mockService.Object, invalidId);

        // Assert
        var notFoundResult = Assert.IsType<NotFound<string>>(result.Result);
        notFoundResult.Value.Should().Contain("not found");
    }

    /// <summary>
    /// Creates a test building instance for use in unit tests.
    /// </summary>
    /// <returns></returns>
    private static Building TestBuilding()
    {
        var name = new EntityName("Engineering");
        var coord = new Coordinates(1, 2, 3);
        var dim = new Dimension(10, 20, 30);
        var color = new Colors("Blue");

        var university = new University(new EntityName("UCR"), new EntityLocation("Costa Rica"));
        var campus = new Campus(new EntityName("RF"), new EntityLocation("Locate"), university);
        var area = new Area(new EntityName("F1"), campus);

        return new Building(1, name, coord, dim, color, area);
    }
}
