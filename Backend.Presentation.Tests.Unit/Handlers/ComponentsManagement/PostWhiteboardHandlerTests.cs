using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.ComponentsManagement;


/// <summary>
/// Unit tests for the <see cref="PostWhiteboardHandler"/> class, which handles the creation of whiteboards.
/// </summary>
public class PostWhiteboardHandlerTests
{
    /// <summary>
    /// Mock for the <see cref="IWhiteboardServices"/> interface used to simulate service behavior in unit tests.
    /// </summary>
    private readonly Mock<IWhiteboardServices> _serviceMock;

    /// <summary>
    /// A valid <see cref="WhiteboardNoIdDto"/> instance used for positive test scenarios.
    /// </summary>
    private readonly WhiteboardNoIdDto _validDto;

    /// <summary>
    /// An invalid <see cref="WhiteboardNoIdDto"/> instance used for negative test scenarios.
    /// </summary>
    private readonly WhiteboardNoIdDto _invalidDto;

    /// <summary>
    /// An invalid <see cref="WhiteboardNoIdDto"/> instance used for negative test scenarios.
    /// </summary>
    private readonly WhiteboardNoIdDto _multipleErrorsDto;

    /// <summary>
    /// Represents an array of expected field names that may trigger validation errors during
    /// processing in the context of whiteboard handling operations.
    /// </summary>
    private static readonly string[] Expected = ["Orientation", "Color", "Dimensions"];

    /// <summary>
    /// Represents a read-only instance of the <see cref="GlobalMapper"/> class.
    /// </summary>
    private readonly GlobalMapper _globalMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostWhiteboardHandlerTests"/> class.
    /// Sets up valid and invalid DTOs and a strict mock for <see cref="IWhiteboardServices"/>.
    /// </summary>
    public PostWhiteboardHandlerTests()
    {
        _serviceMock = new Mock<IWhiteboardServices>(MockBehavior.Strict);

        _globalMapper = new GlobalMapper();

        _validDto = new WhiteboardNoIdDto(
            Orientation: "North",
            Position: new PositionDto(1, 2, 3),
            Dimensions: new DimensionsDto(2.0, 1.0, 0.5),
            MarkerColor: "Red"
        );

        _invalidDto = new WhiteboardNoIdDto(
            Orientation: "North",
            Position: new PositionDto(1, 2, 3),
            Dimensions: new DimensionsDto(2.0, 1.0, 0.5),
            MarkerColor: "not-a-color"
        );

        _multipleErrorsDto = new WhiteboardNoIdDto(
            Orientation: "invalid-orientation",
            Position: new PositionDto(-1000, -2000, -3000),
            Dimensions: new DimensionsDto(0, 0, 0),
            MarkerColor: "invalid-color"
        );
    }

    /// <summary>
    /// Tests that <see cref="PostWhiteboardHandler.HandleAsync"/> returns a BadRequest when the DTO is invalid.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenDtoIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new PostWhiteboardRequest(_invalidDto);

        // Act
        var result = await PostWhiteboardHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1);

        // Assert
        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().ContainSingle(v =>
            v.Parameter == "Color" &&
            v.Message.StartsWith("Invalid color", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that <see cref="PostWhiteboardHandler.HandleAsync"/> returns a Conflict when the service fails to add the whiteboard.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenAdditionFails_ShouldReturnConflict()
    {
        // Arrange
        var request = new PostWhiteboardRequest(_validDto);
        _serviceMock.Setup(s =>
            s.AddWhiteboardAsync(It.IsAny<int>(), It.IsAny<Whiteboard>())
        ).ReturnsAsync(false);

        // Act
        var result = await PostWhiteboardHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 42);

        // Assert
        result.Result.Should().BeOfType<Conflict>();
    }

    /// <summary>
    /// Tests that <see cref="PostWhiteboardHandler.HandleAsync"/> returns Ok when the service successfully adds the whiteboard.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenServiceSucceeds_ShouldReturnOk()
    {
        // Arrange
        var request = new PostWhiteboardRequest(_validDto);
        _serviceMock
            .Setup(s => s.AddWhiteboardAsync(It.IsAny<int>(), It.IsAny<Whiteboard>()))
            .ReturnsAsync(true);

        // Act
        var result = await PostWhiteboardHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1);

        // Assert
        result.Result.Should().BeOfType<Ok<PostWhiteboardResponse>>();
    }

    /// <summary>
    /// Tests that <see cref="PostWhiteboardHandler.HandleAsync"/> returns the expected DTO when a whiteboard is added.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenWhiteboardIsAdded_ShouldReturnExpectedDto()
    {
        // Arrange
        var request = new PostWhiteboardRequest(_validDto);
        _serviceMock
            .Setup(s => s.AddWhiteboardAsync(It.IsAny<int>(), It.IsAny<Whiteboard>()))
            .ReturnsAsync(true);

        // Act
        var result = await PostWhiteboardHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1);
        var ok = result.Result.As<Ok<PostWhiteboardResponse>>();

        // Assert
        ok.Value!.Whiteboard.Should().BeEquivalentTo(_validDto);
    }

    [Fact]
    public async Task HandleAsync_WhenMultipleFieldsAreInvalid_ShouldReturnMultipleValidationErrors()
    {
        // Arrange
        var request = new PostWhiteboardRequest(_multipleErrorsDto);

        // Act
        var result = await PostWhiteboardHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1);

        // Assert
        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);

        badRequest.Value.Should().HaveCount(3);
        badRequest.Value.Select(e => e.Parameter)
            .Should().Contain(Expected);
    }


}
