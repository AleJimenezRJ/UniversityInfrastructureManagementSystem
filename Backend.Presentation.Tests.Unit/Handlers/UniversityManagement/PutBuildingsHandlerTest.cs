using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="PutBuildingHandler"/>, which handles HTTP PUT requests
/// </summary>
public class PutBuildingHandlerTests
{
    private readonly Mock<IBuildingsServices> _mockBuildingService = new(MockBehavior.Strict);
    private readonly Mock<IAreaServices> _mockAreaService = new(MockBehavior.Strict);

    /// <summary>
    /// Initializes the mock services used to simulate <see cref="IBuildingsServices"/> and <see cref="IAreaServices"/> behavior.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenUpdateIsSuccessful()
    {
        var dto = TestData.AddBuildingDto();
        var building = TestData.BuildingEntity();

        _mockBuildingService.Setup(s => s.DisplayBuildingAsync(1)).ReturnsAsync(building);
        _mockAreaService.Setup(s => s.GetByNameAsync(dto.Area.Name)).ReturnsAsync(building.Area);
        _mockBuildingService.Setup(s => s.UpdateBuildingAsync(It.IsAny<Building>(), 1)).ReturnsAsync(true);

        var result = await PutBuildingHandler.HandleAsync(_mockBuildingService.Object, _mockAreaService.Object, 1, dto);

        result.Result.Should().BeOfType<Ok<PutBuildingResponse>>();
    }

    /// <summary>
    /// Tests that <see cref="PutBuildingHandler.HandleAsync"/> returns a BadRequest result
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenBuildingNotFound()
    {
        var dto = TestData.AddBuildingDto();
        _mockBuildingService.Setup(s => s.DisplayBuildingAsync(99)).ReturnsAsync((Building?)null);
        _mockAreaService.Setup(s => s.GetByNameAsync(dto.Area.Name)).ReturnsAsync(TestData.BuildingEntity().Area);

        var result = await PutBuildingHandler.HandleAsync(_mockBuildingService.Object, _mockAreaService.Object, 99, dto);

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests that <see cref="PutBuildingHandler.HandleAsync"/> returns a BadRequest result
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenDtoHasInvalidDimensions()
    {
        var invalidDto = new AddBuildingDto(
            "Building A", 1, 2, 3,
            -10, -20, -30, // dimensiones inválidas
            "Red", new("Area X"));

        var building = TestData.BuildingEntity();
        _mockBuildingService.Setup(s => s.DisplayBuildingAsync(1)).ReturnsAsync(building);
        _mockAreaService.Setup(s => s.GetByNameAsync(invalidDto.Area.Name)).ReturnsAsync(building.Area);

        var result = await PutBuildingHandler.HandleAsync(_mockBuildingService.Object, _mockAreaService.Object, 1, invalidDto);

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests that <see cref="PutBuildingHandler.HandleAsync"/> returns a BadRequest result
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenDtoHasInvalidColor()
    {
        var invalidDto = new AddBuildingDto(
            "Building A", 1, 2, 3,
            10, 20, 30,
            "InvisibleColor", 
            new("Area X"));

        var building = TestData.BuildingEntity();
        _mockBuildingService.Setup(s => s.DisplayBuildingAsync(1)).ReturnsAsync(building);
        _mockAreaService.Setup(s => s.GetByNameAsync(invalidDto.Area.Name)).ReturnsAsync(building.Area);

        var result = await PutBuildingHandler.HandleAsync(_mockBuildingService.Object, _mockAreaService.Object, 1, invalidDto);

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests that <see cref="PutBuildingHandler.HandleAsync"/> returns a Conflict result
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenUpdateFails()
    {
        var dto = TestData.AddBuildingDto();
        var building = TestData.BuildingEntity();

        _mockBuildingService.Setup(s => s.DisplayBuildingAsync(1)).ReturnsAsync(building);
        _mockAreaService.Setup(s => s.GetByNameAsync(dto.Area.Name)).ReturnsAsync(building.Area);
        _mockBuildingService.Setup(s => s.UpdateBuildingAsync(It.IsAny<Building>(), 1)).ReturnsAsync(false);

        var result = await PutBuildingHandler.HandleAsync(_mockBuildingService.Object, _mockAreaService.Object, 1, dto);

        result.Result.Should().BeOfType<Conflict<ErrorResponse>>();
    }

    /// <summary>
    /// Tests that <see cref="PutBuildingHandler.HandleAsync"/> returns a Conflict result
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDuplicatedEntityExceptionThrown()
    {
        var dto = TestData.AddBuildingDto();
        var building = TestData.BuildingEntity();

        _mockBuildingService.Setup(s => s.DisplayBuildingAsync(1)).ReturnsAsync(building);
        _mockAreaService.Setup(s => s.GetByNameAsync(dto.Area.Name)).ReturnsAsync(building.Area);
        _mockBuildingService.Setup(s => s.UpdateBuildingAsync(It.IsAny<Building>(), 1)).ThrowsAsync(new DuplicatedEntityException("duplicated"));

        var result = await PutBuildingHandler.HandleAsync(_mockBuildingService.Object, _mockAreaService.Object, 1, dto);

        result.Result.Should().BeOfType<Conflict<ErrorResponse>>();
    }
}
/// <summary>
/// Static class to provide test data for building-related tests.
/// </summary>
internal static class TestData
{
    public static AddBuildingDto AddBuildingDto() => new(
        "Building A", 1, 2, 3, 10, 20, 30, "Red", new("Area X"));

    public static Building BuildingEntity()
    {
        var university = new University(new EntityName("UCR")); // Suponiendo que así se construye
        var location = new EntityLocation("San Pedro"); // Suponiendo que así se construye
        var campus = new Campus(new EntityName("Main Campus"), location, university);
        var area = new Area(new EntityName("Area X"), campus);

        return new Building(
            new EntityName("Building A"),
            new Coordinates(1, 2, 3),
            new Dimension(10, 20, 30),
            new Colors("Red"),
            area
        )
        {
            BuildingInternalId = 1
        };
    }

}

