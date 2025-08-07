using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="SqlCampusRepository"/> which manages CRUD operations for Campus entities.
/// </summary>
/// 
/// PBI: #269 - Pruebas unitarias para backend <see href="https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/269"/>
/// Tareas implementadas:
/// Review backend components and responsibilities 	
/// Define test cases for each method and controller logic 	
/// Write unit tests for key services and API handlers 
/// Add assertion logic for all expected outcomes 
/// Run tests and ensure all pass

public class SqlCampusRepositoryTests
{
    private readonly Mock<DbSet<Campus>> _mockCampuses;
    private readonly Mock<ThemeParkDataBaseContext> _mockContext;
    private readonly List<Campus> _campusData;
    private readonly SqlCampusRepository _repository;

    /// <summary>
    /// Initializes mock data, repositories, and context for the test suite.
    /// </summary>
    public SqlCampusRepositoryTests()
    {
        _campusData = new List<Campus>();
        _mockCampuses = _campusData.AsQueryable().BuildMockDbSet();

        _mockContext = new Mock<ThemeParkDataBaseContext>();
        _mockContext.Setup(c => c.Campus).Returns(_mockCampuses.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        _repository = new SqlCampusRepository(_mockContext.Object);
    }

    /// <summary>
    /// Tests that an campus is successfully added if it doesn't already exist.
    /// </summary>
    [Fact]
    public async Task AddCampusAsync_ShouldAddCampus_WhenItDoesNotExist()
    {
        // Arrange
        var newCampus = TestCampus();

        _mockCampuses
            .Setup(m => m.Add(It.IsAny<Campus>()))
            .Callback<Campus>(b => _campusData.Add(b));

        // Act
        var result = await _repository.AddCampusAsync(newCampus);

        // Assert
        result.Should().BeTrue();
        _campusData.Should().ContainSingle(a => a.Name!.Name == newCampus.Name!.Name);
    }

    /// <summary>
    /// Tests that a <see cref="DuplicatedEntityException"/> is thrown when trying to add a duplicate campus.
    /// </summary>}
    [Fact]
    public async Task AddCampusAsync_ShouldThrowDuplicatedEntityException_WhenCampusExists()
    {
        // Arrange
        var newCampus = TestCampus();
        _campusData.Add(newCampus);

        // Act
        Func<Task> act = async () => await _repository.AddCampusAsync(newCampus);

        // Assert
        await act.Should().ThrowAsync<DuplicatedEntityException>();
    }

    /// <summary>
    /// Tests that the <see cref="AddCampusAsync"/> method returns false when saving the changes fails.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddCampusAsync_ShouldReturnFalse_WhenSaveFails()
    {
        // Arrange
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(0);
        var newCampus = TestCampus();
        // Act
        var result = await _repository.AddCampusAsync(newCampus);
        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that the result of getting a non-existent campus is null.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenCampusDoesNotExist()
    {
        // Act
        var result = await _repository.GetByNameAsync("Campus");

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that <see cref="GetByNameAsync(string)"/> returns null when the provided campus name is excessively long.
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
    /// Tests that the <see cref="GetByNameAsync(string)"/> method returns null when the provided campus name is empty or null.
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
    /// Tests that the <see cref="ListCampusAsync"/> method returns an list containing all the campuses in the database.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListCampusesAsync_ShouldReturnCampuses_WhenCampusesExist()
    {
        // Arrange
        _campusData.Add(TestCampus());
        _campusData.Add(TestCampus("New Campus"));
        _campusData.Add(TestCampus("Other Campus"));

        // Act
        var result = await _repository.ListCampusAsync();

        // Assert
        result.Should().HaveCount(_campusData.Count);
    }

    /// <summary>
    /// Tests that the <see cref="ListCampusAsync"/> method returns an empty list when no campuses exist in the database.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListCampusesAsync_ShouldReturnEmptyList_WhenNoCampusesExist()
    {
        // Act
        var result = await _repository.ListCampusAsync();

        // Assert
        result.Should().HaveCount(0);
    }

    /// <summary>
    /// Tests that the <see cref="DeleteCampusAsync"/> method returns false when trying to delete a non-existent campus.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteCampusAsync_ShouldReturnFalse_WhenCampusDoesNotExist()
    {
        // Act
        var result = await _repository.DeleteCampusAsync("Campus");
        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that the <see cref="DeleteCampusAsync"/> method returns false when trying to delete an campus with an emtpy name.
    /// </summary>
    /// <returns></returns>
    

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task DeleteCampusAsync_ShouldReturnFalse_WhenCampusNameIsInvalid(string name)
    {
        // Act
        var result = await _repository.DeleteCampusAsync(name);
        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that the <see cref="DeleteCampusAsync"/> method returns false when the save operation fails.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteCampusAsync_ShouldReturnFalse_WhenSaveFails()
    {
        // Arrange
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(0);
        // Act
        var result = await _repository.DeleteCampusAsync("Campus");
        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Helper static method to create a standardized <see cref="Campus"/> object for testing.
    /// </summary>
    /// <returns>A new <see cref="Campus"/> instance with preset values.</returns>
    private static Campus TestCampus(string name = "Campus")
    {
        var university = new University(new EntityName("U"));

        return new Campus(new EntityName(name), new EntityLocation("Loc"), university);
    }
}
