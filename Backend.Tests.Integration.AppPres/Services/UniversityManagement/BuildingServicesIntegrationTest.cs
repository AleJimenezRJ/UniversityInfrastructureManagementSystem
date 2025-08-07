using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AppPres.Services.UniversityManagement;

/// <summary>
/// Integration tests for Building services and presentation layer handlers.
/// Tests the interaction between Application services and Presentation handlers.
/// </summary>
public class BuildingServicesIntegrationTest
{
    private readonly Mock<IBuildingsRepository> _buildingRepositoryMock;
    private readonly Mock<IAreaServices> _areaServicesMock;
    private readonly IBuildingsServices _buildingServices;

    public BuildingServicesIntegrationTest()
    {
        _buildingRepositoryMock = new Mock<IBuildingsRepository>();
        _areaServicesMock = new Mock<IAreaServices>();
        
        // Setup service collection for dependency injection
        var services = new ServiceCollection();
        services.AddScoped(_ => _buildingRepositoryMock.Object);
        services.AddScoped<IBuildingsServices, UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations.BuildingsServices>();
        
        var serviceProvider = services.BuildServiceProvider();
        _buildingServices = serviceProvider.GetRequiredService<IBuildingsServices>();
    }

    // ==============================
    // POST Building Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between PostBuildingHandler and BuildingServices for successful building creation.
    /// </summary>
    [Fact]
    public async Task GivenValidBuildingDto_WhenHandlerCallsService_ReturnsSuccessResponse()
    {
        // Arrange
        var buildingDto = new AddBuildingDto(
            "Engineering Building",
            10.0, 20.0, 0.0,  // coordinates
            50.0, 30.0, 15.0,  // dimensions
            "Red",  // color
            new AddBuildingAreaDto("Engineering Area")
        );

        var university = new University(
            new EntityName("Universidad de Costa Rica"),
            new EntityLocation("Costa Rica")
        );
        var campus = new Campus(
            new EntityName("Campus Central"),
            new EntityLocation("San Pedro"),
            university
        );
        var area = new Area(
            new EntityName("Engineering Area"),
            campus
        );

        _areaServicesMock.Setup(s => s.GetByNameAsync("Engineering Area"))
            .ReturnsAsync(area);
        
        _buildingRepositoryMock.Setup(r => r.AddBuildingAsync(It.IsAny<Building>()))
            .ReturnsAsync(true);

        // Act - Test handler integration with service
        var result = await PostBuildingHandler.HandleAsync(_buildingServices, _areaServicesMock.Object, buildingDto);

        // Assert
        Assert.IsType<Ok<PostBuildingResponse>>(result.Result);
        var okResult = (Ok<PostBuildingResponse>)result.Result;
        okResult.Value.Should().NotBeNull();
        okResult.Value!.Building.Should().BeEquivalentTo(buildingDto);
        
        _buildingRepositoryMock.Verify(r => r.AddBuildingAsync(It.Is<Building>(b => 
            b.Name!.Name == "Engineering Building")), Times.Once);
    }

    /// <summary>
    /// Tests the integration when building creation fails in the repository.
    /// </summary>
    [Fact]
    public async Task GivenRepositoryFailure_WhenHandlerCallsService_ReturnsConflictResponse()
    {
        // Arrange
        var buildingDto = new AddBuildingDto(
            "Failed Building",
            10.0, 20.0, 0.0,
            50.0, 30.0, 15.0,
            "Blue",
            new AddBuildingAreaDto("Engineering Area")
        );

        var university = new University(
            new EntityName("Universidad de Costa Rica"),
            new EntityLocation("Costa Rica")
        );
        var campus = new Campus(
            new EntityName("Campus Central"),
            new EntityLocation("San Pedro"),
            university
        );
        var area = new Area(
            new EntityName("Engineering Area"),
            campus
        );

        _areaServicesMock.Setup(s => s.GetByNameAsync("Engineering Area"))
            .ReturnsAsync(area);
        
        _buildingRepositoryMock.Setup(r => r.AddBuildingAsync(It.IsAny<Building>()))
            .ReturnsAsync(false);

        // Act
        var result = await PostBuildingHandler.HandleAsync(_buildingServices, _areaServicesMock.Object, buildingDto);

        // Assert
        Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        var conflictResult = (Conflict<ErrorResponse>)result.Result;
        conflictResult.Value.Should().NotBeNull();
        conflictResult.Value!.ErrorMessages.Should().Contain("Building could not be added.");
    }

