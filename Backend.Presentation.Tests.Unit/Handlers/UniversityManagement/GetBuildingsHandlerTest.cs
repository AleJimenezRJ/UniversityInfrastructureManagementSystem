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
/// Unit tests for <see cref="GetBuildingHandler"/>, which handles HTTP GET requests
/// for retrieving all buildings in the system.
/// </summary>
public class GetBuildingHandlerTests
{
    private readonly Mock<IBuildingsServices> _buildingServiceMock;

    /// <summary>
    /// Initializes the mock service used to simulate <see cref="IBuildingsServices"/> behavior.
    /// </summary>
    public GetBuildingHandlerTests()
    {
        _buildingServiceMock = new Mock<IBuildingsServices>(MockBehavior.Strict);
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingHandler.HandleAsync"/> returns a successful
    /// <see cref="Ok{T}"/> result containing a list of building DTOs when the service call succeeds.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenCalled_ShouldReturnOkWithListOfBuildings()
    {
        // Arrange
        var buildings = new List<Building>
        {
            TestBuilding()
        };

        _buildingServiceMock.Setup(s => s.ListBuildingAsync())
            .ReturnsAsync(buildings);

        // Act
        var result = await GetBuildingHandler.HandleAsync(_buildingServiceMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<GetBuildingResponse>>(result.Result);
        okResult.Value!.Buildings.Should().HaveCount(1);
        okResult.Value.Buildings[0].Name.Should().Be("Engineering");
    }

    /// <summary>
    /// Tests that <see cref="GetBuildingHandler.HandleAsync"/> returns a <see cref="ProblemHttpResult"/>
    /// when the underlying service throws an exception, simulating a failure such as a database error.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenExceptionThrown_ShouldReturnProblemResult()
    {
        // Arrange
        _buildingServiceMock.Setup(s => s.ListBuildingAsync())
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await GetBuildingHandler.HandleAsync(_buildingServiceMock.Object);

        // Assert
        var problemResult = Assert.IsType<ProblemHttpResult>(result.Result);
        problemResult.ProblemDetails.Detail.Should().Contain("Database connection failed");
    }

    /// <summary>
    /// Creates a fully initialized <see cref="Building"/> instance to use in test scenarios.
    /// This method simulates a valid entity with nested structures such as Area, Campus, and University.
    /// </summary>
    /// <returns>A test <see cref="Building"/> instance.</returns>
    private static Building TestBuilding()
    {
        var name = new EntityName("Engineering");
        var coord = new Coordinates(1.1, 2.2, 3.3);
        var dim = new Dimension(10, 20, 30);
        var color = new Colors("Blue");

        var university = new University(
            new EntityName("UCR"),
            new EntityLocation("Costa Rica")
        );

        var campus = new Campus(
            new EntityName("RF"),
            new EntityLocation("Locate"),
            university
        );

        var area = new Area(
            new EntityName("F1"),
            campus
        );

        return new Building(1, name, coord, dim, color, area);
    }
}
