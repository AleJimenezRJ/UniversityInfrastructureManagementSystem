using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations;

public class AreaServicesTests
{
    private readonly Mock<IAreaRepository> _areaRepositoryMock;
    private readonly AreaServices _serviceUnderTest;
    private readonly Area _area;

    public AreaServicesTests()
    {
        _areaRepositoryMock = new Mock<IAreaRepository>(MockBehavior.Strict);
        _serviceUnderTest = new AreaServices(_areaRepositoryMock.Object);

        var name = new EntityName("Finca 1");
        var universityName = new EntityName("UCR");
        var universityLocation = new EntityLocation("Costa Rica");
        var university = new University(universityName, universityLocation);
        var campusName = new EntityName("Rodrigo Facio");
        var campusLocation = new EntityLocation("San José");
        var campus = new Campus(campusName, campusLocation, university);

        _area = new Area(name, campus);
    }

    /// <summary>
    /// Tests that AddAreaAsync returns true when the repository successfully adds an area.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddAreaAsync_ReturnsTrue_WhenRepositoryReturnsTrue()
    {
        _areaRepositoryMock
            .Setup(repo => repo.AddAreaAsync(_area))
            .ReturnsAsync(true);

        var result = await _serviceUnderTest.AddAreaAsync(_area);

        result.Should().BeTrue();
        _areaRepositoryMock.Verify(repo => repo.AddAreaAsync(_area), Times.Once);
    }

    /// <summary>
    /// Tests that AddAreaAsync returns false when the repository fails to add an area.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddAreaAsync_ReturnsFalse_WhenRepositoryFails()
    {
        _areaRepositoryMock
            .Setup(repo => repo.AddAreaAsync(_area))
            .ReturnsAsync(false);

        var result = await _serviceUnderTest.AddAreaAsync(_area);

        result.Should().BeFalse();
        _areaRepositoryMock.Verify(repo => repo.AddAreaAsync(_area), Times.Once);
    }
    /// <summary>
    /// Tests that GetByNameAsync returns the expected area.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetByNameAsync_ReturnsArea_WhenFound()
    {
        var name = "Finca 1";

        _areaRepositoryMock
            .Setup(repo => repo.GetByNameAsync(name))
            .ReturnsAsync(_area);

        var result = await _serviceUnderTest.GetByNameAsync(name);

        result.Should().Be(_area);
        _areaRepositoryMock.Verify(repo => repo.GetByNameAsync(name), Times.Once);
    }
    /// <summary>
    /// Tests that GetByNameAsync returns null when the area is not found.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetByNameAsync_ReturnsNull_WhenNotFound()
    {
        var name = "Unknown Area";

        _areaRepositoryMock
            .Setup(repo => repo.GetByNameAsync(name))
            .ReturnsAsync((Area?)null);

        var result = await _serviceUnderTest.GetByNameAsync(name);

        result.Should().BeNull();
        _areaRepositoryMock.Verify(repo => repo.GetByNameAsync(name), Times.Once);
    }
    /// <summary>
    /// Tests that ListAreaAsync returns a list of area when the repository provides data.
    /// <returns></returns>
    [Fact]
    public async Task ListAreaAsync_ReturnsListOfAreas()
    {
        var areas = new List<Area> { _area };

        _areaRepositoryMock
            .Setup(repo => repo.ListAreaAsync())
            .ReturnsAsync(areas);

        var result = await _serviceUnderTest.ListAreaAsync();

        result.Should().ContainSingle().Which.Should().Be(_area);
        _areaRepositoryMock.Verify(repo => repo.ListAreaAsync(), Times.Once);
    }
    /// <summary>
    /// Tests that ListAreaAsync returns an empty list when no areas are available.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListAreaAsync_ReturnsEmptyList_WhenNoAreas()
    {
        _areaRepositoryMock
            .Setup(repo => repo.ListAreaAsync())
            .ReturnsAsync(new List<Area>());

        var result = await _serviceUnderTest.ListAreaAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _areaRepositoryMock.Verify(repo => repo.ListAreaAsync(), Times.Once);
    }
    /// <summary>
    /// Tests that DeleteAreaAsync returns true when the repository successfully deletes an area.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteAreaAsync_ReturnsTrue_WhenRepositoryDeletes()
    {
        var name = "Area Central";

        _areaRepositoryMock
            .Setup(repo => repo.DeleteAreaAsync(name))
            .ReturnsAsync(true);

        var result = await _serviceUnderTest.DeleteAreaAsync(name);

        result.Should().BeTrue();
        _areaRepositoryMock.Verify(repo => repo.DeleteAreaAsync(name), Times.Once);
    }
    /// <summary>
    /// Tests that DeleteAreaAsync returns false when the repository fails to delete an area.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteAreaAsync_ReturnsFalse_WhenRepositoryFails()
    {
        var name = "Nonexistent Area";

        _areaRepositoryMock
            .Setup(repo => repo.DeleteAreaAsync(name))
            .ReturnsAsync(false);

        var result = await _serviceUnderTest.DeleteAreaAsync(name);

        result.Should().BeFalse();
        _areaRepositoryMock.Verify(repo => repo.DeleteAreaAsync(name), Times.Once);
    }
    /// <summary>
    /// Tests that ListAreaAsync throws an exception when the repository throws an exception.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ListAreaAsync_ThrowsException_WhenRepositoryThrows()
    {
        _areaRepositoryMock
            .Setup(repo => repo.ListAreaAsync())
            .ThrowsAsync(new InvalidOperationException("DB error"));

        Func<Task> act = async () => await _serviceUnderTest.ListAreaAsync();

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("DB error");
    }
}
