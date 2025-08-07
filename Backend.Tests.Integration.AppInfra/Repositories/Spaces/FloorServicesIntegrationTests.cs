using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Application;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AppInfra.Services.Spaces;

/// <summary>
/// Integration tests for FloorServices that test the interaction between
/// Application and Infrastructure layers for floor operations.
/// </summary>
public class FloorServicesIntegrationTests
{
    private readonly IFloorServices _floorServices;
    private readonly ThemeParkDataBaseContext _context;

    public FloorServicesIntegrationTests()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        _context = new ThemeParkDataBaseContext(options);

        // Pre-seed the complete entity hierarchy required by the SqlFloorRepository
        SeedTestData();

        _context.SaveChanges();

        // Setup services with in-memory database
        var services = new ServiceCollection();
        services.AddLogging();

        // Configure the same in-memory database for all services
        services.AddDbContext<ThemeParkDataBaseContext>(contextOptions =>
            contextOptions.UseInMemoryDatabase(databaseName: databaseName));

        // Register infrastructure services without DbContext registration
        AddInfrastructureServicesForTesting(services);
        services.AddApplicationLayerServices();

        var serviceProvider = services.BuildServiceProvider();
        _floorServices = serviceProvider.GetRequiredService<IFloorServices>();
    }

    /// <summary>
    /// Seeds the test database with the complete entity hierarchy required for floor operations.
    /// This includes University, Campus, Area, Building, and Floor entities.
    /// </summary>
    private void SeedTestData()
    {
        // Create University
        var university1 = new University(EntityName.Create("UCR Test"), EntityLocation.Create("Costa Rica"));
        var university2 = new University(EntityName.Create("UNA Test"), EntityLocation.Create("Costa Rica"));

        // Create Campuses
        var campus1 = new Campus(EntityName.Create("Rodrigo Facio"), EntityLocation.Create("San Pedro"), university1);
        var campus2 = new Campus(EntityName.Create("Campus Sarapiqui"), EntityLocation.Create("Sarapiqui"), university1);

        // Create Areas
        var area1 = new Area(EntityName.Create("Finca 1"), campus1);
        var area2 = new Area(EntityName.Create("Finca 2"), campus1);
        var area3 = new Area(EntityName.Create("Finca 3"), campus2);

        // Create Buildings
        var building1 = new Building(
            1,
            EntityName.Create("Engineering Building"),
            Coordinates.Create(9.9349, -84.0876, 1207),
            Dimension.Create(50, 100, 20),
            Colors.Create("Blue"),
            area1
        );

        var building2 = new Building(
            2,
            EntityName.Create("Science Building"),
            Coordinates.Create(9.9350, -84.0877, 1208),
            Dimension.Create(40, 80, 15),
            Colors.Create("Red"),
            area1
        );

        var building3 = new Building(
            3,
            EntityName.Create("Administration Building"),
            Coordinates.Create(9.9351, -84.0878, 1209),
            Dimension.Create(30, 60, 12),
            Colors.Create("Green"),
            area2
        );

        var building4 = new Building(
            4,
            EntityName.Create("Library Building"),
            Coordinates.Create(9.9352, -84.0879, 1210),
            Dimension.Create(60, 90, 18),
            Colors.Create("Yellow"),
            area3
        );

        // Add entities to context
        _context.University.AddRange(university1, university2);
        _context.Campus.AddRange(campus1, campus2);
        _context.Area.AddRange(area1, area2, area3);
        _context.Buildings.AddRange(building1, building2, building3, building4);

        // Save to get the auto-generated IDs
        _context.SaveChanges();

        // Create Floors and associate them with buildings using shadow properties
        var floor1 = new Floor(1, FloorNumber.Create(1));
        var floor2 = new Floor(2, FloorNumber.Create(2));
        var floor3 = new Floor(3, FloorNumber.Create(1)); // Different building, same floor number

        _context.Floors.AddRange(floor1, floor2, floor3);

        // Set the shadow property BuildingId for the floors
        _context.Entry(floor1).Property("BuildingId").CurrentValue = building1.BuildingInternalId;
        _context.Entry(floor2).Property("BuildingId").CurrentValue = building1.BuildingInternalId;
        _context.Entry(floor3).Property("BuildingId").CurrentValue = building2.BuildingInternalId;
    }

    private static void AddInfrastructureServicesForTesting(IServiceCollection services)
    {
        // Register the SqlFloor implementation for IFloorRepository interface
        services.AddTransient<IFloorRepository, SqlFloorRepository>();

        // Add other necessary repositories without DbContext registration
        // Note: Add other repositories here if needed for floor operations
    }

    /// <summary>
    /// Tests that GetFloorsListAsync returns floors for a valid building ID.
    /// </summary>
    [Fact]
    public async Task GetFloorsListAsync_WithValidBuildingId_ReturnsFloors()
    {
        // Arrange
        const int buildingId = 1;

        // Act
        var result = await _floorServices.GetFloorsListAsync(buildingId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<List<Floor>>();
    }

    /// <summary>
    /// Tests that GetFloorsListAsync returns empty list for a non-existent building ID.
    /// </summary>
    [Fact]
    public async Task GetFloorsListAsync_WithInvalidBuildingId_ThrowsNotFoundException()
    {
        // Arrange
        const int invalidBuildingId = 999;

        // Act & Assert
        await _floorServices.Invoking(x => x.GetFloorsListAsync(invalidBuildingId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Building with id 999 does not exist.");
    }

    /// <summary>
    /// Tests that GetFloorsListPaginatedAsync handles invalid building ID gracefully.
    /// </summary>
    [Fact]
    public async Task GetFloorsListPaginatedAsync_WithInvalidBuildingId_ThrowsNotFoundException()
    {
        // Arrange
        const int invalidBuildingId = 999;
        const int pageSize = 10;
        const int pageIndex = 0;

        // Act & Assert
        await _floorServices.Invoking(x => x.GetFloorsListPaginatedAsync(invalidBuildingId, pageSize, pageIndex))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Building with id 999 does not exist.");
    }

    /// <summary>
    /// Tests that CreateFloorAsync creates a new floor successfully for a valid building ID.
    /// </summary>
    [Fact]
    public async Task CreateFloorAsync_WithValidBuildingId_ReturnsTrue()
    {
        // Arrange
        const int buildingId = 2; // Using different building ID to avoid conflicts

        // Act
        var result = await _floorServices.CreateFloorAsync(buildingId);

        // Assert
        result.Should().BeTrue();

        // Verify the floor was actually created by checking the database
        var floors = await _floorServices.GetFloorsListAsync(buildingId);
        floors.Should().NotBeNull();
        if (floors != null)
        {
            floors.Should().NotBeEmpty();
        }
    }

    /// <summary>
    /// Tests that CreateFloorAsync handles invalid building ID appropriately.
    /// </summary>
    [Fact]
    public async Task CreateFloorAsync_WithInvalidBuildingId_ThrowsNotFoundException()
    {
        // Arrange
        const int invalidBuildingId = -1;

        // Act & Assert
        await _floorServices.Invoking(x => x.CreateFloorAsync(invalidBuildingId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Building with ID '-1' not found.");
    }

    /// <summary>
    /// Tests that DeleteFloorAsync deletes a floor successfully for a valid floor ID.
    /// </summary>
    [Fact]
    public async Task DeleteFloorAsync_WithValidFloorId_ReturnsTrue()
    {
        // Arrange
        const int floorId = 1; // Using an existing floor from our test data

        // Act
        var result = await _floorServices.DeleteFloorAsync(floorId);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that DeleteFloorAsync returns false for a non-existent floor ID.
    /// </summary>
    [Fact]
    public async Task DeleteFloorAsync_WithNonExistentFloorId_ThrowsNotFoundException()
    {
        // Arrange
        const int nonExistentFloorId = 999;

        // Act & Assert
        await _floorServices.Invoking(x => x.DeleteFloorAsync(nonExistentFloorId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Floor with id '999' not found.");
    }

    /// <summary>
    /// Tests the complete workflow: Create, Read, and Delete operations.
    /// </summary>
    [Fact]
    public async Task FloorServices_CompleteWorkflow_CreateReadDelete_WorksCorrectly()
    {
        // Arrange
        const int buildingId = 3; // Using a different building ID for this test

        // Act & Assert - Create
        var createResult = await _floorServices.CreateFloorAsync(buildingId);
        createResult.Should().BeTrue();

        // Act & Assert - Read
        var floors = await _floorServices.GetFloorsListAsync(buildingId);
        floors.Should().NotBeNull();
        if (floors != null)
        {
            floors.Should().NotBeEmpty();
            var firstFloor = floors.First();

            // Act & Assert - Delete
            var deleteResult = await _floorServices.DeleteFloorAsync(firstFloor.FloorId);
            deleteResult.Should().BeTrue();
        }
    }
}
