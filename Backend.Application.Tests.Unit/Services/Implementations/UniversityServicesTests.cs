using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations;

public class UniversityServicesTests
{


    private readonly Mock<IUniversityRepository> _universityRepositoryMock;


    private readonly UniversityServices _serviceUnderTest;


    private readonly University _university;


    public UniversityServicesTests()
    {
        _universityRepositoryMock = new Mock<IUniversityRepository>(MockBehavior.Strict);
        _serviceUnderTest = new UniversityServices(_universityRepositoryMock.Object);

        var name = new EntityName("Main Test");
        var country = new EntityLocation("Costa Rica");

        _university = new University(
            name: name,
            country: country
        );
    }

    /// <summary>
    /// Tests that <see cref="UniversityServices.AddUniversityAsync(University)"/> returns true
    /// when the underlying repository successfully adds the university.
    /// </summary>
    [Fact]
    public async Task AddUniversityAsync_ReturnsTrue_WhenRepositoryReturnsTrue()
    {
        _universityRepositoryMock
            .Setup(repo => repo.AddUniversityAsync(_university))
            .ReturnsAsync(true);

        var result = await _serviceUnderTest.AddUniversityAsync(_university);

        result.Should().BeTrue();
        _universityRepositoryMock.Verify(repo => repo.AddUniversityAsync(_university), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="UniversityServices.AddUniversityAsync(University)"/> returns false
    /// when the underlying repository fails to add the university.
    /// </summary>
    [Fact]
    public async Task AddUniversityAsync_ReturnsFalse_WhenRepositoryFails()
    {
        _universityRepositoryMock
            .Setup(repo => repo.AddUniversityAsync(_university))
            .ReturnsAsync(false);

        var result = await _serviceUnderTest.AddUniversityAsync(_university);

        result.Should().BeFalse();
        _universityRepositoryMock.Verify(repo => repo.AddUniversityAsync(_university), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="UniversityServices.GetByNameAsync(string)"/> returns the expected university
    /// when it exists in the repository.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_ReturnsUniversity_WhenFound()
    {
        var name = "Main Test";

        _universityRepositoryMock
            .Setup(repo => repo.GetByNameAsync(name))
            .ReturnsAsync(_university);

        var result = await _serviceUnderTest.GetByNameAsync(name);

        result.Should().Be(_university);
        _universityRepositoryMock.Verify(repo => repo.GetByNameAsync(name), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="UniversityServices.GetByNameAsync(string)"/> returns null
    /// when the university is not found in the repository.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_ReturnsNull_WhenNotFound()
    {
        var name = "Nonexistent Uni";

        _universityRepositoryMock
            .Setup(repo => repo.GetByNameAsync(name))
            .ReturnsAsync((University?)null);

        var result = await _serviceUnderTest.GetByNameAsync(name);

        result.Should().BeNull();
        _universityRepositoryMock.Verify(repo => repo.GetByNameAsync(name), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="UniversityServices.ListUniversityAsync"/> returns
    /// a list of universities when the repository provides data.
    /// </summary>
    [Fact]
    public async Task ListUniversityAsync_ReturnsListOfUniversities()
    {
        var universities = new List<University> { _university };

        _universityRepositoryMock
            .Setup(repo => repo.ListUniversityAsync())
            .ReturnsAsync(universities);

        var result = await _serviceUnderTest.ListUniversityAsync();

        result.Should().ContainSingle().Which.Should().Be(_university);
        _universityRepositoryMock.Verify(repo => repo.ListUniversityAsync(), Times.Once);
    }


    /// <summary>
    /// Tests that <see cref="UniversityServices.ListUniversityAsync"/> returns an empty list
    /// when there are no universities in the repository.
    /// </summary>
    [Fact]
    public async Task ListUniversityAsync_ReturnsEmptyList_WhenNoUniversities()
    {
        _universityRepositoryMock
            .Setup(repo => repo.ListUniversityAsync())
            .ReturnsAsync(new List<University>());

        var result = await _serviceUnderTest.ListUniversityAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _universityRepositoryMock.Verify(repo => repo.ListUniversityAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="UniversityServices.DeleteUniversityAsync(string)"/> returns true
    /// when the repository successfully deletes the university.
    /// </summary>
    [Fact]
    public async Task DeleteUniversityAsync_ReturnsTrue_WhenRepositoryDeletes()
    {
        var name = "Main Test";

        _universityRepositoryMock
            .Setup(repo => repo.DeleteUniversityAsync(name))
            .ReturnsAsync(true);

        var result = await _serviceUnderTest.DeleteUniversityAsync(name);

        result.Should().BeTrue();
        _universityRepositoryMock.Verify(repo => repo.DeleteUniversityAsync(name), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="UniversityServices.DeleteUniversityAsync"/> returns false
    /// when a university deletion fails.
    /// </summary>
    [Fact]
    public async Task DeleteUniversityAsync_ReturnsFalse_WhenRepositoryFails()
    {
        var name = "Nonexistent";

        _universityRepositoryMock
            .Setup(repo => repo.DeleteUniversityAsync(name))
            .ReturnsAsync(false);

        var result = await _serviceUnderTest.DeleteUniversityAsync(name);

        result.Should().BeFalse();
        _universityRepositoryMock.Verify(repo => repo.DeleteUniversityAsync(name), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="UniversityServices.ListUniversityAsync"/> throws an <see cref="InvalidOperationException"/>
    /// when the underlying repository throws an exception.
    /// </summary>
    [Fact]
    public async Task ListUniversityAsync_ThrowsException_WhenRepositoryThrows()
    {
        _universityRepositoryMock
            .Setup(repo => repo.ListUniversityAsync())
            .ThrowsAsync(new InvalidOperationException("Database error"));

        Func<Task> act = async () => await _serviceUnderTest.ListUniversityAsync();

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error");
    }

}