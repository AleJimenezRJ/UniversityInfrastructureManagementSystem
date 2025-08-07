using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;
/// <summary>
/// Unit tests for <see cref="GetCampusByNameHandler"/>, which handles HTTP GET requests
/// </summary>
public class GetCampusByNameHandlerTests
{
    /// <summary>
    /// Mock service for <see cref="ICampusServices"/> to simulate database operations
    /// </summary>
    private readonly Mock<ICampusServices> _campusServiceMock;
    /// <summary>
    /// Initializes the mock service used to simulate <see cref="ICampusServices"/> behavior.
    /// </summary>
    public GetCampusByNameHandlerTests()
    {
        _campusServiceMock = new Mock<ICampusServices>(MockBehavior.Strict);
    }
    /// <summary>
    /// Tests that <see cref="GetCampusByNameHandler.HandleAsync"/> returns a successful
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenExceptionThrown_ShouldReturnProblemResult()
    {
        _campusServiceMock.Setup(s => s.GetByNameAsync("A"))
            .ThrowsAsync(new Exception("Database failure"));

        var result = await GetCampusByNameHandler.HandleAsync(_campusServiceMock.Object, "A");

        var problemResult = Assert.IsType<NotFound<string>>(result.Result);
        problemResult.Value.Should().Be("An error occurred while retrieving the campus: Database failure");
    }


    /// <summary>
    /// Tests that <see cref="GetCampusByNameHandler.HandleAsync"/> returns Ok when the campus exists.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenCampusExists_ShouldReturnOkResult()
    {
        // Arrange
        var campus = TestCampus("A", "UCR");

        var campusServiceMock = new Mock<ICampusServices>();
        campusServiceMock
            .Setup(s => s.GetByNameAsync("A"))
            .ReturnsAsync(campus);

        // Act
        var result = await GetCampusByNameHandler.HandleAsync(campusServiceMock.Object, "A");

        // Assert
        result.Result.Should().BeOfType<Ok<GetCampusByNameResponse>>();
    }

    /// <summary>
    /// Tests that <see cref="GetCampusByNameHandler.HandleAsync"/> maps the campus name correctly in the response when the campus exists.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenCampusExists_ShouldMapCampusNameCorrectly()
    {
        // Arrange
        var campus = TestCampus("A", "UCR");

        var campusServiceMock = new Mock<ICampusServices>();
        campusServiceMock
            .Setup(s => s.GetByNameAsync("A"))
            .ReturnsAsync(campus);

        // Act
        var result = await GetCampusByNameHandler.HandleAsync(campusServiceMock.Object, "A");
        var okResult = Assert.IsType<Ok<GetCampusByNameResponse>>(result.Result);

        // Assert
        okResult.Value!.Campus.Name.Should().Be("A");
    }

    /// <summary>
    /// Tests that <see cref="GetCampusByNameHandler.HandleAsync"/> maps the university name correctly in the response when the campus exists.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenCampusExists_ShouldMapUniversityNameCorrectly()
    {
        // Arrange
        var campus = TestCampus("A", "UCR");

        var campusServiceMock = new Mock<ICampusServices>();
        campusServiceMock
            .Setup(s => s.GetByNameAsync("A"))
            .ReturnsAsync(campus);

        // Act
        var result = await GetCampusByNameHandler.HandleAsync(campusServiceMock.Object, "A");
        var okResult = Assert.IsType<Ok<GetCampusByNameResponse>>(result.Result);

        // Assert
        okResult.Value!.Campus.University.Should().NotBeNull();
        okResult.Value.Campus.University.Name.Should().Be("UCR");
    }

    /// <summary>
    /// Tests that <see cref="GetCampusByNameHandler.HandleAsync"/> returns a NotFound result when the campus does not exist.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenCampusDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var campusServiceMock = new Mock<ICampusServices>();
        campusServiceMock
            .Setup(s => s.GetByNameAsync("Unknown Campus"))
            .ReturnsAsync((Campus)null!); // Simula no encontrado

        // Act
        var result = await GetCampusByNameHandler.HandleAsync(campusServiceMock.Object, "Unknown Campus");

        // Assert
        var notFoundResult = Assert.IsType<NotFound<string>>(result.Result);
        notFoundResult.Value.Should().Be("The requested campus was not found.");
    }

    /// <summary>
    /// Tests that <see cref="GetCampusByNameHandler.HandleAsync"/> returns NotFound when the campus name is null from the route.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenCampusNameIsNull_ShouldReturnNotFound()
    {
        var mock = new Mock<ICampusServices>();

        var result = await GetCampusByNameHandler.HandleAsync(mock.Object, null!);

        var notFound = Assert.IsType<NotFound<string>>(result.Result);
        notFound.Value.Should().Be("The requested campus was not found."); 
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
