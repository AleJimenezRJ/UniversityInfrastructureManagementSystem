using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="SqlAreaRepository"/> which manages CRUD operations for Area entities.
/// </summary>
public class SqlAreaRepositoryTests
{
    private readonly Mock<DbSet<Area>> _mockAreas;
    private readonly Mock<ThemeParkDataBaseContext> _mockContext;
    private readonly Mock<ICampusRepository> _mockCampusRepository;
    private readonly List<Area> _areaData;
    private readonly SqlAreaRepository _repository;

    /// <summary>
    /// Initializes mock data, repositories, and context for the test suite.
    /// </summary>
    public SqlAreaRepositoryTests()
    {
        _areaData = new List<Area>();
        _mockAreas = _areaData.AsQueryable().BuildMockDbSet();

        _mockContext = new Mock<ThemeParkDataBaseContext>();
        _mockContext.Setup(c => c.Area).Returns(_mockAreas.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        _mockCampusRepository = new Mock<ICampusRepository>();
        _repository = new SqlAreaRepository(_mockContext.Object, _mockCampusRepository.Object);
    }

    /// <summary>
    /// Tests that an area is successfully added if it doesn't already exist.
    /// </summary>
    [Fact]
    public async Task AddAreaAsync_ShouldAddArea_WhenItDoesNotExist()
    {
        // Arrange
        var newArea = TestArea();

        _mockAreas
            .Setup(m => m.Add(It.IsAny<Area>()))
            .Callback<Area>(b => _areaData.Add(b));

        // Act
        var result = await _repository.AddAreaAsync(newArea);

        // Assert
        result.Should().BeTrue();
        _areaData.Should().ContainSingle(a => a.Name!.Name == newArea.Name!.Name);
    }

    /// <summary>
    /// Tests that a <see cref="DuplicatedEntityException"/> is thrown when trying to add a duplicate area.
    /// </summary>}
    [Fact]
    public async Task AddAreaAsync_ShouldThrowDuplicatedEntityException_WhenAreaExists()
    {
        // Arrange
        var newArea = TestArea();
        _areaData.Add(newArea);

        // Act
        Func<Task> act = async () => await _repository.AddAreaAsync(newArea);

        // Assert
        await act.Should().ThrowAsync<DuplicatedEntityException>();
    }

    /// <summary>
    /// Tests that the <see cref="AddAreaAsync"/> method returns false when saving the changes fails.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddAreaAsync_ShouldReturnFalse_WhenSaveFails()
    {
        // Arrange
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(0);
        var newArea = TestArea();
        // Act
        var result = await _repository.AddAreaAsync(newArea);
        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that the result of getting a non-existent area is null.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenAreaDoesNotExist()
    {
        // Act
        var result = await _repository.GetByNameAsync("Area");

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that <see cref="GetByNameAsync(string)"/> returns null when the provided area name is excessively long.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenNameIsVeryLong()
    {
        // Arrange
        var longName = new string('A', 101); 
        // Act
        var result = await _repository.GetByNameAsync(longName);
        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that the <see cref="GetByNameAsync(string)"/> method returns null when the provided area name is empty or null.
    /// </summary>
    /// <returns></returns>
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetByNameAsync_ShouldReturnNull_WhenNameIsInvalid(string name)
    {
        // Act
        var result = await _repository.GetByNameAsync(name);

        // Assert
        result.Should().BeNull();
    }


    /// <summary>
    /// Tests that the <see cref="ListAreaAsync"/> method returns an list containing all the areas in the database.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListAreasAsync_ShouldReturnAreas_WhenAreasExist()
    {
        // Arrange
        _areaData.Add(TestArea());
        _areaData.Add(TestArea("New Area"));
        _areaData.Add(TestArea("Other Area"));

        // Act
        var result = await _repository.ListAreaAsync();

        // Assert
        result.Should().HaveCount(_areaData.Count);
    }

    /// <summary>
    /// Tests that the <see cref="ListAreaAsync"/> method returns an empty list when no areas exist in the database.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListAreasAsync_ShouldReturnEmptyList_WhenNoAreasExist()
    {
        // Act
        var result = await _repository.ListAreaAsync();

        // Assert
        result.Should().HaveCount(0);
    }

    /// <summary>
    /// Tests that the <see cref="DeleteAreaAsync"/> method returns false when trying to delete a non-existent area.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteAreaAsync_ShouldReturnFalse_WhenAreaDoesNotExist()
    {
        // Act
        var result = await _repository.DeleteAreaAsync("Area");
        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that the <see cref="DeleteAreaAsync"/> method returns false when trying to delete an area with an emtpy name.
    /// </summary>
    /// <returns></returns>
    

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task DeleteAreaAsync_ShouldReturnFalse_WhenAreaNameIsInvalid(string name)
    {
        // Act
        var result = await _repository.DeleteAreaAsync(name);
        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that the <see cref="DeleteAreaAsync"/> method returns false when the save operation fails.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteAreaAsync_ShouldReturnFalse_WhenSaveFails()
    {
        // Arrange
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(0);
        // Act
        var result = await _repository.DeleteAreaAsync("Area");
        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Helper static method to create a standardized <see cref="Area"/> object for testing.
    /// </summary>
    /// <returns>A new <see cref="Area"/> instance with preset values.</returns>
    private static Area TestArea(string name = "Area")
    {
        var university = new University(new EntityName("U"));
        var campus = new Campus(new EntityName("C"), new EntityLocation("Loc"), university);

        return new Area(new EntityName(name), campus);
    }
}
