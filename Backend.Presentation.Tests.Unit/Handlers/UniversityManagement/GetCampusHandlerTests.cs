using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;
/// <summary>
/// Unit tests for <see cref="GetCampusHandler"/>, which handles HTTP GET requests
/// </summary>
public class GetCampusHandlerTests
{
    /// <summary>
    /// Mock service for <see cref="ICampusServices"/> to simulate database operations
    /// </summary>
    private readonly Mock<ICampusServices> _campusServiceMock;
    /// <summary>
    /// Initializes the mock service used to simulate <see cref="ICampusServices"/> behavior.
    /// </summary>
    public GetCampusHandlerTests()
    {
        _campusServiceMock = new Mock<ICampusServices>(MockBehavior.Strict);
    }
    /// <summary>
    /// Tests that <see cref="GetCampusHandler.HandleAsync"/> returns a successful
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenExceptionThrown_ShouldReturnProblemResult()
    {
        // Arrange
        _campusServiceMock.Setup(s => s.ListCampusAsync())
            .ThrowsAsync(new Exception("Database failure"));

        // Act
        var result = await GetCampusHandler.HandleAsync(_campusServiceMock.Object);

        // Assert
        var problemResult = Assert.IsType<ProblemHttpResult>(result.Result);
        problemResult.ProblemDetails.Detail.Should().Contain("Database failure");
    }

    /// <summary>
    /// Tests that <see cref="GetCampusHandler.HandleAsync"/> returns a successful
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenNoCampuses_ShouldReturnEmptyList()
    {
        // Arrange
        _campusServiceMock.Setup(s => s.ListCampusAsync())
            .ReturnsAsync(new List<Campus>());

        // Act
        var result = await GetCampusHandler.HandleAsync(_campusServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetCampusResponse>>(result.Result);
        okResult.Value.Campus.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that <see cref="GetCampusHandler.HandleAsync"/> returns a successful
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenMultipleCampuses_ShouldReturnAllMappedCorrectly()
    {
        // Arrange
        var campus1 = TestCampus("Central", "UCR");
        var campus2 = TestCampus("Guápiles", "UTN");

        _campusServiceMock.Setup(s => s.ListCampusAsync())
            .ReturnsAsync(new List<Campus> { campus1, campus2 });

        // Act
        var result = await GetCampusHandler.HandleAsync(_campusServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetCampusResponse>>(result.Result);
        okResult.Value.Campus.Should().HaveCount(2);
        okResult.Value.Campus.Select(c => c.Name).Should().Contain(new[] { "Central", "Guápiles" });
        
    }
    /// <summary>
    /// Tests that <see cref="GetCampusHandler.HandleAsync"/> handles duplicate campus names correctly.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenDuplicateCampusNames_ShouldReturnAllAsSeparate()
    {
        // Arrange
        var university = new University(new EntityName("UCR"), new EntityLocation("CR"));

        var campus1 = new Campus(new EntityName("Sur"), new EntityLocation("Cartago"), university);
        var campus2 = new Campus(new EntityName("Sur"), new EntityLocation("Perez Zeledón"), university);

        _campusServiceMock.Setup(s => s.ListCampusAsync())
            .ReturnsAsync(new List<Campus> { campus1, campus2 });

        // Act
        var result = await GetCampusHandler.HandleAsync(_campusServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetCampusResponse>>(result.Result);
        okResult.Value.Campus.Should().HaveCount(2);
        okResult.Value.Campus[0].Name.Should().Be("Sur");
        okResult.Value.Campus[1].Name.Should().Be("Sur");
        okResult.Value.Campus[0].Location.Should().NotBe(okResult.Value.Campus[1].Location);
    }
    /// <summary>
    /// Tests that <see cref="GetCampusHandler.HandleAsync"/> returns campuses in the same order as the service.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnCampusesInSameOrderAsService()
    {
        // Arrange
        var campus1 = TestCampus("A", "UCR");
        var campus2 = TestCampus("B", "UCR");
        var campus3 = TestCampus("C", "UCR");

        _campusServiceMock.Setup(s => s.ListCampusAsync())
            .ReturnsAsync(new List<Campus> { campus1, campus2, campus3 });

        // Act
        var result = await GetCampusHandler.HandleAsync(_campusServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetCampusResponse>>(result.Result);
        okResult.Value.Campus.Should().HaveCount(3);
        okResult.Value.Campus.Select(c => c.Name).Should().ContainInOrder("A", "B", "C");
    }

    /// <summary>
    /// Creates a test <see cref="Campus"/> instance with the specified names for testing purposes.
    /// </summary>
    /// <param name="campusName"></param>
    /// <param name="universityName"></param>
    /// <returns></returns>
    private static Campus TestCampus(string campusName = "Rodrigo Facio", string universityName = "UCR")
    {
        var university = new University(
            new EntityName(universityName),
            new EntityLocation("Costa Rica")
        );

        return new Campus(
            new EntityName(campusName),
            new EntityLocation("San Ramón"),
            university
        );
    }
}
