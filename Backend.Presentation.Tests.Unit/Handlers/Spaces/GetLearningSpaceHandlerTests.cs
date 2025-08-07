using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.Spaces;

/// <summary>
/// Unit tests for the <see cref="GetLearningSpaceHandler"/> class.
/// These tests verify the different outcomes of retrieving a learning space by ID,
/// including successful retrieval, invalid ID, not found, and concurrency conflict scenarios.
/// </summary>
public class GetLearningSpaceHandlerTests
{
    private readonly Mock<ILearningSpaceServices> _serviceMock;

    private readonly LearningSpace _exampleSpace;

    /// <summary>
    /// Initializes the test class with a mock of <see cref="ILearningSpaceServices"/> and an example learning space.
    /// </summary>
    public GetLearningSpaceHandlerTests()
    {
        _serviceMock = new Mock<ILearningSpaceServices>(MockBehavior.Strict);

        _exampleSpace = new LearningSpace(
            EntityName.Create("Physics Lab"),
            LearningSpaceType.Create("Laboratory"),
            Capacity.Create(30),
            Size.Create(90),
            Size.Create(100),
            Size.Create(80),
            Colors.Create("Blue"),
            Colors.Create("Gray"),
            Colors.Create("White")
        );
    }

    /// <summary>
    /// Verifies that a valid ID returns a 200 OK response with a correctly mapped DTO.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WithValidId_ReturnsOkWithMappedResponse()
    {
        // Arrange
        int validId = 1;
        _serviceMock
            .Setup(s => s.GetLearningSpaceAsync(validId))
            .ReturnsAsync(_exampleSpace);

        // Act
        var result = await GetLearningSpaceHandler.HandleAsync(_serviceMock.Object, validId);

        // Assert
        result.Result.Should().BeOfType<Ok<GetLearningSpaceResponse>>();
        var okResult = result.Result as Ok<GetLearningSpaceResponse>;

        okResult!.Value.Should().NotBeNull();
        var dto = okResult.Value.LearningSpace;

        // Validate DTO content
        dto.Name.Should().Be("Physics Lab");
        dto.Type.Should().Be("Laboratory");
        dto.MaxCapacity.Should().Be(30);
        dto.Height.Should().Be(90);
        dto.Width.Should().Be(100);
        dto.Length.Should().Be(80);
        dto.ColorFloor.Should().Be("Blue");
        dto.ColorWalls.Should().Be("Gray");
        dto.ColorCeiling.Should().Be("White");
    }

    /// <summary>
    /// Verifies that an invalid ID (e.g., negative) returns a 400 Bad Request with appropriate validation errors.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        int invalidId = -5;

        // Act
        var result = await GetLearningSpaceHandler.HandleAsync(_serviceMock.Object, invalidId);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var badRequest = result.Result as BadRequest<List<ValidationError>>;
        badRequest!.Value.Should().ContainSingle(e =>
            e.Message == "Invalid learning space id format.");
    }

    /// <summary>
    /// Verifies that when the learning space does not exist, a 404 Not Found is returned.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenNotFound_ThrowsNotFound()
    {
        // Arrange
        int validId = 10;
        _serviceMock
            .Setup(s => s.GetLearningSpaceAsync(validId))
            .ThrowsAsync(new NotFoundException("Learning space not found."));

        // Act
        var result = await GetLearningSpaceHandler.HandleAsync(_serviceMock.Object, validId);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        var notFound = result.Result as NotFound<string>;
        notFound!.Value.Should().Be("Learning space not found.");
    }

    /// <summary>
    /// Verifies that when a concurrency conflict occurs, a 409 Conflict is returned.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenConflict_ThrowsConflict()
    {
        // Arrange
        int validId = 2;
        _serviceMock
            .Setup(s => s.GetLearningSpaceAsync(validId))
            .ThrowsAsync(new ConcurrencyConflictException("Concurrent modification detected."));

        // Act
        var result = await GetLearningSpaceHandler.HandleAsync(_serviceMock.Object, validId);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
        var conflict = result.Result as Conflict<string>;
        conflict!.Value.Should().Be("Concurrent modification detected.");
    }
}


