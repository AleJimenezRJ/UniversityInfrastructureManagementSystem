using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.ComponentsManagement;

/// <summary>
/// Contains unit tests for the <see cref="GetLearningComponentHandler"/> class.
/// These tests verify the handler's behavior across various scenarios, including valid component retrieval,
/// invalid pagination parameters, empty results, and error handling for unknown component types during mapping.
/// </summary>
public class GetLearningComponentHandlerTests
{
    private readonly Mock<ILearningComponentServices> _serviceMock;
    private readonly GlobalMapper _mapper;
    private readonly Projector _projector;
    private readonly Whiteboard _whiteboard;

    /// <summary>
    /// Initializes test data and mocks for the handler tests.
    /// </summary>
    public GetLearningComponentHandlerTests()
    {
        _serviceMock = new Mock<ILearningComponentServices>(MockBehavior.Strict);
        _mapper = new GlobalMapper();

        _projector = new Projector("Slides", Area2D.Create(2, 3), 1, Orientation.Create("North"), Coordinates.Create(1, 2, 3), Dimension.Create(1, 1, 1));
        _whiteboard = new Whiteboard(Colors.Create("White"), 2, Orientation.Create("East"), Coordinates.Create(3, 4, 5), Dimension.Create(1, 1, 1));
    }

    /// <summary>
    /// Verifies that the handler returns an <see cref="Ok{TValue}"/> result
    /// with a list of valid learning components when the service provides them.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenComponentsAreValid()
    {
        _serviceMock.Setup(s => s.GetLearningComponentAsync(10, 0)).ReturnsAsync([_projector, _whiteboard]);

        var result = await GetLearningComponentHandler.HandleAsync(_serviceMock.Object, _mapper);

        result.Result.Should().BeOfType<Ok<GetLearningComponentResponse>>();
    }

    /// <summary>
    /// Verifies that the handler returns a <see cref="BadRequest{TValue}"/> result
    /// when an invalid page size is provided.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenPageSizeIsInvalid()
    {
        // Arrange
        int invalidPageSize = 25;
        int validPageIndex = 0;

        _serviceMock
            .Setup(s => s.GetLearningComponentAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<LearningComponent>());

        var mapper = new GlobalMapper();

        // Act
        var result = await GetLearningComponentHandler.HandleAsync(
            _serviceMock.Object,
            mapper,
            invalidPageSize,
            validPageIndex
        );

        // Assert
        var badRequest = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        badRequest.Value!.ErrorMessages.Should().ContainSingle(e => e.Contains("Page size"));
    }

    /// <summary>
    /// Verifies that the handler returns a <see cref="BadRequest{TValue}"/> result
    /// when an invalid page index is provided.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenPageIndexIsInvalid()
    {
        // Arrange
        int validPageSize = 10;
        int invalidPageIndex = -1;

        _serviceMock
            .Setup(s => s.GetLearningComponentAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<LearningComponent>());

        var mapper = new GlobalMapper();

        // Act
        var result = await GetLearningComponentHandler.HandleAsync(
            _serviceMock.Object,
            mapper,
            validPageSize,
            invalidPageIndex
        );

        // Assert
        var badRequest = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        badRequest.Value!.ErrorMessages.Should().ContainSingle(e => e.Contains("Page index"));
    }

    /// <summary>
    /// Verifies that the handler returns a <see cref="BadRequest{TValue}"/> result
    /// when both page size and page index are invalid.
    /// Ensures that both corresponding error messages are present in the response.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenPageSizeAndIndexAreInvalid()
    {
        _serviceMock
            .Setup(s => s.GetLearningComponentAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<LearningComponent>());

        var result = await GetLearningComponentHandler.HandleAsync(
            _serviceMock.Object,
            _mapper,
            0,   // inválido
            -1   // inválido
        );

        var badRequest = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        badRequest.Value!.ErrorMessages.Should().Contain(e => e.Contains("Page size"));
        badRequest.Value!.ErrorMessages.Should().Contain(e => e.Contains("Page index"));
        badRequest.Value!.ErrorMessages.Should().HaveCount(2);
    }

    /// <summary>
    /// Verifies that the handler returns an <see cref="Ok{TValue}"/> result with an empty list
    /// when the service returns no learning components.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenNoComponentsAreReturned()
    {
        _serviceMock.Setup(s => s.GetLearningComponentAsync(10, 0))
            .ReturnsAsync(new List<LearningComponent>());

        var result = await GetLearningComponentHandler.HandleAsync(
            _serviceMock.Object,
            _mapper,
            10,
            0
        );

        var ok = Assert.IsType<Ok<GetLearningComponentResponse>>(result.Result);
        ok.Value!.LearningComponents.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that the handler returns a <see cref="Conflict"/> result
    /// when the <see cref="GlobalMapper"/> fails to map an unknown component type,
    /// typically throwing a <see cref="System.NotSupportedException"/>.
    /// This tests the handler's error handling for mapping failures.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenMapperFailsWithNotSupportedException()
    {
        var unknownComponent = new UnknownComponent(999,
            Orientation.Create("North"),
            Dimension.Create(1, 1, 1),
            Coordinates.Create(0, 0, 0)
        );

        _serviceMock.Setup(s => s.GetLearningComponentAsync(10, 0))
            .ReturnsAsync([unknownComponent]);

        var result = await GetLearningComponentHandler.HandleAsync(_serviceMock.Object, _mapper);

        result.Result.Should().BeOfType<Conflict>();
    }

    /// <summary>
    /// Represents an unknown learning component type used for testing error handling in the handler.
    /// This class is internal to the test file and is not part of the main application domain.
    /// </summary>
    private class UnknownComponent : LearningComponent
    {
        public UnknownComponent(int id, Orientation orientation, Dimension dimensions, Coordinates position)
            : base(id, orientation, dimensions, position) { }
    }

}
