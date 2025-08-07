using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations;

public class CampusServicesTests
{
    private readonly Mock<ICampusRepository> _campusRepositoryMock;
    private readonly CampusServices _serviceUnderTest;
    private readonly Campus _campus;

    public CampusServicesTests()
    {
        _campusRepositoryMock = new Mock<ICampusRepository>(MockBehavior.Strict);
        _serviceUnderTest = new CampusServices(_campusRepositoryMock.Object);

        var name = new EntityName("Campus Central");
        var location = new EntityLocation("San José");
        var universityName = new EntityName("UCR");
        var universityLocation = new EntityLocation("Costa Rica");
        var university = new University(universityName, universityLocation);

        _campus = new Campus(name, location, university);
    }

    /// <summary>
    /// Tests that AddCampusAsync returns true when the repository successfully adds a campus.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddCampusAsync_ReturnsTrue_WhenRepositoryReturnsTrue()
    {
        _campusRepositoryMock
            .Setup(repo => repo.AddCampusAsync(_campus))
            .ReturnsAsync(true);

        var result = await _serviceUnderTest.AddCampusAsync(_campus);

        result.Should().BeTrue();
        _campusRepositoryMock.Verify(repo => repo.AddCampusAsync(_campus), Times.Once);
    }

    /// <summary>
    /// Tests that AddCampusAsync returns false when the repository fails to add a campus.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddCampusAsync_ReturnsFalse_WhenRepositoryFails()
    {
        _campusRepositoryMock
            .Setup(repo => repo.AddCampusAsync(_campus))
            .ReturnsAsync(false);

        var result = await _serviceUnderTest.AddCampusAsync(_campus);

        result.Should().BeFalse();
        _campusRepositoryMock.Verify(repo => repo.AddCampusAsync(_campus), Times.Once);
    }
    /// <summary>
    /// Tests that GetByNameAsync returns the expected campus.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetByNameAsync_ReturnsCampus_WhenFound()
    {
        var name = "Campus Central";

        _campusRepositoryMock
            .Setup(repo => repo.GetByNameAsync(name))
            .ReturnsAsync(_campus);

        var result = await _serviceUnderTest.GetByNameAsync(name);

        result.Should().Be(_campus);
        _campusRepositoryMock.Verify(repo => repo.GetByNameAsync(name), Times.Once);
    }
    /// <summary>
    /// Tests that GetByNameAsync returns null when the campus is not found.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetByNameAsync_ReturnsNull_WhenNotFound()
    {
        var name = "Unknown Campus";

        _campusRepositoryMock
            .Setup(repo => repo.GetByNameAsync(name))
            .ReturnsAsync((Campus?)null);

        var result = await _serviceUnderTest.GetByNameAsync(name);

        result.Should().BeNull();
        _campusRepositoryMock.Verify(repo => repo.GetByNameAsync(name), Times.Once);
    }
    /// <summary>
    /// Tests that ListCampusAsync returns a list of campus when the repository provides data.
    /// <returns></returns>
    [Fact]
    public async Task ListCampusAsync_ReturnsListOfCampuses()
    {
        var campuses = new List<Campus> { _campus };

        _campusRepositoryMock
            .Setup(repo => repo.ListCampusAsync())
            .ReturnsAsync(campuses);

        var result = await _serviceUnderTest.ListCampusAsync();

        result.Should().ContainSingle().Which.Should().Be(_campus);
        _campusRepositoryMock.Verify(repo => repo.ListCampusAsync(), Times.Once);
    }
    /// <summary>
    /// Tests that ListCampusAsync returns an empty list when no campuses are available.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListCampusAsync_ReturnsEmptyList_WhenNoCampuses()
    {
        _campusRepositoryMock
            .Setup(repo => repo.ListCampusAsync())
            .ReturnsAsync(new List<Campus>());

        var result = await _serviceUnderTest.ListCampusAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _campusRepositoryMock.Verify(repo => repo.ListCampusAsync(), Times.Once);
    }
    /// <summary>
    /// Tests that DeleteCampusAsync returns true when the repository successfully deletes a campus.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteCampusAsync_ReturnsTrue_WhenRepositoryDeletes()
    {
        var name = "Campus Central";

        _campusRepositoryMock
            .Setup(repo => repo.DeleteCampusAsync(name))
            .ReturnsAsync(true);

        var result = await _serviceUnderTest.DeleteCampusAsync(name);

        result.Should().BeTrue();
        _campusRepositoryMock.Verify(repo => repo.DeleteCampusAsync(name), Times.Once);
    }
    /// <summary>
    /// Tests that DeleteCampusAsync returns false when the repository fails to delete a campus.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteCampusAsync_ReturnsFalse_WhenRepositoryFails()
    {
        var name = "Nonexistent Campus";

        _campusRepositoryMock
            .Setup(repo => repo.DeleteCampusAsync(name))
            .ReturnsAsync(false);

        var result = await _serviceUnderTest.DeleteCampusAsync(name);

        result.Should().BeFalse();
        _campusRepositoryMock.Verify(repo => repo.DeleteCampusAsync(name), Times.Once);
    }
    /// <summary>
    /// Tests that ListCampusAsync throws an exception when the repository throws an exception.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListCampusAsync_ThrowsException_WhenRepositoryThrows()
    {
        _campusRepositoryMock
            .Setup(repo => repo.ListCampusAsync())
            .ThrowsAsync(new InvalidOperationException("DB error"));

        Func<Task> act = async () => await _serviceUnderTest.ListCampusAsync();

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("DB error");
    }
}
