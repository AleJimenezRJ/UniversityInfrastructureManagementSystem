using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="GetAreaHandler"/>, which handles HTTP GET requests
/// </summary>
public class GetAreaHandlerTests
{
    /// <summary>
    /// Mock of the <see cref="IAreaServices"/> to simulate the service layer for testing.
    /// </summary>
    private readonly Mock<IAreaServices> _areaServiceMock;
    /// <summary>
    /// Initializes the mock service used to simulate <see cref="IAreaServices"/> behavior.
    /// </summary>
    public GetAreaHandlerTests()
    {
        _areaServiceMock = new Mock<IAreaServices>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that <see cref="GetAreaHandler.HandleAsync"/> returns a successful
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenCalled_ShouldReturnOkWithListOfAreas()
    {
        // Arrange
        var areas = new List<Area> { TestArea() };

        _areaServiceMock.Setup(s => s.ListAreaAsync())
            .ReturnsAsync(areas);

        // Act
        var result = await GetAreaHandler.HandleAsync(_areaServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetAreaResponse>>(result.Result);
        okResult.Value.Area.Should().HaveCount(1);
        okResult.Value.Area[0].Name.Should().Be("F1");
        okResult.Value.Area[0].Campus.Name.Should().Be("RF");
    }

    /// <summary>
    /// Tests that <see cref="GetAreaHandler.HandleAsync"/> returns a <see cref="ProblemHttpResult"/>
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenExceptionThrown_ShouldReturnProblemResult()
    {
        // Arrange
        _areaServiceMock.Setup(s => s.ListAreaAsync())
            .ThrowsAsync(new Exception("Database unavailable"));

        // Act
        var result = await GetAreaHandler.HandleAsync(_areaServiceMock.Object);

        // Assert
        var problemResult = Assert.IsType<ProblemHttpResult>(result.Result);
        problemResult.ProblemDetails.Detail.Should().Contain("Database unavailable");
    }

    /// <summary>
    /// Tests that <see cref="GetAreaHandler.HandleAsync"/> returns an empty list when no areas exist.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenNoAreas_ShouldReturnEmptyList()
    {
        // Arrange
        _areaServiceMock.Setup(s => s.ListAreaAsync())
            .ReturnsAsync(new List<Area>());

        // Act
        var result = await GetAreaHandler.HandleAsync(_areaServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetAreaResponse>>(result.Result);
        okResult?.Value.Area.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that <see cref="GetAreaHandler.HandleAsync"/> correctly maps multiple areas to the response.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenMultipleAreas_ShouldReturnAllMappedCorrectly()
    {
        // Arrange
        var area1 = TestArea("F1", "RF");
        var area2 = TestArea("F2", "RC");

        _areaServiceMock.Setup(s => s.ListAreaAsync())
            .ReturnsAsync(new List<Area> { area1, area2 });

        // Act
        var result = await GetAreaHandler.HandleAsync(_areaServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetAreaResponse>>(result.Result);
        okResult.Value.Area.Should().HaveCount(2);
        okResult.Value.Area.Select(a => a.Name).Should().Contain(new[] { "F1", "F2" });
        okResult.Value.Area.Select(a => a.Campus.Name).Should().Contain(new[] { "RF", "RC" });
    }
    /// <summary>
    /// Tests that <see cref="GetAreaHandler.HandleAsync"/> correctly maps fields in the DTO.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldMapFieldsCorrectlyInDto()
    {
        // Arrange
        var area = TestArea("Zona Norte", "Principal");
        _areaServiceMock.Setup(s => s.ListAreaAsync()).ReturnsAsync(new List<Area> { area });

        // Act
        var result = await GetAreaHandler.HandleAsync(_areaServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetAreaResponse>>(result.Result);
        var dto = okResult.Value.Area.First();

        dto.Name.Should().Be("Zona Norte");
        dto.Campus.Should().NotBeNull();
        dto.Campus.Name.Should().Be("Principal");
    }

    /// <summary>
    /// Tests that <see cref="GetAreaHandler.HandleAsync"/> returns multiple areas with different names and campuses.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnMultipleAreas()
    {
        // Arrange
        var area1 = TestArea("Zona A", "Campus Central");
        var area2 = TestArea("Zona B", "Campus Norte");

        _areaServiceMock.Setup(s => s.ListAreaAsync())
            .ReturnsAsync(new List<Area> { area1, area2 });

        // Act
        var result = await GetAreaHandler.HandleAsync(_areaServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetAreaResponse>>(result.Result);
        okResult.Value.Area.Should().HaveCount(2);
        okResult.Value.Area.Select(a => a.Name).Should().Contain(new[] { "Zona A", "Zona B" });
        okResult.Value.Area.Select(a => a.Campus.Name).Should().Contain(new[] { "Campus Central", "Campus Norte" });
    }
    /// <summary>
    /// Helper static method to create a standardized <see cref="Area"/> object for testing.
    /// </summary>
    /// <param name="areaName"></param>
    /// <param name="campusName"></param>
    /// <returns></returns>
    private static Area TestArea(string areaName = "F1", string campusName = "RF")
    {
        var university = new University(
            new EntityName("UCR"),
            new EntityLocation("Costa Rica")
        );

        var campus = new Campus(
            new EntityName(campusName),
            new EntityLocation("Locate"),
            university
        );

        return new Area(
            new EntityName(areaName),
            campus
        );
    }


}
