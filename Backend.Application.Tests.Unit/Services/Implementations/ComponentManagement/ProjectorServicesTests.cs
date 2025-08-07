using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations.ComponentManagement;

/*
Supervised Activity: PI June 11th
Eleni Gougani and Angelica Vargas

PBIs:
-CPD-LC-001-005 Update a projector (Issue #24)
-CPD-LC-001-004 Create a projector (Issue #22)
-CPD-LC-001-001 List components in a learning space (Issue #9)

Completed tasks:
-Unit tests cases for ProjectorServices class, including(GetProjectorAsync, AddProjectorAsync, UpdateProjectorAsync methods).
- Unit tests cases for Projector class, including constructor and properties (Dimensions, Coordinates, Orientation, Area2D, ProjectedContent).
*/

/// <summary>
/// Unit tests for the <see cref="ProjectorServices"/> class, which manages projector components.
/// </summary>
public class ProjectorServicesTests
{
    /// <summary>
    /// Mock repository for projector data access.
    /// </summary>
    private readonly Mock<IProjectorRepository> _projectorRepositoryMock;
    /// <summary>
    /// The service under test.
    /// </summary>
    private readonly ProjectorServices _serviceUnderTest;
    /// <summary>
    /// A sample projector instance used for testing.
    /// </summary>
    private readonly Projector _projectorSample;
    /// <summary>
    /// A non-empty list of projectors for testing repository returns.
    /// </summary>
    private readonly List<Projector> _nonEmptyProjectorList;
    /// <summary>
    /// An empty list of projectors for testing repository returns.
    /// </summary>
    private readonly List<Projector> _emptyProjectorList;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectorServicesTests"/> class.
    /// Sets up the mock repository and test data.
    /// </summary>
    public ProjectorServicesTests()
    {
        _projectorRepositoryMock = new Mock<IProjectorRepository>(MockBehavior.Strict);
        _serviceUnderTest = new ProjectorServices(_projectorRepositoryMock.Object);

        _projectorSample = new Projector(
            projectedContent: "Sample content",
            projectionArea: Area2D.Create(5, 12),
            id: 1,
            orientation: Orientation.Create("North"),
            position: Coordinates.Create(7, 6, 4),
            dimensions: Dimension.Create(1, 1.6, 8));

        _nonEmptyProjectorList = new List<Projector> { _projectorSample };
        _emptyProjectorList = new List<Projector>();
    }

    /// <summary>
    /// Tests that <see cref="ProjectorServices.GetProjectorAsync"/> returns an empty list when the repository returns an empty list.
    /// </summary>
    [Fact]
    public async Task GetProjectorAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        _projectorRepositoryMock
            .Setup(projectorRepository => projectorRepository.GetAllAsync())
            .ReturnsAsync(_emptyProjectorList);

        // Act
        var result = await _serviceUnderTest.GetProjectorAsync();

        // Assert
        result.Should().BeSameAs(_emptyProjectorList, because: "service should forward whatever the repository returns");
    }

    /// <summary>
    /// Tests that <see cref="ProjectorServices.GetProjectorAsync"/> returns a non-empty list when the repository returns a non-empty list.
    /// </summary>
    [Fact]
    public async Task GetProjectorAsync_WhenRepositoryReturnsNonEmptyList_ReturnsNonEmptyList()
    {
        // Arrange
        _projectorRepositoryMock
            .Setup(projectorRepository => projectorRepository.GetAllAsync())
            .ReturnsAsync(_nonEmptyProjectorList);

        // Act
        var result = await _serviceUnderTest.GetProjectorAsync();

        // Assert
        result.Should().BeSameAs(_nonEmptyProjectorList, because: "service should forward whatever the repository returns");
    }

    /// <summary>
    /// Tests that <see cref="ProjectorServices.AddProjectorAsync"/> calls the repository's AddComponentAsync method with valid parameters.
    /// </summary>
    [Fact]
    public async Task AddProjectorAsync_WhenGivenValidParameters_CallsCreateOnRepository()
    {
        // Arrange
        int learningSpaceId = 5;
        _projectorRepositoryMock
            .Setup(projectorRepository => projectorRepository.AddComponentAsync(learningSpaceId, _projectorSample))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.AddProjectorAsync(learningSpaceId, _projectorSample);

        // Assert
        _projectorRepositoryMock.Verify(projectorRepository => projectorRepository.AddComponentAsync(5, _projectorSample),
            Times.Once, failMessage: "Service should always call AddComponentAsync on repository when adding a new projector");
    }

    /// <summary>
    /// Tests that <see cref="ProjectorServices.AddProjectorAsync"/> returns the result from the repository.
    /// </summary>
    /// <param name="expectedResult">The expected result from the repository.</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AddProjectorAsync_WhenGivenValidParameters_ReturnsResultFromRepository(bool expectedResult)
    {
        // Arrange
        int learningSpaceId = 5;
        _projectorRepositoryMock
            .Setup(projectorRepository => projectorRepository.AddComponentAsync(learningSpaceId, _projectorSample))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _serviceUnderTest.AddProjectorAsync(learningSpaceId, _projectorSample);

        // Assert
        result.Should().Be(expectedResult,
            because: "the service should forward the result from the repository");
    }

    /// <summary>
    /// Tests that <see cref="ProjectorServices.UpdateProjectorAsync"/> calls the repository's UpdateAsync method with valid parameters.
    /// </summary>
    [Fact]
    public async Task UpdateProjectorAsync_WhenGivenValidParameters_CallsUpdateOnRepository()
    {
        // Arrange
        int learningSpaceId = 5;
        int learningComponentId = 1;
        _projectorRepositoryMock
            .Setup(projectorRepository => projectorRepository.UpdateAsync(learningSpaceId, learningComponentId, _projectorSample))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.UpdateProjectorAsync(learningSpaceId, learningComponentId, _projectorSample);

        // Assert
        _projectorRepositoryMock.Verify(projectorRepository => projectorRepository.UpdateAsync(learningSpaceId, learningComponentId, _projectorSample),
            Times.Once, failMessage: "Service should always call UpdateAsync on repository when updating a projector");
    }

    /// <summary>
    /// Tests that <see cref="ProjectorServices.UpdateProjectorAsync"/> returns the result from the repository.
    /// </summary>
    /// <param name="expectedResult">The expected result from the repository.</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateProjectorAsync_WhenGivenValidParameters_ReturnsResultFromRepository(bool expectedResult)
    {
        // Arrange
        int learningSpaceId = 5;
        int learningComponentId = 1;
        _projectorRepositoryMock
            .Setup(projectorRepository => projectorRepository.UpdateAsync(learningSpaceId, learningComponentId, _projectorSample))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _serviceUnderTest.UpdateProjectorAsync(learningSpaceId, learningComponentId, _projectorSample);

        // Assert
        result.Should().Be(expectedResult,
            because: "the service should forward the result from the repository");
    }
}
