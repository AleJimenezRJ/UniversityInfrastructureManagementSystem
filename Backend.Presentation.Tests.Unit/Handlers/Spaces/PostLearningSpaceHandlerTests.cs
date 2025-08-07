using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.Spaces;

/// <summary>
/// Provides unit tests for the <see cref="PostLearningSpaceHandler"/> class, specifically testing its behavior when
/// handling requests to create new learning spaces.
/// </summary>
public class PostLearningSpaceHandlerTests
{
    /// <summary>
    /// Represents a mock implementation of the <see cref="ILearningSpaceServices"/> interface.
    /// </summary>
    private readonly Mock<ILearningSpaceServices> _serviceMock;

    /// <summary>
    /// Represents a valid instance of the <see cref="LearningSpaceDto"/> class.
    /// </summary>
    private readonly LearningSpaceDto _validDto;

    /// <summary>
    /// Represents a invalid instance of the <see cref="LearningSpaceDto"/> class.
    /// </summary>
    private readonly LearningSpaceDto _invalidDto;

    /// <summary>
    /// Represents the default valid floor identifier used for validation purposes.
    /// </summary>
    private readonly int _validFloorId = 1;

    /// <summary>
    /// Represents the default invalid floor identifier used for validation purposes.
    /// </summary>
    private readonly int _invalidFloorId = -1;

    /// <summary>
    /// Represents a valid request for posting a learning space.
    /// </summary>
    private readonly PostLearningSpaceRequest _validRequest;

    /// <summary>
    /// Represents a invalid request for posting a learning space.
    /// </summary>
    private readonly PostLearningSpaceRequest _invalidRequest;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostLearningSpaceHandlerTests"/> class.
    /// </summary>
    public PostLearningSpaceHandlerTests()
    {
        _serviceMock = new Mock<ILearningSpaceServices>(MockBehavior.Strict);

        _validDto = new LearningSpaceDto(
            Name: "IF-104",
            Type: "Laboratory",
            MaxCapacity: 30,
            Height: 3.0,
            Width: 5.0,
            Length: 7.0,
            ColorFloor: "White",
            ColorWalls: "Blue",
            ColorCeiling: "White"
        );

        _invalidDto = new LearningSpaceDto(
            Name: "IF-104",
            Type: "Laboratory",
            MaxCapacity: -10, // Invalid capacity
            Height: 3.0,
            Width: 5.0,
            Length: 7.0,
            ColorFloor: "White",
            ColorWalls: "Blue",
            ColorCeiling: "White"
        );

        _validRequest = new PostLearningSpaceRequest(_validDto);

        _invalidRequest = new PostLearningSpaceRequest(_invalidDto);

    }

