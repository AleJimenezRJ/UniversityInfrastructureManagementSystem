
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.UniversityManagement;

public class SqlUniversityRepositoryTests
{
    private readonly List<University> _universityData;
    private readonly Mock<DbSet<University>> _mockUniversities;

    private readonly Mock<ThemeParkDataBaseContext> _mockContext;
    private readonly SqlUniversityRepository _repository;

    /// <summary>
    /// Initializes mock data, repositories, and context for the test suite.
    /// </summary>
    public SqlUniversityRepositoryTests()
    {
        _universityData = new List<University>();
        _mockUniversities = _universityData.AsQueryable().BuildMockDbSet();





        _mockContext = new Mock<ThemeParkDataBaseContext>();
        _mockContext.Setup(c => c.University).Returns(_mockUniversities.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        _repository = new SqlUniversityRepository(_mockContext.Object);
    }

    /// <summary>
    /// Tests that a university is successfully added if it doesn't already exist.
    /// </summary>
    [Fact]
    public async Task AddUniversityAsync_ShouldAddUniversity_WhenItDoesNotExist()
    {
        // Arrange
        var newUniversity = TestUniversity();

        _mockUniversities
            .Setup(m => m.Add(It.IsAny<University>()))
            .Callback<University>(u => _universityData.Add(u));

        // Act
        var result = await _repository.AddUniversityAsync(newUniversity);


        // Assert
        result.Should().BeTrue();
        _universityData.Should().ContainSingle(u => u.Name.Name == newUniversity.Name.Name);
    }

    /// <summary>
    /// Tests that a <see cref="DuplicatedEntityException"/> is thrown when trying to add a duplicate university.
    /// </summary>}
    [Fact]
    public async Task AddUniversityAsync_ShouldThrowDuplicatedEntityException_WhenUniversityExists()
    {
        // Arrange
        var university = TestUniversity();
        _universityData.Add(university);

        // Act
        Func<Task> act = async () => await _repository.AddUniversityAsync(university);

        // Assert
        await act.Should().ThrowAsync<DuplicatedEntityException>();
    }

    /// <summary>
    /// Tests that the <see cref="GetByNameAsync(string)"/> method returns null when the specified university does not exist.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenUniversityDoesNotExist()
    {
        // Act
        var result = await _repository.GetByNameAsync("NoExiste");

        // Assert
        result.Should().BeNull();
    }


    /// <summary>
    /// Tests that the <see cref="ListUniversityAsync"/> method returns all universities present in the repository.
    /// </summary>
    [Fact]
    public async Task ListUniversityAsync_ShouldReturnAllUniversities()
    {
        // Arrange
        _universityData.Add(TestUniversity("UCR"));
        _universityData.Add(TestUniversity("UNA"));
        _universityData.Add(TestUniversity("TEC"));

        // Act
        var result = await _repository.ListUniversityAsync();

        // Assert
        result.Should().HaveCount(3);
    }


    /// <summary>
    /// Tests that <see cref="DeleteUniversityAsync(string)"/> returns false when attempting to delete a university that does not exist.
    /// </summary>
    [Fact]
    public async Task DeleteUniversityAsync_ShouldReturnFalse_WhenUniversityDoesNotExist()
    {
        // Act
        var result = await _repository.DeleteUniversityAsync("Inexistente");

        // Assert
        result.Should().BeFalse();
    }


    /// <summary>
    /// Tests that <see cref="DeleteUniversityAsync(string)"/> returns false when the provided university name is invalid (empty).
    /// </summary>
    [Fact]
    public async Task DeleteUniversityAsync_ShouldReturnFalse_WhenNameIsInvalid()
    {
        // Act
        var result = await _repository.DeleteUniversityAsync("");

        // Assert
        result.Should().BeFalse();
    }


    /// <summary>
    /// Tests that <see cref="ListUniversityAsync"/> returns an empty list when there are no universities in the repository.
    /// </summary>
    [Fact]
    public async Task ListUniversityAsync_ShouldReturnEmptyList_WhenNoUniversities()
    {
        // Arrange
        _universityData.Clear();

        // Act
        var result = await _repository.ListUniversityAsync();

        // Assert
        result.Should().BeEmpty();
    }


    /// <summary>
    /// Tests that <see cref="AddUniversityAsync(University)"/> returns false when saving to the database fails (SaveChangesAsync returns 0).
    /// </summary>
    [Fact]
    public async Task AddUniversityAsync_ShouldReturnFalse_WhenSaveFails()
    {
        // Arrange
        var newUniversity = TestUniversity();
        _mockUniversities
            .Setup(m => m.Add(It.IsAny<University>()))
            .Callback<University>(u => _universityData.Add(u));
        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(0);

        // Act
        var result = await _repository.AddUniversityAsync(newUniversity);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that <see cref="GetByNameAsync(string)"/> returns null when the provided university name is invalid
    /// (null, empty, or whitespace).
    /// </summary>
    /// <param name="invalidName">An invalid university name (null, empty, or whitespace).</param>
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public async Task GetByNameAsync_ShouldReturnNull_WhenNameIsInvalid(string invalidName)
    {
        // Act
        var result = await _repository.GetByNameAsync(invalidName);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that <see cref="GetByNameAsync(string)"/> returns null when the provided university name is excessively long.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenNameIsVeryLong()
    {
        // Arrange
        var veryLongName = new string('a', 10000);

        // Act
        var result = await _repository.GetByNameAsync(veryLongName);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that <see cref="DeleteUniversityAsync(string)"/> returns false when saving changes to the database fails,
    /// and ensures the university is not removed from the data source.
    /// </summary>
    [Fact]
    public async Task DeleteUniversityAsync_ShouldReturnFalse_WhenSaveFails()
    {
        // Arrange
        var university = TestUniversity();
        _universityData.Add(university);

        _mockUniversities
            .Setup(m => m.Remove(It.IsAny<University>()))
            .Callback<University>(u => _universityData.Remove(u));

        _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(0);

        // Act
        var result = await _repository.DeleteUniversityAsync(university.Name.Name);

        // Assert
        result.Should().BeFalse();
        _universityData.Should().Contain(u => u.Name.Name == university.Name.Name);
    }

    /// <summary>
    /// Helper static method to create a standardized <see cref="University"/> object for testing.
    /// </summary>
    /// <returns>A new <see cref="University"/> instance with preset values.</returns>
    private static University TestUniversity(string name = "Universidad de Costa Rica")
    {
        var entityName = new EntityName(name);
        var Country = new EntityLocation("Costa Rica");

        return new University(entityName, Country);
    }
}
