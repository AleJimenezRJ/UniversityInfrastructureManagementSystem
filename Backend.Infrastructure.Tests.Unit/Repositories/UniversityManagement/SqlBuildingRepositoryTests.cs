using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="SqlBuildingRepository"/> which manages CRUD operations for Building entities.
/// </summary>
public class SqlBuildingRepositoryTests
{
    private readonly Mock<DbSet<Building>> _mockBuildings;
    private readonly Mock<ThemeParkDataBaseContext> _mockContext;
    private readonly Mock<IAreaRepository> _mockAreaRepo;
    private readonly List<Building> _buildingData;
    private readonly SqlBuildingRepository _repository;

    /// <summary>
    /// Initializes mock data, repositories, and context for the test suite.
    /// </summary>
    public SqlBuildingRepositoryTests()
    {
        _buildingData = new List<Building>();
        _mockBuildings = _buildingData.AsQueryable().BuildMockDbSet();

        _mockContext = new Mock<ThemeParkDataBaseContext>();
        _mockContext.Setup(c => c.Buildings).Returns(_mockBuildings.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        _mockAreaRepo = new Mock<IAreaRepository>();
        _repository = new SqlBuildingRepository(_mockContext.Object, _mockAreaRepo.Object);
    }

    /// <summary>
    /// Tests that a building is successfully added if it doesn't already exist.
    /// </summary>
    [Fact]
    public async Task AddBuildingAsync_ShouldAddBuilding_WhenItDoesNotExist()
    {
        // Arrange
        var newBuilding = TestBuilding();

        _mockBuildings
            .Setup(m => m.Add(It.IsAny<Building>()))
            .Callback<Building>(b => _buildingData.Add(b));

        // Act
        var result = await _repository.AddBuildingAsync(newBuilding);

        // Assert
        result.Should().BeTrue();
        _buildingData.Should().ContainSingle(b => b.Name!.Name == newBuilding.Name!.Name);
    }

    /// <summary>
    /// Tests that an existing building is updated correctly.
    /// </summary>
    [Fact]
    public async Task UpdateBuildingAsync_ShouldUpdate_WhenBuildingExists()
    {
        // Arrange
        var existing = TestBuilding();
        _buildingData.Add(existing);

        var update = TestBuilding();
        update.Color = new Colors("Red");

        // Act
        var result = await _repository.UpdateBuildingAsync(update, existing.BuildingInternalId);

        // Assert
        result.Should().BeTrue();
        _buildingData[0].Color!.Value.Should().Be("Red");
    }

    /// <summary>
    /// Tests that updating a non-existent building returns false.
    /// </summary>
    [Fact]
    public async Task UpdateBuildingAsync_ShouldReturnFalse_WhenNotFound()
    {
        // Arrange
        var update = TestBuilding();

        // Act
        var result = await _repository.UpdateBuildingAsync(update, 999);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests retrieval of a specific building by ID when it exists.
    /// </summary>
    [Fact]
    public async Task DisplayBuildingAsync_ShouldReturnBuilding_WhenFound()
    {
        // Arrange
        var building = TestBuilding();
        _buildingData.Add(building);

        // Act
        var result = await _repository.DisplayBuildingAsync(building.BuildingInternalId);

        // Assert
        result.Should().BeEquivalentTo(building);
    }

    /// <summary>
    /// Tests that retrieving a non-existent building returns null.
    /// </summary>
    [Fact]
    public async Task DisplayBuildingAsync_ShouldReturnNull_WhenNotFound()
    {
        // Act
        var result = await _repository.DisplayBuildingAsync(999);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that all buildings are listed correctly.
    /// </summary>
    [Fact]
    public async Task ListBuildingAsync_ShouldReturnAllBuildings()
    {
        // Arrange
        _buildingData.Add(TestBuilding());

        // Act
        var result = await _repository.ListBuildingAsync();

        // Assert
        result.Should().HaveCount(_buildingData.Count);
    }

    /// <summary>
    /// Tests successful deletion of an existing building.
    /// </summary>
    [Fact]
    public async Task DeleteBuildingAsync_ShouldDelete_WhenExists()
    {
        // Arrange
        var building = TestBuilding();
        _buildingData.Add(building);

        _mockBuildings
            .Setup(m => m.Remove(It.IsAny<Building>()))
            .Callback<Building>(b => _buildingData.Remove(b));

        // Act
        var result = await _repository.DeleteBuildingAsync(building.BuildingInternalId);

        // Assert
        result.Should().BeTrue();
        _buildingData.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that deleting a non-existent building returns false.
    /// </summary>
    [Fact]
    public async Task DeleteBuildingAsync_ShouldReturnFalse_WhenNotExists()
    {
        // Act
        var result = await _repository.DeleteBuildingAsync(999);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Helper static method to create a standardized <see cref="Building"/> object for testing.
    /// </summary>
    /// <returns>A new <see cref="Building"/> instance with preset values.</returns>
    private static Building TestBuilding()
    {
        var name = new EntityName("Test");
        var coord = new Coordinates(1.1, 2.2, 3.3);
        var dim = new Dimension(10, 20, 30);
        var color = new Colors("Blue");
        var university = new University(new EntityName("U"));
        var campus = new Campus(new EntityName("C"), new EntityLocation("Loc"), university);
        var area = new Area(new EntityName("A"), campus);

        return new Building(1, name, coord, dim, color, area);
    }

    /// <summary>
    /// Tests that adding a building throws a <see cref="DuplicatedEntityException"/> when a building with the same name already exists.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddBuildingAsync_ShouldThrowDuplicatedEntityException_WhenNameAlreadyExists()
    {
        // Arrange
        var building = TestBuilding();
        _buildingData.Add(building);

        // Act
        Func<Task> act = async () => await _repository.AddBuildingAsync(building);

        // Assert
        await act.Should().ThrowAsync<DuplicatedEntityException>()
            .WithMessage($"*{building.Name.Name}*");
    }
    /// <summary>
    /// Tests that updating a building does not overwrite fields with null values.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task UpdateBuildingAsync_ShouldNotOverwriteFields_WhenNewValuesAreNull()
    {
        // Arrange
        var existing = TestBuilding();
        existing.Color = new Colors("Green");
        _buildingData.Add(existing);

        var update = TestBuilding();
        update.Color = null;

        // Act
        var result = await _repository.UpdateBuildingAsync(update, existing.BuildingInternalId);

        // Assert
        result.Should().BeTrue();
        _buildingData.First().Color!.Value.Should().Be("Green"); // no se sobreescribió
    }
    /// <summary>
    /// Tests that deleting a building that does not exist returns false without throwing an exception.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteBuildingAsync_ShouldHandleNonExistingBuildingGracefully()
    {
        // Act
        var result = await _repository.DeleteBuildingAsync(12345);

        // Assert
        result.Should().BeFalse();
    }

}