    /// <summary>
    /// Tests the integration when a duplicate building exception is thrown.
    /// </summary>
    [Fact]
    public async Task GivenDuplicateBuilding_WhenHandlerCallsService_ReturnsConflictResponse()
    {
        // Arrange
        var buildingDto = new AddBuildingDto(
            "Duplicate Building",
            10.0, 20.0, 0.0,
            50.0, 30.0, 15.0,
            "Green",
            new AddBuildingAreaDto("Engineering Area")
        );

        var university = new University(
            new EntityName("Universidad de Costa Rica"),
            new EntityLocation("Costa Rica")
        );
        var campus = new Campus(
            new EntityName("Campus Central"),
            new EntityLocation("San Pedro"),
            university
        );
        var area = new Area(
            new EntityName("Engineering Area"),
            campus
        );

        _areaServicesMock.Setup(s => s.GetByNameAsync("Engineering Area"))
            .ReturnsAsync(area);
        
        _buildingRepositoryMock.Setup(r => r.AddBuildingAsync(It.IsAny<Building>()))
            .ThrowsAsync(new DuplicatedEntityException("Building with this name already exists"));

        // Act
        var result = await PostBuildingHandler.HandleAsync(_buildingServices, _areaServicesMock.Object, buildingDto);

        // Assert
        Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        var conflictResult = (Conflict<ErrorResponse>)result.Result;
        conflictResult.Value.Should().NotBeNull();
        conflictResult.Value!.ErrorMessages.Should().Contain("Building with this name already exists");
    }

    // ==============================
    // GET Building Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between GetBuildingHandler and BuildingServices for successful building retrieval.
    /// </summary>
    [Fact]
    public async Task GivenExistingBuildings_WhenHandlerCallsService_ReturnsBuildingList()
    {
        // Arrange
        var university = new University(
            new EntityName("Universidad de Costa Rica"),
            new EntityLocation("Costa Rica")
        );
        var campus = new Campus(
            new EntityName("Campus Central"),
            new EntityLocation("San Pedro"),
            university
        );
        var area = new Area(
            new EntityName("Engineering Area"),
            campus
        );

        var buildings = new List<Building>
        {
            new Building(
                new EntityName("Building A"),
                Coordinates.Create(10.0, 20.0, 0.0),
                Dimension.Create(50.0, 30.0, 15.0),
                Colors.Create("Red"),
                area
            ),
            new Building(
                new EntityName("Building B"),
                Coordinates.Create(15.0, 25.0, 0.0),
                Dimension.Create(40.0, 25.0, 12.0),
                Colors.Create("Blue"),
                area
            )
        };

        _buildingRepositoryMock.Setup(r => r.ListBuildingAsync())
            .ReturnsAsync(buildings);

        // Act
        var result = await GetBuildingHandler.HandleAsync(_buildingServices);

        // Assert
        Assert.IsType<Ok<GetBuildingResponse>>(result.Result);
        var okResult = (Ok<GetBuildingResponse>)result.Result;
        okResult.Value.Should().NotBeNull();
        okResult.Value!.Buildings.Should().HaveCount(2);
    }

    /// <summary>
    /// Tests the integration when no buildings exist in the repository.
    /// </summary>
    [Fact]
    public async Task GivenNoBuildings_WhenHandlerCallsService_ReturnsEmptyList()
    {
        // Arrange
        _buildingRepositoryMock.Setup(r => r.ListBuildingAsync())
            .ReturnsAsync(new List<Building>());

        // Act
        var result = await GetBuildingHandler.HandleAsync(_buildingServices);

        // Assert
        Assert.IsType<Ok<GetBuildingResponse>>(result.Result);
        var okResult = (Ok<GetBuildingResponse>)result.Result;
        okResult.Value.Should().NotBeNull();
        okResult.Value!.Buildings.Should().BeEmpty();
    }

