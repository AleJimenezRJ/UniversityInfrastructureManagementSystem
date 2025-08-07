using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.Spaces;

public class SqlFloorRepositoryTests
{
    /// <summary>
    /// Repository under test.
    /// </summary>
    private readonly SqlFloorRepository _repository;

    /// <summary>
    /// Sample university entity used for test setup and assertions.
    /// </summary>
    private readonly University _university;

    /// <summary>
    /// Sample campus entity used for test setup and assertions.
    /// </summary>
    private readonly Campus _campus;

    /// <summary>
    /// Sample area entity used for test setup and assertions.
    /// </summary>
    private readonly Area _area;

    /// <summary>
    /// Sample building entity used for test setup and assertions.
    /// </summary>
    private readonly Building _building;

    /// <summary>
    /// Sample floor entity used for test setup and assertions.
    /// </summary>
    private Floor _floor;

    /// <summary>
    /// Expected count of floors in the database for testing purposes.
    /// </summary>
    private const int _expectedCount = 1;

    /// <summary>
    /// In-memory database context used for testing.
    /// </summary>
    private readonly ThemeParkDataBaseContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlFloorRepositoryTests"/> class.
    /// </summary>
    public SqlFloorRepositoryTests()
    {
        // Initialize the in-memory database context and sample entities for testing.
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ThemeParkDataBaseContext(options);

        // Create sample entities for the university, campus, area, and building.
        _university = new University(
            name: EntityName.Create("Test University"),
            country: EntityLocation.Create("Test Country")
        );

        _campus = new Campus(
            name: EntityName.Create("Test Campus"),
            location: EntityLocation.Create("Test City"),
            university: _university
        );

        _area = new Area(
            name: EntityName.Create("Test Area"),
            campus: _campus
        );

        _building = new Building(
            name: EntityName.Create("Test Building"),
            coordinates: Coordinates.Create(9.928, -84.086, 19),
            dimensions: Dimension.Create(50, 30, 10),
            color: Colors.Create("Blue"),
            area: _area
        );

        // Add the sample entities to the in-memory context.
        _context.University.Add(_university);
        _context.Campus.Add(_campus);
        _context.Area.Add(_area);
        _context.Buildings.Add(_building);

        // Create floor that will be used in tests.
        _floor = new Floor(
            number: FloorNumber.Create(1)
        );

        _context.Entry(_floor).Property("BuildingId").CurrentValue = _building.BuildingInternalId;
        _context.Floors.Add(_floor);
        _context.SaveChanges();

        var mockLogger = new Mock<ILogger<SqlFloorRepository>>();
        _repository = new SqlFloorRepository(_context, mockLogger.Object);
    }

    /// <summary>
    /// Tests the <see cref="CreateFloorAsync"/> method to ensure that a new floor is created successfully  when a valid
    /// building exists.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task CreateFloorAsync_WhenBuildingExists_ShouldCreateNewFloor()
    {
        // Arrange
        var buildingId = _building.BuildingInternalId;

        // Act
        var result = await _repository.CreateFloorAsync(buildingId);

        // Assert
        result.Should().BeTrue("a valid building exists and creation should succeed");

        var createdFloor = await _context.Floors.FirstOrDefaultAsync(f => f.Number.Value == 2 && EF.Property<int>(f, "BuildingId") == buildingId);
        
        Assert.NotNull(createdFloor);
    }

    /// <summary>
    /// Tests that the <see cref="_repository.CreateFloorAsync"/> method throws a <see cref="NotFoundException"/>  when
    /// attempting to create a floor for a building that does not exist.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task CreateFloorAsync_WhenBuildingDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var nonExistentBuildingId = 9999;

        // Act
        Func<Task> action = async () => await _repository.CreateFloorAsync(nonExistentBuildingId);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Building with ID '{nonExistentBuildingId}' not found.");
    }

    /// <summary>
    /// Tests that <see cref="_repository.ListFloorsAsync"/> returns a non-empty collection of floors when a building
    /// with the specified ID exists.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task ListFloorAsync_WhenBuildingExists_ShouldReturnFloors()
    {
        // Arrange
        var buildingId = _building.BuildingInternalId;

        // Act
        var floors = await _repository.ListFloorsAsync(buildingId);

        // Assert
        floors.Should().NotBeNullOrEmpty("the building has floors and should return them");

        floors.Should().ContainSingle(f => f.Number.Value == 1, "the floor with number 1 should be present");
    }

    /// <summary>
    /// Tests that <see cref="_repository.ListFloorsAsync"/> throws a <see cref="NotFoundException"/>  when attempting
    /// to list floors for a building that does not exist.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task ListFloorAsync_WhenBuildingDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var nonExistentBuildingId = 9999;

        // Act
        Func<Task> action = async () => await _repository.ListFloorsAsync(nonExistentBuildingId);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Building with id {nonExistentBuildingId} does not exist.");
    }

    /// <summary>
    /// Tests the <c>DeleteFloorAsync</c> method to ensure that it successfully deletes an existing floor.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task DeleteFloorAsync_WhenFloorExists_ShouldDeleteFloor()
    {
        // Arrange
        var floorId = _floor.FloorId;

        // Act
        var result = await _repository.DeleteFloorAsync(floorId);

        // Assert
        result.Should().BeTrue("the floor exists and should be deleted successfully");

        var deletedFloor = await _context.Floors.FindAsync(floorId);

        Assert.Null(deletedFloor);
    }

    /// <summary>
    /// Tests that <see cref="_repository.DeleteFloorAsync"/> throws a <see cref="NotFoundException"/>  when attempting
    /// to delete a floor that does not exist.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task DeleteFloorAsync_WhenFloorDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var nonExistentFloorId = 9999;

        // Act
        Func<Task> action = async () => await _repository.DeleteFloorAsync(nonExistentFloorId);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Floor with ID '{nonExistentFloorId}' not found.");
    }

    /// <summary>
    /// Verifies that paginated listing of floors for an existing building returns the correct number of floors.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListFloorsPaginatedAsync_WhenBuildingExists_ShouldReturnPaginatedFloors()
    {
        // Arrange
        var buildingId = _building.BuildingInternalId;
        var pageSize = 10;
        var pageIndex = 0;

        // Act
        var paginatedFloors = await _repository.ListFloorsPaginatedAsync(buildingId, pageSize, pageIndex);

        // Assert
        paginatedFloors.Should().NotBeEmpty("the building has floors and should return them");
        paginatedFloors.Should().HaveCount(1, "the building has one floor and should return it");
    }

    /// <summary>
    /// Verifies that paginated listing of floors for a non-existent building throws a <see cref="NotFoundException"/>.
    /// </summary>
    [Fact]
    public async Task ListFloorsPaginatedAsync_WhenBuildingDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var nonExistentBuildingId = 999;
        var pageSize = 10;
        var pageIndex = 0;

        // Act
        Func<Task> act = () => _repository.ListFloorsPaginatedAsync(nonExistentBuildingId, pageSize, pageIndex);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*does not exist*");
    }


    /// <summary>
    /// Verifies that paginated floors for an existing building returns the correct total count expected in tests.
    /// </summary>
    [Fact]
    public async Task ListFloorsPaginatedAsync_WhenBuildingExists_ShouldReturnExpectedCount()
    {
        // Arrange
        var buildingId = _building.BuildingInternalId;
        var pageSize = 10;
        var pageIndex = 0;

        // Act
        var result = await _repository.ListFloorsPaginatedAsync(_floor.FloorId, pageSize, pageIndex);

        // Assert
        result.TotalCount.Should().Be(_expectedCount,
            because: "the total count of learning spaces should match the expected count in tests");
    }

}
