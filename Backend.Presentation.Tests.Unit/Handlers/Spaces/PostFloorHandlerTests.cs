using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.Spaces
{
    /// <summary>
    /// Provides unit tests for the <see cref="PostFloorHandler"/> class, specifically testing its behavior when
    /// handling requests to create new floors.
    /// </summary>
    public class PostFloorHandlerTests
    {
        /// <summary>
        /// Mock implementation of the <see cref="IFloorServices"/> interface.
        /// </summary>
        private readonly Mock<IFloorServices> _serviceMock;

        /// <summary>
        /// Represents a valid building identifier for testing.
        /// </summary>
        private readonly int _validBuildingId = 1;

        /// <summary>
        /// Represents an invalid building identifier for testing.
        /// </summary>
        private readonly int _invalidBuildingId = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostFloorHandlerTests"/> class.
        /// </summary>
        public PostFloorHandlerTests()
        {
            _serviceMock = new Mock<IFloorServices>(MockBehavior.Strict);
        }

        /// <summary>
        /// Tests that <see cref="PostFloorHandler.HandleAsync"/> returns an <see cref="Ok{T}"/> result
        /// when provided with a valid building ID.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a result of type
        /// </returns>
        [Fact]
        public async Task HandleAsync_WithValidBuildingId_ReturnsOk()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.CreateFloorAsync(_validBuildingId))
                .ReturnsAsync(true);

            // Act
            var result = await PostFloorHandler.HandleAsync(_serviceMock.Object, _validBuildingId);

            // Assert
            result.Result.Should().BeOfType<Ok<PostFloorResponse>>();
        }

        /// <summary>
        /// Tests that <see cref="PostFloorHandler.HandleAsync"/> returns a <see cref="BadRequest{T}"/> result
        /// when provided with an invalid building ID.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a result of type
        /// </returns>
        [Fact]
        public async Task HandleAsync_WithInvalidBuildingId_ReturnsBadRequest()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.CreateFloorAsync(_validBuildingId))
                .ReturnsAsync(true);

            // Act
            var result = await PostFloorHandler.HandleAsync(_serviceMock.Object, _invalidBuildingId);

            // Assert
            result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();

            var badRequest = result.Result as BadRequest<List<ValidationError>>;

            badRequest!.Value.Should().ContainSingle(e =>
                e.Parameter == "BuildingId" &&
                e.Message == "Invalid building id format.");
        }

        /// <summary>
        /// Tests that <see cref="PostFloorHandler.HandleAsync"/> returns a <see cref="NotFound{T}"/> result
        /// when the service throws a <see cref="NotFoundException"/>.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a result of type
        /// </returns>
        [Fact]
        public async Task HandleAsync_WhenServiceThrowsNotFoundException_ReturnsNotFound()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.CreateFloorAsync(_validBuildingId))
                .ThrowsAsync(new NotFoundException("Building not found."));

            // Act
            var result = await PostFloorHandler.HandleAsync(_serviceMock.Object, _validBuildingId);

            // Assert
            result.Result.Should().BeOfType<NotFound<string>>();

            var notFound = result.Result as NotFound<string>;

            notFound!.Value.Should().Be("Building not found.");
        }

        /// <summary>
        /// Tests that <see cref="PostFloorHandler.HandleAsync"/> returns a <see cref="Conflict{T}"/> result
        /// when the service throws a <see cref="ConcurrencyConflictException"/>.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a result of type
        /// </returns>
        [Fact]
        public async Task HandleAsync_WhenServiceThrowsConcurrencyConflictException_ReturnsConflict()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.CreateFloorAsync(_validBuildingId))
                .ThrowsAsync(new ConcurrencyConflictException("Concurrency conflict."));

            // Act
            var result = await PostFloorHandler.HandleAsync(_serviceMock.Object, _validBuildingId);

            // Assert
            result.Result.Should().BeOfType<Conflict<string>>();

            var conflict = result.Result as Conflict<string>;

            conflict!.Value.Should().Be("Concurrency conflict.");
        }

        /// <summary>
        /// Tests that <see cref="PostFloorHandler.HandleAsync"/> returns a <see cref="Conflict{T}"/> result
        /// when the service throws a <see cref="DuplicatedEntityException"/>.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation, containing a result of type
        /// </returns>
        [Fact]
        public async Task HandleAsync_WhenServiceThrowsDuplicatedEntityException_ReturnsConflict()
        {
            // Arrange
            _serviceMock
                .Setup(s => s.CreateFloorAsync(_validBuildingId))
                .ThrowsAsync(new DuplicatedEntityException("Floor already exists."));

            // Act
            var result = await PostFloorHandler.HandleAsync(_serviceMock.Object, _validBuildingId);

            // Assert
            result.Result.Should().BeOfType<Conflict<string>>();

            var conflict = result.Result as Conflict<string>;

            conflict!.Value.Should().Be("Floor already exists.");
        }
    }
}