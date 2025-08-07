using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AppInfra.Repositories.UniversityManagement;

public class UniversityServicesTests
{
    private readonly ThemeParkDataBaseContext _context;
    private readonly IUniversityServices _service;

    public UniversityServicesTests()
    {
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ThemeParkDataBaseContext(options);

        IUniversityRepository repository = new SqlUniversityRepository(_context);
        _service = new UniversityServices(repository);
    }

    /// <summary>
    /// Tests that AddUniversityAsync returns true when a university is successfully added.
    /// </summary>
    [Fact]
    public async Task AddUniversityAsync_ReturnsTrue_WhenUniversityIsAdded()
    {
        var university = new University(
            name: new EntityName("Test University"),
            country: new EntityLocation("Costa Rica"));

        var result = await _service.AddUniversityAsync(university);

        result.Should().BeTrue();
        var saved = await _context.University.FirstOrDefaultAsync(u => u.Name == university.Name);
        saved.Should().NotBeNull();
        saved!.Country.Should().Be(university.Country);
    }

    /// <summary>
    /// Tests that AddUniversityAsync throws a DuplicatedEntityException when a university already exists.
    /// </summary>
    [Fact]
    public async Task AddUniversityAsync_ThrowsDuplicatedEntityException_WhenUniversityExists()
    {
        var university = new University(
            name: new EntityName("Duplicate University"),
            country: new EntityLocation("Costa Rica"));

        await _service.AddUniversityAsync(university);

        var duplicated = new University(
            name: new EntityName("Duplicate University"),
            country: new EntityLocation("Panamá"));

        var action = async () => await _service.AddUniversityAsync(duplicated);

        await Assert.ThrowsAsync<DuplicatedEntityException>(action);
    }

    /// <summary>
    /// Tests that GetByNameAsync returns the university when it exists in the database.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_ReturnsUniversity_WhenItExists()
    {
        var university = new University(
            name: new EntityName("UCR"),
            country: new EntityLocation("Costa Rica"));

        await _service.AddUniversityAsync(university);

        var result = await _service.GetByNameAsync("UCR");

        result.Should().NotBeNull();
        result!.Name.Should().Be(university.Name);
        result.Country.Should().Be(university.Country);
    }

    /// <summary>
    /// Tests that GetByNameAsync returns null when the university does not exist.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_ReturnsNull_WhenUniversityDoesNotExist()
    {
        var result = await _service.GetByNameAsync("NonExistent");

        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that ListUniversityAsync returns all universities added to the database.
    /// </summary>
    [Fact]
    public async Task ListUniversityAsync_ReturnsAllUniversities()
    {
        var uni1 = new University(new EntityName("UNA"), new EntityLocation("Costa Rica"));
        var uni2 = new University(new EntityName("TEC"), new EntityLocation("Costa Rica"));

        await _service.AddUniversityAsync(uni1);
        await _service.AddUniversityAsync(uni2);

        var result = (await _service.ListUniversityAsync()).ToList();

        result.Should().HaveCount(2);
        result.Should().Contain(u => u.Name == uni1.Name);
        result.Should().Contain(u => u.Name == uni2.Name);
    }

    /// <summary>
    /// Tests that ListUniversityAsync returns an empty list when no universities exist.
    /// </summary>
    [Fact]
    public async Task ListUniversityAsync_ReturnsEmpty_WhenNoUniversitiesExist()
    {
        var result = await _service.ListUniversityAsync();

        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that DeleteUniversityAsync returns true when the university is deleted.
    /// </summary>
    [Fact]
    public async Task DeleteUniversityAsync_ReturnsTrue_WhenUniversityIsDeleted()
    {
        var university = new University(
            new EntityName("DeleteMe"),
            new EntityLocation("Costa Rica"));

        await _service.AddUniversityAsync(university);

        var result = await _service.DeleteUniversityAsync("DeleteMe");

        result.Should().BeTrue();
        var exists = await _context.University.AnyAsync(u => u.Name == university.Name);
        exists.Should().BeFalse();
    }

    /// <summary>
    /// Tests that DeleteUniversityAsync returns false when the university does not exist.
    /// </summary>
    [Fact]
    public async Task DeleteUniversityAsync_ReturnsFalse_WhenUniversityDoesNotExist()
    {
        var result = await _service.DeleteUniversityAsync("FakeUniversity");

        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that DeleteUniversityAsync returns false when given an invalid name.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task DeleteUniversityAsync_ReturnsFalse_WhenNameIsInvalid(string name)
    {
        var result = await _service.DeleteUniversityAsync(name);

        result.Should().BeFalse();
    }
}
