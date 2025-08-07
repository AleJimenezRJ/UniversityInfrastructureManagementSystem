using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.ComponentsManagement;

/// <summary>
/// Contains unit tests for the <see cref="GetSingleLearningComponentByIdHandler"/> class.
/// Verifies correct responses for valid and invalid learning component IDs, as well as correct DTO mapping
/// and error handling for unknown component types.
/// </summary>
public class GetSingleLearningComponentByIdHandlerTests
{
    private readonly Mock<ILearningComponentServices> _serviceMock;

    private readonly Projector _validProjector;

    private readonly Whiteboard _validWhiteboard;

    private readonly UnknownComponent _invalidComponent;

    private readonly GlobalMapper _globalMapper;

    /// <summary>
    /// Initializes test data and mocks for the handler tests.
    /// </summary>
    public GetSingleLearningComponentByIdHandlerTests()
    {
        _serviceMock = new Mock<ILearningComponentServices>(MockBehavior.Strict);

        _globalMapper = new GlobalMapper();

        _validProjector = new Projector(
            "Slides",
            Area2D.Create(4, 5),
            1,
            Orientation.Create("North"),
            Coordinates.Create(1, 2, 3),
            Dimension.Create(1, 1, 1)
        );

        _validWhiteboard = new Whiteboard(
            Colors.Create("Red"),
            2,
            Orientation.Create("East"),
            Coordinates.Create(4, 5, 6),
            Dimension.Create(1, 1, 1)
        );

        _invalidComponent = new UnknownComponent(
            id: 999,
            orientation: Orientation.Create("North"),
            dimensions: Dimension.Create(1, 1, 1),
            position: Coordinates.Create(0, 0, 0)
        );
    }

    /// <summary>
    /// Verifies that the handler returns an Ok result when a valid Projector is returned by the service.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenValidProjectorReturned()
    {
        _serviceMock.Setup(s => s.GetSingleLearningComponentByIdAsync(1))
            .ReturnsAsync(_validProjector);

        var result = await GetSingleLearningComponentByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, 1);

        result.Result.Should().BeOfType<Ok<GetSingleLearningComponentByIdResponse>>();
    }

    /// <summary>
    /// Verifies that the handler returns an Ok result when a valid Whiteboard is returned by the service.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenValidWhiteboardReturned()
    {
        _serviceMock.Setup(s => s.GetSingleLearningComponentByIdAsync(2))
            .ReturnsAsync(_validWhiteboard);

        var result = await GetSingleLearningComponentByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, 2);

        result.Result.Should().BeOfType<Ok<GetSingleLearningComponentByIdResponse>>();
    }

    /// <summary>
    /// Verifies that the handler maps a Projector entity to the correct DTO type.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnCorrectDtoType_ForProjector()
    {
        _serviceMock.Setup(s => s.GetSingleLearningComponentByIdAsync(1))
            .ReturnsAsync(_validProjector);

        var result = await GetSingleLearningComponentByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, 1);
        var response = result.Result.As<Ok<GetSingleLearningComponentByIdResponse>>().Value;

        response!.LearningComponent.Should().BeOfType<ProjectorDto>();
    }

    /// <summary>
    /// Verifies that the handler maps a Whiteboard entity to the correct DTO type.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnCorrectDtoType_ForWhiteboard()
    {
        _serviceMock.Setup(s => s.GetSingleLearningComponentByIdAsync(2))
            .ReturnsAsync(_validWhiteboard);

        var result = await GetSingleLearningComponentByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, 2);
        var response = result.Result.As<Ok<GetSingleLearningComponentByIdResponse>>().Value;

        response!.LearningComponent.Should().BeOfType<WhiteboardDto>();
    }

    /// <summary>
    /// Verifies that the handler returns a BadRequest result for an invalid component ID.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_ForInvalidId()
    {
        int invalidId = -5;

        var result = await GetSingleLearningComponentByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, invalidId);

        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        Assert.Single(badRequest.Value!);
        Assert.Equal("Invalid learning component id format.", badRequest.Value![0].Message);
    }

    /// <summary>
    /// Verifies that the handler returns a BadRequest when an unknown component type is returned
    /// and no mapper is registered.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenMapperThrowsNotSupportedException()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.GetSingleLearningComponentByIdAsync(999))
            .ReturnsAsync(_invalidComponent); // componente sin mapper registrado

        // Act
        var result = await GetSingleLearningComponentByIdHandler
            .HandleAsync(_serviceMock.Object, _globalMapper, 999);

        // Assert
        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().ContainSingle(e =>
            e.Parameter == "Mapper" &&
            e.Message.Contains("No ID-based mapper registered"));
    }




    /// <summary>
    /// Represents an unknown learning component type used for testing error handling in the handler.
    /// </summary>
    private class UnknownComponent : LearningComponent
    {
        public UnknownComponent(int id, Orientation orientation, Dimension dimensions, Coordinates position)
            : base(id, orientation, dimensions, position) { }
    }

}
