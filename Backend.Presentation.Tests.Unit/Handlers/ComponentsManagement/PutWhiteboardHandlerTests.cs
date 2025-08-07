using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.ComponentsManagement;

/// <summary>
/// Unit tests for the <see cref="PutWhiteboardHandler"/> class, which handles updating whiteboards.
/// </summary>
public class PutWhiteboardHandlerTests
{
    /// <summary>
    /// A mock instance of the <see cref="IWhiteboardServices"/> interface utilized for unit testing
    /// of the <see cref="PutWhiteboardHandler"/> class. Configured with <see cref="MockBehavior.Strict"/>
    /// to enforce strict verification of all interactions with the mocked service.
    /// </summary>
    private readonly Mock<IWhiteboardServices> _serviceMock;

    /// <summary>
    /// Represents a valid instance of the <see cref="WhiteboardNoIdDto"/> utilized for testing
    /// valid input scenarios in the <see cref="PutWhiteboardHandlerTests"/> class. Configured
    /// with valid data for orientation, position, dimensions, and marker color.
    /// </summary>
    private readonly WhiteboardNoIdDto _validDto;

    /// <summary>
    /// Represents an instance of the <see cref="WhiteboardNoIdDto"/> class
    /// with intentionally invalid properties for testing purposes.
    /// This facilitates validating the application's behavior and error-handling mechanisms
    /// when encountering invalid "Orientation", "Dimensions", or "MarkerColor" values.
    /// </summary>
    private readonly WhiteboardNoIdDto _invalidDto;

    /// <summary>
    /// An instance of the <see cref="WhiteboardNoIdDto"/> class that represents a whiteboard
    /// with multiple invalid properties. This object is used to test scenarios where validation
    /// errors are expected for several fields, such as "Orientation", "Dimensions", and "MarkerColor".
    /// </summary>
    private readonly WhiteboardNoIdDto _multipleErrorsDto;

    /// <summary>
    /// Represents a read-only instance of the <see cref="GlobalMapper"/> class.
    /// </summary>
    private readonly GlobalMapper _globalMapper;

    /// <summary>
    /// An array of string values representing the expected validation error parameters
    /// for whiteboard fields. These expected parameters include properties that are
    /// commonly validated, such as "Orientation", "Dimensions", and "Color".
    /// </summary>
    private static readonly string[] Expected = ["Orientation", "Dimensions", "Color"];

    /// <summary>
    /// Initializes a new instance of the <see cref="PutWhiteboardHandlerTests"/> class,
    /// setting up mock services and test DTOs.
    /// </summary>
    public PutWhiteboardHandlerTests()
    {
        _serviceMock = new Mock<IWhiteboardServices>(MockBehavior.Strict);

        _globalMapper = new GlobalMapper();

        _validDto = new WhiteboardNoIdDto(
            Orientation: "North",
            Position: new PositionDto(1, 2, 3),
            Dimensions: new DimensionsDto(2.0, 1.0, 0.5),
            MarkerColor: "Blue"
        );

        _invalidDto = new WhiteboardNoIdDto(
            Orientation: "invalid-orientation",
            Position: new PositionDto(1, 2, 3),
            Dimensions: new DimensionsDto(0.0, -1.0, 0.0),
            MarkerColor: "not-a-color"
        );

        _multipleErrorsDto = new WhiteboardNoIdDto(
            Orientation: "invalid-orientation",
            Position: new PositionDto(100, 200, 300),
            Dimensions: new DimensionsDto(0, 0, 0),
            MarkerColor: "invalid-color"
        );
    }

    /// <summary>
    /// Tests that a bad request is returned when the DTO is invalid.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenDtoIsInvalid_ShouldReturnBadRequest()
    {
        var request = new PutWhiteboardRequest(_invalidDto);

        var result = await PutWhiteboardHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1, 1);

        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().Contain(v =>
            v.Parameter == "Color" &&
            v.Message.StartsWith("Invalid color", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that a conflict result is returned when the update fails.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenUpdateFails_ShouldReturnConflict()
    {
        var request = new PutWhiteboardRequest(_validDto);
        _serviceMock.Setup(s =>
            s.UpdateWhiteboardAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Whiteboard>())
        ).ReturnsAsync(false);

        var result = await PutWhiteboardHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1, 2);

        result.Result.Should().BeOfType<Conflict>();
    }

    /// <summary>
    /// Tests that an OK result is returned when the service update succeeds.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenServiceSucceeds_ShouldReturnOk()
    {
        var request = new PutWhiteboardRequest(_validDto);
        _serviceMock.Setup(s =>
            s.UpdateWhiteboardAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Whiteboard>())
        ).ReturnsAsync(true);

        var result = await PutWhiteboardHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1, 2);

        result.Result.Should().BeOfType<Ok<PutWhiteboardResponse>>();
    }

    /// <summary>
    /// Tests that the returned DTO matches the expected values when the whiteboard is updated.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenWhiteboardIsUpdated_ShouldReturnExpectedDto()
    {
        var request = new PutWhiteboardRequest(_validDto);
        _serviceMock.Setup(s =>
            s.UpdateWhiteboardAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Whiteboard>())
        ).ReturnsAsync(true);

        var result = await PutWhiteboardHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1, 2);
        var ok = result.Result.As<Ok<PutWhiteboardResponse>>();

        ok.Value!.Whiteboard.Should().BeEquivalentTo(_validDto);
    }

    /// <summary>
    /// Tests that multiple validation errors are returned when multiple fields are invalid.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenMultipleFieldsAreInvalid_ShouldReturnMultipleValidationErrors()
    {
        var request = new PutWhiteboardRequest(_multipleErrorsDto);

        var result = await PutWhiteboardHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1, 1);

        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().HaveCountGreaterThanOrEqualTo(2);
        badRequest.Value.Select(e => e.Parameter)
            .Should().Contain(Expected);
    }
}