    /// <summary>
    /// Tests that the <see cref="PostLearningSpaceHandler.HandleAsync"/> method returns an <see cref="Ok{T}"/> result
    /// when provided with a valid request and floor ID.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a result of type
    /// </returns>
    [Fact]
    public async Task HandleAsync_WithValidRequest_ReturnsOk()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.CreateLearningSpaceAsync(_validFloorId, It.IsAny<LearningSpace>()))
            .ReturnsAsync(true);

        // Act
        var result = await PostLearningSpaceHandler.HandleAsync(_serviceMock.Object, _validRequest, _validFloorId);

        // Assert
        result.Result.Should().BeOfType<Ok<PostLearningSpaceResponse>>();
    }

    [Fact]
    public async Task HandleAsync_NullRequest_ReturnsBadRequest()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.CreateLearningSpaceAsync(_validFloorId, It.IsAny<LearningSpace>()))
            .ReturnsAsync(true);

        // Act
        var result = await PostLearningSpaceHandler.HandleAsync(_serviceMock.Object, request: null!, _validFloorId);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();

        var badRequest = result.Result as BadRequest<List<ValidationError>>;

        badRequest!.Value.Should().ContainSingle(e =>
            e.Parameter == "Request" &&
            e.Message == "Request body is required.");
    }

    /// <summary>
    /// Tests the <see cref="PostLearningSpaceHandler.HandleAsync"/> method to ensure that it returns a <see
    /// cref="BadRequest{T}"/> response when provided with an invalid request.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a result of type
    /// </returns>
    [Fact]
    public async Task HandleAsync_WithInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.CreateLearningSpaceAsync(_validFloorId, It.IsAny<LearningSpace>()))
            .ReturnsAsync(true);

        // Act
        var result = await PostLearningSpaceHandler.HandleAsync(_serviceMock.Object, _invalidRequest, _validFloorId);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();

        var badRequest = result.Result as BadRequest<List<ValidationError>>;

        badRequest!.Value.Should().ContainSingle(e =>
            e.Parameter == "MaxCapacity" &&
            e.Message == "Capacidad máxima inválida. Debe ser un número entero positivo.");
    }

    /// <summary>
    /// Tests the <c>HandleAsync</c> method to ensure it returns a <see cref="BadRequest{T}"/> response  when an invalid
    /// floor ID is provided.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a result of type
    /// </returns>
    [Fact]
    public async Task HandleAsync_WithInvalidFloorId_ReturnsBadRequest()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.CreateLearningSpaceAsync(_validFloorId, It.IsAny<LearningSpace>()))
            .ReturnsAsync(true);

        // Act
        var result = await PostLearningSpaceHandler.HandleAsync(_serviceMock.Object, _validRequest, _invalidFloorId);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();

        var badRequest = result.Result as BadRequest<List<ValidationError>>;

        badRequest!.Value.Should().ContainSingle(e =>
            e.Parameter == "FloorId" &&
            e.Message == "Invalid floor Id format.");
    }

    /// <summary>
    /// Tests the <c>HandleAsync</c> method to ensure it returns a <c>BadRequest</c> result when provided with an
    /// invalid floor ID and an invalid request.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a result of type
    /// </returns>
    [Fact]
    public async Task HandleAsync_WithInvalidFloorIdAndInvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.CreateLearningSpaceAsync(_validFloorId, It.IsAny<LearningSpace>()))
            .ReturnsAsync(true);

        // Act
        var result = await PostLearningSpaceHandler.HandleAsync(_serviceMock.Object, _invalidRequest, _invalidFloorId);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();

        var badRequest = result.Result as BadRequest<List<ValidationError>>;

        badRequest!.Value.Should().ContainSingle(e =>
            e.Parameter == "FloorId" &&
            e.Message == "Invalid floor Id format.");
    }

    /// <summary>
    /// Tests the behavior of the <c>HandleAsync</c> method when the service throws a <see cref="NotFoundException"/>.
    /// Verifies that the method returns a <see cref="NotFound{T}"/> result with the appropriate error message.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a result of type
    /// </returns>
    [Fact]
    public async Task HandleAsync_WhenServiceThrowNotFoundException_ReturnsNotFound()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.CreateLearningSpaceAsync(_validFloorId, It.IsAny<LearningSpace>()))
            .ThrowsAsync(new NotFoundException("Floor not found."));

        // Act
        var result = await PostLearningSpaceHandler.HandleAsync(_serviceMock.Object, _validRequest, _validFloorId);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();

        result.Result.As<NotFound<string>>().Value.Should().Be("Floor not found.");
    }

    /// <summary>
    /// Tests that the <see cref="PostLearningSpaceHandler.HandleAsync"/> method returns a <see
    /// cref="ProblemHttpResult"/> when the service throws an unexpected <see cref="ConcurrencyConflictException"/>.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a result of type
    /// </returns>
    [Fact]
    public async Task HandleAsync_WhenServiceThrowsUnexpectedException_ReturnsProblemHttpResult()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.CreateLearningSpaceAsync(_validFloorId, It.IsAny<LearningSpace>()))
            .ThrowsAsync(new ConcurrencyConflictException("Unexpected error."));
        // Act
        var result = await PostLearningSpaceHandler.HandleAsync(_serviceMock.Object, _validRequest, _validFloorId);

        // Assert
        result.Result.Should().BeOfType<ProblemHttpResult>();

        var problem = result.Result as ProblemHttpResult;

        problem!.ProblemDetails.Detail.Should().Be("Unexpected error.");

        problem.ProblemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests that the <see cref="PostLearningSpaceHandler.HandleAsync"/> method returns a <see cref="Conflict{T}"/>
    /// result when the service throws a <see cref="DuplicatedEntityException"/>.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a result of type
    /// </returns>
    [Fact]
    public async Task HandleAsync_WhenServiceThrowsConflictException_ReturnsConflict()
    {
        // Arrange
        _serviceMock
            .Setup(s => s.CreateLearningSpaceAsync(_validFloorId, It.IsAny<LearningSpace>()))
            .ThrowsAsync(new DuplicatedEntityException("Learning space already exists."));
        // Act
        var result = await PostLearningSpaceHandler.HandleAsync(_serviceMock.Object, _validRequest, _validFloorId);
        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
        result.Result.As<Conflict<string>>().Value.Should().Be("Learning space already exists.");
    }
}
