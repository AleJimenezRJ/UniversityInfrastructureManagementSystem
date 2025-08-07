using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.Spaces;

public class DeleteFloorHandlerTests
{
    /// <summary>
    /// Mock for the floor services used in the tests.
    /// </summary>
    private readonly Mock<IFloorServices> _floorServicesMock;

    /// <summary>
    /// A valid floor ID that will be used in tests to simulate a successful deletion.
    /// </summary>
    private int _validFloorId;

    /// <summary>
    /// An invalid floor ID (e.g., negative value) that will be used in tests to simulate an error scenario.
    /// </summary>
    private int _negativeFloorId;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteFloorHandlerTests"/> class.
    /// </summary>
    public DeleteFloorHandlerTests()
    {
        // Initialize the mock for the floor services
        _floorServicesMock = new Mock<IFloorServices>();
        // Set a valid floor ID for testing
        _validFloorId = 1;
        // Set an invalid floor ID for testing
        _negativeFloorId = -1;
    }

    /// <summary>
    /// Tests when a valid and existing floor ID is provided, the handler should return NoContent if the deletion is successful.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenFloorIdIsValid_ShouldReturnNoContent()
    {
        // Arrange
        _floorServicesMock.Setup(f => f.DeleteFloorAsync(_validFloorId))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteFloorHandler.HandleAsync(_floorServicesMock.Object, _validFloorId);

        // Assert
        result.Result.Should().BeOfType<NoContent>(because: "the floor deletion should return NoContent when successful");
    }

    /// <summary>
    /// Tests when a valid floor ID is provided but the floor does not exist, the handler should return NotFound.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenFloorDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var errorMessage = "FloorId not found";
        _floorServicesMock.Setup(f => f.DeleteFloorAsync(_validFloorId))
            .ThrowsAsync(new NotFoundException(errorMessage));

        // Act
        var result = await DeleteFloorHandler.HandleAsync(_floorServicesMock.Object, _validFloorId);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>(because: "the handler should return NotFound when the floor does not exist");
    }

    /// <summary>
    /// Tests when a valid floor ID is provided but the floor does not exist, the handler should forward the error message.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenFloorDoesNotExits_ForwardsTheNotFoundErrorMessage()
    {
        // Arrange
        var errorMessage = "FloorId not found";
        _floorServicesMock.Setup(f => f.DeleteFloorAsync(_validFloorId))
            .ThrowsAsync(new NotFoundException(errorMessage));

        // Act
        var result = await DeleteFloorHandler.HandleAsync(_floorServicesMock.Object, _validFloorId);

        // Assert
        var notFoundResult = result.Result.As<NotFound<string>>();
        notFoundResult.Value.Should().Be(errorMessage, because: "the handler should forward the error message when the floor does not exist");
    }

    /// <summary>
    /// Tests when a concurrency conflict occurs while deleting a floor, the handler should return Conflict.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenConcurrencyConflictOccurs_ShouldReturnConflict()
    {
        // Arrange
        var errorMessage = "Concurrency conflict deleting the floor";
        _floorServicesMock.Setup(f => f.DeleteFloorAsync(_validFloorId))
            .ThrowsAsync(new ConcurrencyConflictException(errorMessage));

        // Act
        var result = await DeleteFloorHandler.HandleAsync(_floorServicesMock.Object, _validFloorId);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>(because: "a concurrency conflict should return Conflict");
    }

    /// <summary>
    /// Tests when a concurrency conflict occurs while deleting a floor, the handler should forward the error message.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenConcurrencyConflictOccurs_ForwardsTheConflictErrorMessage()
    {
        // Arrange
        var errorMessage = "Concurrency conflict deleting the floor";
        _floorServicesMock.Setup(f => f.DeleteFloorAsync(_validFloorId))
            .ThrowsAsync(new ConcurrencyConflictException(errorMessage));

        // Act
        var result = await DeleteFloorHandler.HandleAsync(_floorServicesMock.Object, _validFloorId);

        // Assert
        var conflictResult = result.Result.As<Conflict<string>>();
        conflictResult.Value.Should().Be(errorMessage, because: "the handler should forward the concurrency error message");
    }

    /// <summary>
    /// Tests when an invalid floor ID is provided (e.g., negative value), the handler should return BadRequest.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenFloorIdIsNegative_ShouldReturnBadRequest()
    {
        // Act
        var result = await DeleteFloorHandler.HandleAsync(_floorServicesMock.Object, _negativeFloorId);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>(because: "the handler should return BadRequest for negative floor IDs");
    }

    /// <summary>
    /// Tests when an invalid floor ID is provided (e.g., negative value), the handler should return BadRequest with a single validation error.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenFloorIdIsInvalid_ShouldReturnBadRequestWithASingleValidationError()
    {
        // Act
        var result = await DeleteFloorHandler.HandleAsync(_floorServicesMock.Object, _negativeFloorId);

        // Assert
        var badRequestResult = result.Result.As<BadRequest<List<ValidationError>>>();
        badRequestResult.Value.Should().ContainSingle(because: "the handler should return a single validation error for invalid floor ID");
    }

    /// <summary>
    /// Tests when an invalid floor ID is provided (e.g., negative value), the handler should return BadRequest with a validation error containing the correct parameter name.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenFloorIdIsInvalid_ShouldReturnValidationErrorWithCorrectParameterName()
    {
        // Act
        var result = await DeleteFloorHandler.HandleAsync(_floorServicesMock.Object, _negativeFloorId);

        // Assert: que el resultado sea BadRequest
        var badRequestResult = result.Result.As<BadRequest<List<ValidationError>>>();

        var validationError = badRequestResult.Value![0];
        validationError.Parameter.Should().Be("FloorId", because: "the handler should return a validation error with the correct parameter name for invalid floor IDs");
    }

    /// <summary>
    /// Tests when an invalid floor ID is provided (e.g., negative value), the handler should return BadRequest with a specific error message.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenFloorIdIsInvalid_ShouldReturnValidationErrorWithCorrectParameterMessage()
    {
        // Act
        var result = await DeleteFloorHandler.HandleAsync(_floorServicesMock.Object, _negativeFloorId);

        // Assert: que el resultado sea BadRequest
        var badRequestResult = result.Result.As<BadRequest<List<ValidationError>>>();

        var validationError = badRequestResult.Value![0];
        validationError.Message.Should().Be("Invalid floor id format.", because: "the handler should return a specific error message for invalid floor IDs");
    }
}
