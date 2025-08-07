using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.ComponentsManagement;

/// <summary>
/// Unit tests for the <see cref="GetWhiteboardHandler"/> class.
///
/// These tests validate the behavior of the handler under different service and mapping conditions,
/// ensuring correct HTTP responses and proper DTO mapping for the whiteboard components.
/// </summary>
public class GetWhiteboardHandlerTests
{
    private readonly Mock<IWhiteboardServices> _serviceMock;
    private readonly GlobalMapper _mapper;

    public GetWhiteboardHandlerTests()
    {
        _serviceMock = new Mock<IWhiteboardServices>(MockBehavior.Strict);
        _mapper = new GlobalMapper();
    }

    /// <summary>
    /// Verifies that the handler returns an HTTP 200 OK response with a list of whiteboard DTOs
    /// when the service returns multiple valid whiteboard entities.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WithWhiteboardDtos_WhenServiceReturnsWhiteboards()
    {
        // Arrange
        var whiteboards = new List<Whiteboard>
    {
        new Whiteboard(
            Colors.Create("Red"),
            1,
            Orientation.Create("North"),
            Coordinates.Create(1, 2, 3),
            Dimension.Create(1, 1, 1)),
        new Whiteboard(
            Colors.Create("Blue"),
            2,
            Orientation.Create("South"),
            Coordinates.Create(4, 5, 6),
            Dimension.Create(2, 2, 2))
    };

        _serviceMock.Setup(s => s.GetWhiteboardAsync()).ReturnsAsync(whiteboards);

        // Act
        var result = await GetWhiteboardHandler.HandleAsync(_serviceMock.Object, _mapper);

        var okResult = Assert.IsType<Ok<GetWhiteboardResponse>>(result.Result);

        // Assert response content
        okResult.Value!.Whiteboards.Should().HaveCount(2);
        okResult.Value.Whiteboards.Should().AllBeOfType<WhiteboardDto>();
    }

    /// <summary>
    /// Ensures that the handler returns an HTTP 200 OK response with an empty list
    /// when the service returns no whiteboards.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WithEmptyList_WhenServiceReturnsNoWhiteboards()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetWhiteboardAsync()).ReturnsAsync(new List<Whiteboard>());

        // Act
        var result = await GetWhiteboardHandler.HandleAsync(_serviceMock.Object, _mapper);

        // Assert
        var okResult = Assert.IsType<Ok<GetWhiteboardResponse>>(result.Result);
        okResult.Value!.Whiteboards.Should().BeEmpty();
    }

    /// <summary>
    /// Confirms that an unexpected exception thrown by the service is propagated.
    /// This ensures that the handler does not swallow internal service errors.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldThrowException_WhenServiceThrows()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetWhiteboardAsync()).ThrowsAsync(new Exception("Service failure"));

        // Act
        Func<Task> act = async () => await GetWhiteboardHandler.HandleAsync(_serviceMock.Object, _mapper);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Service failure");
    }

    /// <summary>
    /// Validates that if the mapper throws a NotSupportedException when attempting to map
    /// an unsupported whiteboard type, the handler returns an HTTP 409 Conflict response.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenMapperNotSupported()
    {
        // Arrange
        var serviceMock = new Mock<IWhiteboardServices>();

        var unmappedWhiteboard = new UnmappedWhiteboard();

        serviceMock.Setup(s => s.GetWhiteboardAsync())
            .ReturnsAsync(new List<Whiteboard> { unmappedWhiteboard });

        // Act
        var result = await GetWhiteboardHandler.HandleAsync(serviceMock.Object, _mapper);

        // Assert
        result.Result.Should().BeOfType<Conflict>();
    }

    /// <summary>
    /// Helper class representing a whiteboard subtype not supported by the mapper.
    /// Used to simulate a NotSupportedException during mapping.
    /// </summary>
    private class UnmappedWhiteboard : Whiteboard
    {
        public UnmappedWhiteboard() : base(
            Colors.Create("Red"),
            Orientation.Create("North"),
            Coordinates.Create(0, 0, 0),
            Dimension.Create(1, 1, 1)
        ) 
        {
        }
    }
}
