using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.Spaces;

public class DeleteLearningSpaceHandlerTests
{
    /// <summary>
    /// Mock for the learning space services used in the tests.
    /// </summary>
    private readonly Mock<ILearningSpaceServices> _learningSpaceServicesMock;

    /// <summary>
    /// A valid learning space ID that will be used in tests to simulate a successful deletion.
    /// </summary>
    private int _validLearningSpaceId;

    private int _negativeLearningSpaceId;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLearningSpaceHandlerTests"/> class.
    /// </summary>
    public DeleteLearningSpaceHandlerTests()
    {
        // Initializes a new instance of the <see cref="DeleteLearningSpaceHandlerTests"/> class.
        _learningSpaceServicesMock = new Mock<ILearningSpaceServices>();
        // Sets a valid learning space ID that will be used in tests to simulate a successful deletion.
        _validLearningSpaceId = 1;
        // Sets a negative learning space ID that will be used in tests to simulate an invalid input.
        _negativeLearningSpaceId = -1;
    }

    /// <summary>
    /// Tests when a valid and existing learning space ID is provided, the handler should return NoContent if the deletion is successful.
    /// </summary>
    /// /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenLearningSpaceIdIsValidAndExits_ShouldReturnNoContent()
    {
        // Arrange
        _learningSpaceServicesMock.Setup(l => l.DeleteLearningSpaceAsync(_validLearningSpaceId))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteLearningSpaceHandler.HandleAsync(_learningSpaceServicesMock.Object, _validLearningSpaceId);

        // Assert
        result.Result.Should().BeOfType<NoContent>(because: "the learning space deletion should return NoContent when successful");
    }

    /// <summary>
    /// Tests when a valid learning space ID is provided but the learning space does not exist, the handler should return NotFound.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenLearningSpaceDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var errorMessage = "LearningSpaceId not found";
        _learningSpaceServicesMock.Setup(s => s.DeleteLearningSpaceAsync(_validLearningSpaceId))
            .ThrowsAsync(new NotFoundException(errorMessage));

        // Act
        var result = await DeleteLearningSpaceHandler.HandleAsync(_learningSpaceServicesMock.Object, _validLearningSpaceId);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>(because: "the handler should return NotFound when the learning space does not exist");
    }

    /// <summary>
    /// Tests when a valid learning space ID is provided but the learning space does not exist, the handler should forward the NotFound error message.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenLearningSpaceDoesNotExist_ForwardsTheNotFoundErrorMessage()
    {
        // Arrange
        var errorMessage = "LearningSpaceId not found";
        _learningSpaceServicesMock.Setup(s => s.DeleteLearningSpaceAsync(_validLearningSpaceId))
            .ThrowsAsync(new NotFoundException(errorMessage));

        // Act
        var result = await DeleteLearningSpaceHandler.HandleAsync(_learningSpaceServicesMock.Object, _validLearningSpaceId);

        // Assert
        var notFoundResult = result.Result.As<NotFound<string>>();
        notFoundResult.Value.Should().Be(errorMessage, because: "the handler should forward the error message when the learning space does not exist");
    }

    /// <summary>
    /// Tests when a concurrency conflict occurs while deleting a learning space, the handler should return Conflict.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenConcurrencyConflictOccurs_ShouldReturnConflict()
    {
        // Arrange
        var errorMessage = "Concurrency conflict deleting the learning space";
        _learningSpaceServicesMock.Setup(s => s.DeleteLearningSpaceAsync(_validLearningSpaceId))
                .ThrowsAsync(new ConcurrencyConflictException(errorMessage));

        // Act
        var result = await DeleteLearningSpaceHandler.HandleAsync(_learningSpaceServicesMock.Object, _validLearningSpaceId);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>(because: "a concurrency conflict should return Conflict");
    }

    /// <summary>
    /// Tests when a concurrency conflict occurs while deleting a learning space, the handler should forward the Conflict error message.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenConcurrencyConflictOccurs_ForwardsTheConflictErrorMessage()
    {
        // Arrange
        var errorMessage = "Concurrency conflict deleting the learning space";
        _learningSpaceServicesMock.Setup(s => s.DeleteLearningSpaceAsync(_validLearningSpaceId))
            .ThrowsAsync(new ConcurrencyConflictException(errorMessage));

        // Act
        var result = await DeleteLearningSpaceHandler.HandleAsync(_learningSpaceServicesMock.Object, _validLearningSpaceId);

        // Assert
        var conflictResult = result.Result.As<Conflict<string>>();
        conflictResult.Value.Should().Be(errorMessage, because: "the handler should forward the concurrency error message");
    }

    /// <summary>
    /// Tests when an invalid learning space ID is provided (e.g., negative value), the handler should return BadRequest.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenLearningSpaceIdIsNegative_ShouldReturnBadRequest()
    {
        // Act
        var result = await DeleteLearningSpaceHandler.HandleAsync(_learningSpaceServicesMock.Object, _negativeLearningSpaceId);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>(because: "the handler should return BadRequest for negative learning space IDs");
    }

    /// <summary>
    /// Tests when an invalid learning space ID is provided (e.g., negative value), the handler should return BadRequest with a single validation error.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenLearningSpaceIdIsInvalid_ShouldReturnBadRequestWithASingleValidationError()
    {
        // Act
        var result = await DeleteLearningSpaceHandler.HandleAsync(_learningSpaceServicesMock.Object, _negativeLearningSpaceId);

        // Assert
        var badRequestResult = result.Result.As<BadRequest<List<ValidationError>>>();
        badRequestResult.Value.Should().ContainSingle(because: "the handler should return a single validation error for invalid learning space ID");
    }

    /// <summary>
    /// Tests when an invalid learning space ID is provided (e.g., negative value), the handler should return BadRequest with a validation error containing the correct parameter name.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenLearningSpaceIdIsInvalid_ShouldReturnValidationErrorWithCorrectParameterName()
    {
        // Act
        var result = await DeleteLearningSpaceHandler.HandleAsync(_learningSpaceServicesMock.Object, _negativeLearningSpaceId);

        // Assert
        var badRequestResult = result.Result.As<BadRequest<List<ValidationError>>>();
        var validationError = badRequestResult.Value![0];
        validationError.Parameter.Should().Be("LearningSpaceId", because: "the handler should return a validation error with the correct parameter name for invalid learning space IDs");
    }

    /// <summary>
    /// Tests when an invalid learning space ID is provided (e.g., negative value), the handler should return BadRequest with a specific error message.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenLearningSpaceIdIsInvalid_ShouldReturnValidationErrorWithCorrectParameterMessage()
    {
        // Act
        var result = await DeleteLearningSpaceHandler.HandleAsync(_learningSpaceServicesMock.Object, _negativeLearningSpaceId);

        // Assert
        var badRequestResult = result.Result.As<BadRequest<List<ValidationError>>>();
        var validationError = badRequestResult.Value![0];
        validationError.Message.Should().Be("Invalid floor id format.", because: "the handler should return a specific error message for invalid learning space IDs");
    }
}