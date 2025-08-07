using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="GetUniversityHandler"/>, which handles HTTP GET requests
/// for retrieving all universities in the system.
/// </summary>
public class GetUniversityHandlerTests
{
    private readonly Mock<IUniversityServices> _universityServiceMock;

    private static readonly string[] ExpectedUniversityNames = { "UCR", "TEC" };

    /// <summary>
    /// Initializes the mock service used to simulate <see cref="IUniversityServices"/> behavior.
    /// </summary>
    public GetUniversityHandlerTests()
    {
        _universityServiceMock = new Mock<IUniversityServices>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that <see cref="GetUniversityHandler.HandleAsync"/> returns a <see cref="ProblemHttpResult"/>
    /// when the underlying service throws an exception, simulating a failure such as a database error.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenExceptionThrown_ShouldReturnProblemResult()
    {
        // Arrange
        _universityServiceMock.Setup(s => s.ListUniversityAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await GetUniversityHandler.HandleAsync(_universityServiceMock.Object);

        // Assert
        var problemResult = Assert.IsType<ProblemHttpResult>(result.Result);
        problemResult.ProblemDetails.Detail.Should().Contain("An error occurred while retrieving universities");
        problemResult.ProblemDetails.Detail.Should().Contain("Database error");
    }

    /// <summary>
    /// Tests that <see cref="GetUniversityHandler.HandleAsync(IUniversityServices)"/> returns an
    /// <see cref="Ok{T}"/> result with the list of universities when universities exist.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenUniversitiesExist_ShouldReturnOkResultWithData()
    {
        // Arrange
        List<University> universities = TestUniversity();

        _universityServiceMock.Setup(s => s.ListUniversityAsync())
            .ReturnsAsync(universities);

        // Act
        var result = await GetUniversityHandler.HandleAsync(_universityServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetUniversityResponse>>(result.Result);
        okResult.Value!.University.Should().HaveCount(2);
        okResult.Value.University.Select(u => u.Name).Should().Contain(ExpectedUniversityNames);
    }


    /// <summary>
    /// Tests that <see cref="GetUniversityHandler.HandleAsync(IUniversityServices)"/> returns an
    /// <see cref="Ok{T}"/> result with an empty list when no universities exist.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenNoUniversitiesExist_ShouldReturnOkWithEmptyList()
    {
        // Arrange
        _universityServiceMock.Setup(s => s.ListUniversityAsync())
            .ReturnsAsync(new List<University>());

        // Act
        var result = await GetUniversityHandler.HandleAsync(_universityServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetUniversityResponse>>(result.Result);
        okResult.Value!.University.Should().BeEmpty();
    }

    /// <summary>
    /// Creates a fully initialized <see cref="University"/> instance to use in test scenarios.
    /// </summary>
    /// <returns>A test <see cref="University"/> instance.</returns>
    private static List<University> TestUniversity()
    {
        return new List<University>
        {
            new University(new EntityName("UCR"), new EntityLocation("Costa Rica")),
            new University(new EntityName("TEC"), new EntityLocation("Cartago"))
        };
    }
}
