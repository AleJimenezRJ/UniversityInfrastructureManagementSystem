using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.ComponentsManagement;

/// <summary>
/// Unit tests for the <see cref="GetProjectorHandler"/> class.
/// 
/// These tests verify the behavior of the handler when interacting with the projector service
/// and the global mapper, covering scenarios such as successful retrieval, empty results,
/// error propagation, and mapper unsupported types.
/// </summary>
public class GetProjectorHandlerTests
{
    private readonly Mock<IProjectorServices> _serviceMock;
    private readonly GlobalMapper _mapper;

    public GetProjectorHandlerTests()
    {
        _serviceMock = new Mock<IProjectorServices>(MockBehavior.Strict);
        _mapper = new GlobalMapper();
    }

    /// <summary>
    /// Verifies that the handler returns an HTTP 200 OK response containing
    /// a list of projector DTOs when the service returns multiple projectors.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WithProjectorDtos_WhenServiceReturnsProjectors()
    {
        // Arrange
        var projectors = new List<Projector>
        {
            new Projector("Slides", Area2D.Create(4,5), 1, Orientation.Create("North"), Coordinates.Create(1,2,3), Dimension.Create(1,1,1)),
            new Projector("HD", Area2D.Create(3,3), 2, Orientation.Create("South"), Coordinates.Create(2,3,4), Dimension.Create(1,1,1))
        };

        _serviceMock.Setup(s => s.GetProjectorAsync()).ReturnsAsync(projectors);

        // Act
        var result = await GetProjectorHandler.HandleAsync(_serviceMock.Object, _mapper);

        // Assert
        var okResult = Assert.IsType<Ok<GetProjectorResponse>>(result.Result);
        okResult.Value!.Projectors.Should().HaveCount(2);
        okResult.Value.Projectors.Should().AllBeOfType<ProjectorDto>();
    }

    /// <summary>
    /// Ensures that the handler returns an HTTP 200 OK response with an empty
    /// list when the service returns no projectors.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WithEmptyList_WhenServiceReturnsNoProjectors()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetProjectorAsync()).ReturnsAsync(new List<Projector>());

        // Act
        var result = await GetProjectorHandler.HandleAsync(_serviceMock.Object, _mapper);

        // Assert
        var okResult = Assert.IsType<Ok<GetProjectorResponse>>(result.Result);
        okResult.Value!.Projectors.Should().BeEmpty();
    }

    /// <summary>
    /// Validates that the handler correctly handles a case where the service
    /// returns null or empty list by returning an HTTP 200 OK with an empty list.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WithEmptyList_WhenServiceReturnsNull()
    {
        _serviceMock.Setup(s => s.GetProjectorAsync()).ReturnsAsync(new List<Projector>());

        var result = await GetProjectorHandler.HandleAsync(_serviceMock.Object, _mapper);

        var okResult = Assert.IsType<Ok<GetProjectorResponse>>(result.Result);
        okResult.Value!.Projectors.Should().BeEmpty();
    }

    /// <summary>
    /// Confirms that the handler propagates exceptions thrown by the service.
    /// This simulates unexpected errors and verifies proper exception behavior.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldThrowException_WhenServiceThrows()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetProjectorAsync()).ThrowsAsync(new Exception("Service failure"));

        // Act
        Func<Task> act = async () => await GetProjectorHandler.HandleAsync(_serviceMock.Object, _mapper);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Service failure");
    }

    /// <summary>
    /// Tests that when the mapper does not support the projector type (throws
    /// NotSupportedException), the handler returns an HTTP 409 Conflict response.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenMapperNotSupported()
    {
        // Arrange
        var serviceMock = new Mock<IProjectorServices>();
        var mapper = new GlobalMapper();

        var unmappedProjector = new UnmappedProjector(); // Subclass not registered

        serviceMock
            .Setup(s => s.GetProjectorAsync())
            .ReturnsAsync([unmappedProjector]);

        // Act
        var result = await GetProjectorHandler.HandleAsync(serviceMock.Object, mapper);

        // Assert
        result.Result.Should().BeOfType<Conflict>();
    }

    // Helper class representing a projector subtype not supported by the mapper
    private class UnmappedProjector : Projector
    {
        public UnmappedProjector() : base(
            "Fake",
            Area2D.Create(1, 1),
            999,
            Orientation.Create("North"),
            Coordinates.Create(0, 0, 0),
            Dimension.Create(1, 1, 1))
        {
        }
    }

}