    // ==============================
    // GET Building By ID Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between GetBuildingByIdHandler and BuildingServices for successful building retrieval by ID.
    /// </summary>
    [Fact]
    public async Task GivenExistingBuildingId_WhenHandlerCallsService_ReturnsBuilding()
    {
        // Arrange
        var buildingId = 1;
        var university = new University(
            new EntityName("Universidad de Costa Rica"),
            new EntityLocation("Costa Rica")
        );
        var campus = new Campus(
            new EntityName("Campus Central"),
            new EntityLocation("San Pedro"),
            university
        );
        var area = new Area(
            new EntityName("Engineering Area"),
            campus
        );

        var building = new Building(
            buildingId,
            new EntityName("Engineering Building"),
            Coordinates.Create(10.0, 20.0, 0.0),
            Dimension.Create(50.0, 30.0, 15.0),
            Colors.Create("Yellow"),
            area
        );

        _buildingRepositoryMock.Setup(r => r.DisplayBuildingAsync(buildingId))
            .ReturnsAsync(building);

        // Act
        var result = await GetBuildingByIdHandler.HandleAsync(_buildingServices, buildingId);

        // Assert
        Assert.IsType<Ok<GetBuildingByNameResponse>>(result.Result);
        var okResult = (Ok<GetBuildingByNameResponse>)result.Result;
        okResult.Value.Should().NotBeNull();
        okResult.Value!.Building.Name.Should().Be("Engineering Building");
    }

    /// <summary>
    /// Tests the integration when building is not found by ID.
    /// </summary>
    [Fact]
    public async Task GivenNonExistingBuildingId_WhenHandlerCallsService_ReturnsNotFound()
    {
        // Arrange
        var buildingId = 999;
        
        _buildingRepositoryMock.Setup(r => r.DisplayBuildingAsync(buildingId))
            .ReturnsAsync((Building?)null);

        // Act
        var result = await GetBuildingByIdHandler.HandleAsync(_buildingServices, buildingId);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
        var notFoundResult = (NotFound<string>)result.Result;
        notFoundResult.Value.Should().Contain("not found");
    }

    // ==============================
    // PUT Building Integration Tests
    // ==============================
    
    /// <summary>
    /// Tests the integration between PutBuildingHandler and BuildingServices for successful building update.
    /// </summary>
    [Fact]
    public async Task GivenValidUpdateDto_WhenHandlerCallsService_ReturnsSuccessResponse()
    {
        // Arrange
        var buildingId = 1;
        var buildingDto = new AddBuildingDto(
            "Updated Building",
            10.0, 20.0, 0.0,
            50.0, 30.0, 15.0,
            "Purple",
            new AddBuildingAreaDto("Engineering Area")
        );

        var university = new University(
            new EntityName("Universidad de Costa Rica"),
            new EntityLocation("Costa Rica")
        );
        var campus = new Campus(
            new EntityName("Campus Central"),
            new EntityLocation("San Pedro"),
            university
        );
        var area = new Area(
            new EntityName("Engineering Area"),
            campus
        );

        var existingBuilding = new Building(
            buildingId,
            new EntityName("Old Building"),
            Coordinates.Create(10.0, 20.0, 0.0),
            Dimension.Create(50.0, 30.0, 15.0),
            Colors.Create("Orange"),
            area
        );

        _areaServicesMock.Setup(s => s.GetByNameAsync("Engineering Area"))
            .ReturnsAsync(area);
        
        _buildingRepositoryMock.Setup(r => r.DisplayBuildingAsync(buildingId))
            .ReturnsAsync(existingBuilding);
        
        _buildingRepositoryMock.Setup(r => r.UpdateBuildingAsync(It.IsAny<Building>(), buildingId))
            .ReturnsAsync(true);

        // Act
        var result = await PutBuildingHandler.HandleAsync(_buildingServices, _areaServicesMock.Object, buildingId, buildingDto);

        // Assert
        Assert.IsType<Ok<PutBuildingResponse>>(result.Result);
        var okResult = (Ok<PutBuildingResponse>)result.Result;
        okResult.Value.Should().NotBeNull();
        okResult.Value!.Building.Should().BeEquivalentTo(buildingDto);
    }
}
