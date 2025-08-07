using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="GetBuildingsPaginatedListHandler"/>, which handles HTTP GET requests
/// for retrieving all buildings in the system.
/// </summary>
public class GetBuildingsPaginatedListHandlerTests
{
    private readonly Mock<IBuildingsServices> _buildingServiceMock;

    /// <summary>
    /// Initializes the mock service used to simulate <see cref="IBuildingsServices"/> behavior.
    /// </summary>
    public GetBuildingsPaginatedListHandlerTests()
    {
        _buildingServiceMock = new Mock<IBuildingsServices>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingPaginatedListHandler.HandleAsync"/> returns a successful
    /// <see cref="Ok{T}"/> result containing a list of building DTOs with the pagination information when the service call succeeds.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenCalledWithDefaultParams_ShouldReturnOkWithCorrectData()
    {
        // Arrange
        var buildings = new List<Building>
        {
            TestBuilding()
        };

        var paginatedList = new PaginatedList<Building>(buildings, buildings.Count, 10, 0);

        _buildingServiceMock.Setup(s => s.GetBuildingsListPaginatedAsync(10, 0, null))
            .ReturnsAsync(paginatedList);

        // Act
        var result = await GetBuildingPaginatedListHandler.HandleAsync(_buildingServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetBuildingPaginatedListResponse>>(result.Result);
        var buildingList = okResult.Value!.Buildings.ToList();
        buildingList.Should().HaveCount(1);
        buildingList[0].Name.Should().Be("Engineering");
        okResult.Value.PageSize.Should().Be(10);
        okResult.Value.PageIndex.Should().Be(0);
    }    
    
    /// <summary>
    /// Tests that <see cref="GetBuildingPaginatedListHandler.HandleAsync"/> returns a successful
    /// <see cref="Ok{T}"/> result containing a list of building DTOs with the pagination information 
    /// when the service call succeeds. 
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenTotalBuildingsIsLargerThanOne_ShouldReturnOkWithCorrectData()
    {
        // Arrange
        var buildings = new List<Building>
        {
            TestBuilding(),
            TestBuilding(),
            TestBuilding(),
            TestBuilding(),
            TestBuilding(),
            TestBuilding(),
            TestBuilding(),
            TestBuilding(),
            TestBuilding(),
            TestBuilding()

        };

        var paginatedList = new PaginatedList<Building>(buildings, 10, 10, 0);

        _buildingServiceMock.Setup(s => s.GetBuildingsListPaginatedAsync(10, 0, null))
            .ReturnsAsync(paginatedList);

        // Act
        var result = await GetBuildingPaginatedListHandler.HandleAsync(_buildingServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetBuildingPaginatedListResponse>>(result.Result);
        var buildingList = okResult.Value!.Buildings.ToList();
        buildingList.Should().HaveCount(10);
        buildingList[0].Name.Should().Be("Engineering");
        okResult.Value.PageSize.Should().Be(10);
        okResult.Value.PageIndex.Should().Be(0);
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingPaginatedListHandler.HandleAsync"/> returns a <see cref="BadRequest"/> of  type <see cref="ValidationError"/>
    /// when the the request has an invalid number of pages.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenPageSizeIsTooSmall_ShouldReturnValidationError()
    {
        var result = await GetBuildingPaginatedListHandler.HandleAsync(_buildingServiceMock.Object, pageSize: 0, pageIndex: 0);

        var badRequestResult = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequestResult.Value.Should().ContainSingle()
            .Which.Message.Should().Be("Page size must be greater than or equal to 1");
    }
    /// <summary>
    /// Tests that <see cref="GetBuildingPaginatedListHandler.HandleAsync"/> returns a <see cref="NotFound"/>
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenServiceThrowsNotFound_ShouldReturnNotFound()
    {
        _buildingServiceMock.Setup(s => s.GetBuildingsListPaginatedAsync(10, 0, null))
            .ThrowsAsync(new NotFoundException("No buildings found"));

        var result = await GetBuildingPaginatedListHandler.HandleAsync(_buildingServiceMock.Object, 10, 0);

        var notFoundResult = Assert.IsType<NotFound<string>>(result.Result);
        notFoundResult.Value.Should().Be("No buildings found");
    }
    /// <summary>
    /// Creates a fully initialized <see cref="Building"/> instance to use in test scenarios.
    /// This method simulates a valid entity with nested structures such as Area, Campus, and University.
    /// </summary>
    /// <returns>A test <see cref="Building"/> instance.</returns>
    private static Building TestBuilding()
    {
        var name = new EntityName("Engineering");
        var coord = new Coordinates(1.1, 2.2, 3.3);
        var dim = new Dimension(10, 20, 30);
        var color = new Colors("Blue");

        var university = new University(
            new EntityName("UCR"),
            new EntityLocation("Costa Rica")
        );

        var campus = new Campus(
            new EntityName("RF"),
            new EntityLocation("Locate"),
            university
        );

        var area = new Area(
            new EntityName("F1"),
            campus
        );

        return new Building(1, name, coord, dim, color, area);
    }
}
