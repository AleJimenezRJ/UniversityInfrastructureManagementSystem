using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.Spaces;

/// <summary>
/// Contains unit tests for the <see cref="PutLearningSpaceHandler"/> class.
/// </summary>
public class PutLearningSpaceHandlerTests
{
    private readonly Mock<ILearningSpaceServices> _serviceMock;
    private readonly LearningSpace _exampleSpace;
    private readonly LearningSpaceDto _exampleDto;
    private readonly LearningSpaceDto _badDto;
    private readonly int validId = 1;
    private readonly int invalidId = -5;

    /// <summary>
    /// Initializes a new instance of the <see cref="PutLearningSpaceHandlerTests"/> class,
    /// setting up mocks and sample objects for the test cases.
    /// </summary>
    public PutLearningSpaceHandlerTests()
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

        _exampleDto = new LearningSpaceDto(
            "Physics Lab",
            "Laboratory",
            30,
            90,
            100,
            80,
            "Blue",
            "Gray",
            "White"
        );

        _badDto = new LearningSpaceDto(
            "",
            "",
            -1,
            -10,
            -10,
            -10,
            "",
            "",
            ""
        );
    }

    /// <summary>
    /// Tests that the handler returns an Ok result when a valid request is provided
    /// to update a learning space.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WithValidRequest_ReturnsOk()
    {
        var request = new PutLearningSpaceRequest(_exampleDto);

        _serviceMock.Setup(s => s.UpdateLearningSpaceAsync(validId, It.IsAny<LearningSpace>()))
                    .ReturnsAsync(true);

        var result = await PutLearningSpaceHandler.HandleAsync(_serviceMock.Object, request, validId);

        result.Result.Should().BeOfType<Ok<PutLearningSpaceResponse>>();
        var ok = result.Result as Ok<PutLearningSpaceResponse>;
        ok!.Value?.Message.Should().Contain("updated successfully");
    }

    /// <summary>
    /// Tests that the handler returns a BadRequest result when an invalid ID is provided.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WithInvalidId_ReturnsBadRequest()
    {
        var request = new PutLearningSpaceRequest(_exampleDto);

        var result = await PutLearningSpaceHandler.HandleAsync(_serviceMock.Object, request, invalidId);

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var badRequest = result.Result as BadRequest<List<ValidationError>>;
        badRequest!.Value.Should().Contain(e => e.Message.Contains("Invalid learning space Id format."));
    }

    /// <summary>
    /// Tests that the handler returns a BadRequest result when an invalid DTO is provided.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WithInvalidDto_ReturnsBadRequest()
    {
        var request = new PutLearningSpaceRequest(_badDto);

        var result = await PutLearningSpaceHandler.HandleAsync(_serviceMock.Object, request, 1);

        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
        var badRequest = result.Result as BadRequest<List<ValidationError>>;
        badRequest!.Value.Should().NotBeEmpty();

        // No mock setup is needed since validation fails before the service call.
    }

    /// <summary>
    /// Tests that the handler returns a NotFound result when the target learning space
    /// does not exist.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenNotFound_ReturnsNotFound()
    {
        var request = new PutLearningSpaceRequest(_exampleDto);

        _serviceMock.Setup(s => s.UpdateLearningSpaceAsync(validId, It.IsAny<LearningSpace>()))
                    .ThrowsAsync(new NotFoundException("Not found"));

        var result = await PutLearningSpaceHandler.HandleAsync(_serviceMock.Object, request, validId);

        result.Result.Should().BeOfType<NotFound<string>>();
        var notFound = result.Result as NotFound<string>;
        notFound!.Value.Should().Be("Not found");
    }

    /// <summary>
    /// Tests that the handler returns a Conflict result when a concurrency conflict occurs during the update.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenConflict_ReturnsConflict()
    {
        var request = new PutLearningSpaceRequest(_exampleDto);

        _serviceMock.Setup(s => s.UpdateLearningSpaceAsync(validId, It.IsAny<LearningSpace>()))
                    .ThrowsAsync(new ConcurrencyConflictException("Conflict"));

        var result = await PutLearningSpaceHandler.HandleAsync(_serviceMock.Object, request, validId);

        result.Result.Should().BeOfType<Conflict<string>>();
        var conflict = result.Result as Conflict<string>;
        conflict!.Value.Should().Be("Conflict");
    }

    /// <summary>
    /// Tests that the handler returns a Conflict result when a DuplicatedEntityException is thrown.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenDuplicatedEntity_ReturnsConflict()
    {
        var request = new PutLearningSpaceRequest(_exampleDto);

        _serviceMock.Setup(s => s.UpdateLearningSpaceAsync(validId, It.IsAny<LearningSpace>()))
                    .ThrowsAsync(new DuplicatedEntityException("Already exists"));

        var result = await PutLearningSpaceHandler.HandleAsync(_serviceMock.Object, request, validId);

        result.Result.Should().BeOfType<Conflict<string>>();
        var conflict = result.Result as Conflict<string>;
        conflict!.Value.Should().Be("Already exists");
    }
}
