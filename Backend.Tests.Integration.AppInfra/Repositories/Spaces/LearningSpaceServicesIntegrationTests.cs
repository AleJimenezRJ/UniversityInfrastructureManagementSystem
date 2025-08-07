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
/// Integration tests for LearningSpaceServices that test the interaction between
/// Application and Infrastructure layers for learning space operations.
/// </summary>
public class LearningSpaceServicesIntegrationTest
{
    private readonly ILearningSpaceServices _learningSpaceServices;
    private readonly ThemeParkDataBaseContext _context;

    public LearningSpaceServicesIntegrationTest()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        _context = new ThemeParkDataBaseContext(options);

        // Pre-seed the complete entity hierarchy required by the SqlLearningSpaceRepository
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
        _learningSpaceServices = serviceProvider.GetRequiredService<ILearningSpaceServices>();
    }

    /// <summary>
    /// Seeds the test database with the complete entity hierarchy required for learning space operations.
    /// This includes University, Campus, Area, Building, Floor, and LearningSpace entities.
    /// </summary>
    private void SeedTestData()
    {
        // Create University
        var university = new University(EntityName.Create("UCR Test"), EntityLocation.Create("Costa Rica"));

        // Create Campus
        var campus = new Campus(EntityName.Create("Rodrigo Facio"), EntityLocation.Create("San Pedro"), university);

        // Create Area
        var area = new Area(EntityName.Create("Finca 1"), campus);

        // Create Building
        var building = new Building(
            1,
            EntityName.Create("Engineering Building"),
            Coordinates.Create(9.9349, -84.0876, 1207),
            Dimension.Create(50, 100, 20),
            Colors.Create("Blue"),
            area
        );

        // Add entities to context
        _context.University.Add(university);
        _context.Campus.Add(campus);
        _context.Area.Add(area);
        _context.Buildings.Add(building);

        // Save to get the auto-generated IDs
        _context.SaveChanges();

        // Create Floor and associate it with building using shadow properties
        var floor = new Floor(1, FloorNumber.Create(1));
        _context.Floors.Add(floor);
        _context.Entry(floor).Property("BuildingId").CurrentValue = building.BuildingInternalId;

        // Save floor
        _context.SaveChanges();

        // Create Learning Spaces and associate them with floor using shadow properties
        var learningSpace1 = new LearningSpace(
            1,
            EntityName.Create("Lab 101"),
            LearningSpaceType.Create("Laboratory"),
            Capacity.Create(30),
            Size.Create(3.0),
            Size.Create(5.0),
            Size.Create(6.0),
            Colors.Create("White"),
            Colors.Create("Blue"),
            Colors.Create("Gray")
        );

        var learningSpace2 = new LearningSpace(
            2,
            EntityName.Create("Classroom 201"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(25),
            Size.Create(3.5),
            Size.Create(6.0),
            Size.Create(7.0),
            Colors.Create("Gray"),
            Colors.Create("White"),
            Colors.Create("Blue")
        );

        _context.LearningSpaces.AddRange(learningSpace1, learningSpace2);

        // Set the shadow property FloorId for the learning spaces
        _context.Entry(learningSpace1).Property("FloorId").CurrentValue = floor.FloorId;
        _context.Entry(learningSpace2).Property("FloorId").CurrentValue = floor.FloorId;
    }

    private static void AddInfrastructureServicesForTesting(IServiceCollection services)
    {
        // Register the SqlLearningSpace implementation for ILearningSpaceRepository interface
        services.AddTransient<ILearningSpaceRepository, SqlLearningSpaceRepository>();

        // Add other necessary repositories without DbContext registration
        services.AddTransient<IFloorRepository, SqlFloorRepository>();
    }

    /// <summary>
    /// Tests that GetLearningSpaceAsync returns the correct learning space when a valid ID is provided.
    /// </summary>
    [Fact]
    public async Task GetLearningSpaceAsync_WithValidId_ReturnsLearningSpace()
    {
        // Arrange
        const int validLearningSpaceId = 1;

        // Act
        var result = await _learningSpaceServices.GetLearningSpaceAsync(validLearningSpaceId);

        // Assert
        result.Should().NotBeNull();
        result!.LearningSpaceId.Should().Be(validLearningSpaceId);
        result.Name.Name.Should().Be("Lab 101");
        result.Type.Value.Should().Be("Laboratory");
        result.MaxCapacity.Value.Should().Be(30);
    }

    /// <summary>
    /// Tests that GetLearningSpaceAsync throws NotFoundException when an invalid ID is provided.
    /// </summary>
    [Fact]
    public async Task GetLearningSpaceAsync_WithInvalidId_ThrowsNotFoundException()
    {
        // Arrange
        const int invalidLearningSpaceId = 999;

        // Act & Assert
        await _learningSpaceServices.Invoking(x => x.GetLearningSpaceAsync(invalidLearningSpaceId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Learning Space with id '999' not found.");
    }

    /// <summary>
    /// Tests that GetLearningSpaceAsync throws NotFoundException when a negative ID is provided.
    /// </summary>
    [Fact]
    public async Task GetLearningSpaceAsync_WithNegativeId_ThrowsNotFoundException()
    {
        // Arrange
        const int negativeId = -1;

        // Act & Assert
        await _learningSpaceServices.Invoking(x => x.GetLearningSpaceAsync(negativeId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Learning Space with id '-1' not found.");
    }

    /// <summary>
    /// Tests that GetLearningSpaceAsync throws NotFoundException when zero ID is provided.
    /// </summary>
    [Fact]
    public async Task GetLearningSpaceAsync_WithZeroId_ThrowsNotFoundException()
    {
        // Arrange
        const int zeroId = 0;

        // Act & Assert
        await _learningSpaceServices.Invoking(x => x.GetLearningSpaceAsync(zeroId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Learning Space with id '0' not found.");
    }

    /// <summary>
    /// Tests that GetLearningSpaceAsync returns the correct learning space for the second seeded learning space.
    /// </summary>
    [Fact]
    public async Task GetLearningSpaceAsync_WithSecondValidId_ReturnsCorrectLearningSpace()
    {
        // Arrange
        const int validLearningSpaceId = 2;

        // Act
        var result = await _learningSpaceServices.GetLearningSpaceAsync(validLearningSpaceId);

        // Assert
        result.Should().NotBeNull();
        result!.LearningSpaceId.Should().Be(validLearningSpaceId);
        result.Name.Name.Should().Be("Classroom 201");
        result.Type.Value.Should().Be("Classroom");
        result.MaxCapacity.Value.Should().Be(25);
        result.Height.Value.Should().Be(3.5);
        result.Width.Value.Should().Be(6.0);
        result.Length.Value.Should().Be(7.0);
    }

    /// <summary>
    /// Tests that GetLearningSpaceAsync validates that all properties are correctly retrieved.
    /// </summary>
    [Fact]
    public async Task GetLearningSpaceAsync_WithValidId_ReturnsLearningSpaceWithAllProperties()
    {
        // Arrange
        const int validLearningSpaceId = 1;

        // Act
        var result = await _learningSpaceServices.GetLearningSpaceAsync(validLearningSpaceId);

        // Assert
        result.Should().NotBeNull();
        result!.LearningSpaceId.Should().Be(validLearningSpaceId);

        // Validate all properties
        result.Name.Should().NotBeNull();
        result.Name.Name.Should().Be("Lab 101");

        result.Type.Should().NotBeNull();
        result.Type.Value.Should().Be("Laboratory");

        result.MaxCapacity.Should().NotBeNull();
        result.MaxCapacity.Value.Should().Be(30);

        result.Height.Should().NotBeNull();
        result.Height.Value.Should().Be(3.0);

        result.Width.Should().NotBeNull();
        result.Width.Value.Should().Be(5.0);

        result.Length.Should().NotBeNull();
        result.Length.Value.Should().Be(6.0);

        result.ColorFloor.Should().NotBeNull();
        result.ColorFloor.Value.Should().Be("White");

        result.ColorWalls.Should().NotBeNull();
        result.ColorWalls.Value.Should().Be("Blue");

        result.ColorCeiling.Should().NotBeNull();
        result.ColorCeiling.Value.Should().Be("Gray");
    }

    /// <summary>
    /// Tests that GetLearningSpacesListAsync returns learning spaces for a valid floor ID.
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListAsync_WithValidFloorId_ReturnsLearningSpaces()
    {
        // Arrange
        const int validFloorId = 1;

        // Act
        var result = await _learningSpaceServices.GetLearningSpacesListAsync(validFloorId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result!.Should().Contain(ls => ls.Name.Name == "Lab 101");
        result!.Should().Contain(ls => ls.Name.Name == "Classroom 201");
    }

    /// <summary>
    /// Tests that GetLearningSpacesListAsync returns empty list for a floor with no learning spaces.
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListAsync_WithFloorWithoutLearningSpaces_ReturnsEmptyList()
    {
        // Arrange - Create a new floor without learning spaces
        var floor = new Floor(2, FloorNumber.Create(2));
        _context.Floors.Add(floor);
        _context.Entry(floor).Property("BuildingId").CurrentValue = 1; // Use existing building
        await _context.SaveChangesAsync();

        const int floorWithoutLearningSpaces = 2;

        // Act
        var result = await _learningSpaceServices.GetLearningSpacesListAsync(floorWithoutLearningSpaces);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that GetLearningSpacesListAsync throws NotFoundException for an invalid floor ID.
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListAsync_WithInvalidFloorId_ThrowsNotFoundException()
    {
        // Arrange
        const int invalidFloorId = 999;

        // Act & Assert
        await _learningSpaceServices.Invoking(x => x.GetLearningSpacesListAsync(invalidFloorId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Floor with Id '999' not found.");
    }

    /// <summary>
    /// Tests that GetLearningSpacesListPaginatedAsync returns paginated learning spaces for a valid floor ID.
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListPaginatedAsync_WithValidFloorId_ReturnsPaginatedLearningSpaces()
    {
        // Arrange
        const int validFloorId = 1;
        const int pageSize = 10;
        const int pageIndex = 0;
        const string searchText = "";

        // Act
        var result = await _learningSpaceServices.GetLearningSpacesListPaginatedAsync(validFloorId, pageSize, pageIndex, searchText);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<PaginatedList<LearningSpace>>();
        result.PageIndex.Should().Be(pageIndex);
        result.PageSize.Should().Be(pageSize);
        result.TotalCount.Should().Be(2);
        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Tests that GetLearningSpacesListPaginatedAsync filters learning spaces by search text.
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListPaginatedAsync_WithSearchText_ReturnsFilteredResults()
    {
        // Arrange
        const int validFloorId = 1;
        const int pageSize = 10;
        const int pageIndex = 0;
        const string searchText = "Lab";

        // Act
        var result = await _learningSpaceServices.GetLearningSpacesListPaginatedAsync(validFloorId, pageSize, pageIndex, searchText);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<PaginatedList<LearningSpace>>();
        result.Should().HaveCount(1);
        result.First().Name.Name.Should().Be("Lab 101");
    }

    /// <summary>
    /// Tests that GetLearningSpacesListPaginatedAsync handles pagination correctly.
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListPaginatedAsync_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        const int validFloorId = 1;
        const int pageSize = 1;
        const int pageIndex = 0;
        const string searchText = "";

        // Act
        var result = await _learningSpaceServices.GetLearningSpacesListPaginatedAsync(validFloorId, pageSize, pageIndex, searchText);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<PaginatedList<LearningSpace>>();
        result.PageIndex.Should().Be(pageIndex);
        result.PageSize.Should().Be(pageSize);
        result.TotalCount.Should().Be(2);
        result.Should().HaveCount(1); // Only one item per page
    }

    /// <summary>
    /// Tests that CreateLearningSpaceAsync creates a learning space successfully.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_WithValidData_ReturnsTrue()
    {
        // Arrange
        const int validFloorId = 1;
        var newLearningSpace = new LearningSpace(
            EntityName.Create("Auditorium 301"),
            LearningSpaceType.Create("Auditorium"),
            Capacity.Create(100),
            Size.Create(4.0),
            Size.Create(15.0),
            Size.Create(20.0),
            Colors.Create("Black"),
            Colors.Create("Red"),
            Colors.Create("White")
        );

        // Act
        var result = await _learningSpaceServices.CreateLearningSpaceAsync(validFloorId, newLearningSpace);

        // Assert
        result.Should().BeTrue();

        // Verify the learning space was created by checking the list
        var learningSpaces = await _learningSpaceServices.GetLearningSpacesListAsync(validFloorId);
        learningSpaces.Should().Contain(ls => ls.Name.Name == "Auditorium 301");
    }

    /// <summary>
    /// Tests that CreateLearningSpaceAsync throws NotFoundException for invalid floor ID.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_WithInvalidFloorId_ThrowsNotFoundException()
    {
        // Arrange
        const int invalidFloorId = 999;
        var newLearningSpace = new LearningSpace(
            EntityName.Create("Test Space"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(20),
            Size.Create(3.0),
            Size.Create(5.0),
            Size.Create(6.0),
            Colors.Create("White"),
            Colors.Create("Blue"),
            Colors.Create("Gray")
        );

        // Act & Assert
        await _learningSpaceServices.Invoking(x => x.CreateLearningSpaceAsync(invalidFloorId, newLearningSpace))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Floor with Id '999' not found.");
    }

    /// <summary>
    /// Tests that UpdateLearningSpaceAsync updates a learning space successfully.
    /// </summary>
    [Fact]
    public async Task UpdateLearningSpaceAsync_WithValidData_ReturnsTrue()
    {
        // Arrange
        const int learningSpaceId = 1;
        var updatedLearningSpace = new LearningSpace(
            learningSpaceId,
            EntityName.Create("Lab 101 Updated"),
            LearningSpaceType.Create("Laboratory"),
            Capacity.Create(35), // Changed capacity
            Size.Create(3.5), // Changed height
            Size.Create(5.5), // Changed width
            Size.Create(6.5), // Changed length
            Colors.Create("Gray"), // Changed floor color
            Colors.Create("Green"), // Changed wall color
            Colors.Create("Blue") // Changed ceiling color
        );

        // Act
        var result = await _learningSpaceServices.UpdateLearningSpaceAsync(learningSpaceId, updatedLearningSpace);

        // Assert
        result.Should().BeTrue();

        // Verify the learning space was updated
        var updatedSpace = await _learningSpaceServices.GetLearningSpaceAsync(learningSpaceId);
        updatedSpace.Should().NotBeNull();
        updatedSpace!.Name.Name.Should().Be("Lab 101 Updated");
        updatedSpace.MaxCapacity.Value.Should().Be(35);
        updatedSpace.Height.Value.Should().Be(3.5);
    }

    /// <summary>
    /// Tests that UpdateLearningSpaceAsync throws NotFoundException for non-existent learning space.
    /// </summary>
    [Fact]
    public async Task UpdateLearningSpaceAsync_WithNonExistentId_ThrowsNotFoundException()
    {
        // Arrange
        const int nonExistentId = 999;
        var learningSpace = new LearningSpace(
            nonExistentId,
            EntityName.Create("Test Space"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(20),
            Size.Create(3.0),
            Size.Create(5.0),
            Size.Create(6.0),
            Colors.Create("White"),
            Colors.Create("Blue"),
            Colors.Create("Gray")
        );

        // Act & Assert
        await _learningSpaceServices.Invoking(x => x.UpdateLearningSpaceAsync(nonExistentId, learningSpace))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Learning space with the Id '999' not found.");
    }

    /// <summary>
    /// Tests that DeleteLearningSpaceAsync deletes a learning space successfully.
    /// </summary>
    [Fact]
    public async Task DeleteLearningSpaceAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        const int learningSpaceId = 2; // Delete the second learning space

        // Act
        var result = await _learningSpaceServices.DeleteLearningSpaceAsync(learningSpaceId);

        // Assert
        result.Should().BeTrue();

        // Verify the learning space was deleted
        await _learningSpaceServices.Invoking(x => x.GetLearningSpaceAsync(learningSpaceId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Learning Space with id '2' not found.");
    }

    /// <summary>
    /// Tests that DeleteLearningSpaceAsync throws NotFoundException for non-existent learning space.
    /// </summary>
    [Fact]
    public async Task DeleteLearningSpaceAsync_WithNonExistentId_ThrowsNotFoundException()
    {
        // Arrange
        const int nonExistentId = 999;

        // Act & Assert
        await _learningSpaceServices.Invoking(x => x.DeleteLearningSpaceAsync(nonExistentId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Learning space with the Id '999' not found.");
    }

    /// <summary>
    /// Tests the complete workflow: Create, Read, Update, and Delete operations.
    /// </summary>
    [Fact]
    public async Task LearningSpaceServices_CompleteWorkflow_CRUD_WorksCorrectly()
    {
        // Arrange
        const int floorId = 1;
        var learningSpace = new LearningSpace(
            EntityName.Create("Workshop 401"),
            LearningSpaceType.Create("Laboratory"),
            Capacity.Create(15),
            Size.Create(3.0),
            Size.Create(8.0),
            Size.Create(10.0),
            Colors.Create("Yellow"),
            Colors.Create("Orange"),
            Colors.Create("Purple")
        );

        // Act & Assert - Create
        var createResult = await _learningSpaceServices.CreateLearningSpaceAsync(floorId, learningSpace);
        createResult.Should().BeTrue();

        // Act & Assert - Read (get the created learning space)
        var learningSpaces = await _learningSpaceServices.GetLearningSpacesListAsync(floorId);
        var createdSpace = learningSpaces!.FirstOrDefault(ls => ls.Name.Name == "Workshop 401");
        createdSpace.Should().NotBeNull();

        // Act & Assert - Update
        var updatedSpace = new LearningSpace(
            createdSpace!.LearningSpaceId,
            EntityName.Create("Workshop 401 Updated"),
            LearningSpaceType.Create("Laboratory"),
            Capacity.Create(20), // Changed capacity
            Size.Create(3.5),
            Size.Create(8.5),
            Size.Create(10.5),
            Colors.Create("Blue"),
            Colors.Create("White"),
            Colors.Create("Gray")
        );

        var updateResult = await _learningSpaceServices.UpdateLearningSpaceAsync(createdSpace.LearningSpaceId, updatedSpace);
        updateResult.Should().BeTrue();

        // Verify update
        var retrievedSpace = await _learningSpaceServices.GetLearningSpaceAsync(createdSpace.LearningSpaceId);
        retrievedSpace!.Name.Name.Should().Be("Workshop 401 Updated");
        retrievedSpace.MaxCapacity.Value.Should().Be(20);

        // Act & Assert - Delete
        var deleteResult = await _learningSpaceServices.DeleteLearningSpaceAsync(createdSpace.LearningSpaceId);
        deleteResult.Should().BeTrue();

        // Verify deletion
        await _learningSpaceServices.Invoking(x => x.GetLearningSpaceAsync(createdSpace.LearningSpaceId))
            .Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    /// Tests basic service integration without complex entity creation.
    /// This validates that the Application and Infrastructure layers are properly connected.
    /// </summary>
    [Fact]
    public async Task LearningSpaceServices_ServiceIntegration_WorksCorrectly()
    {
        // Arrange
        const int floorId = 1;
        const int pageSize = 10;
        const int pageIndex = 0;
        const string searchText = "";

        // Act
        var result = await _learningSpaceServices.GetLearningSpacesListPaginatedAsync(floorId, pageSize, pageIndex, searchText);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<PaginatedList<LearningSpace>>();
    }
}
