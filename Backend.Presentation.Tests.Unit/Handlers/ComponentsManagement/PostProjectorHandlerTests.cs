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

public class PostProjectorHandlerTests
{
    /// <summary>
    /// Mock for the <see cref="IProjectorServices"/> interface used to simulate service behavior in unit tests.
    /// </summary>
    private readonly Mock<IProjectorServices> _serviceMock;

    /// <summary>
    /// A valid <see cref="ProjectorNoIdDto"/> instance used for positive test scenarios.
    /// </summary>
    private readonly ProjectorNoIdDto _validDto;

    /// <summary>
    /// An invalid <see cref="ProjectorNoIdDto"/> instance used for negative test scenarios.
    /// </summary>
    private readonly ProjectorNoIdDto _invalidDto;

    /// <summary>
    /// Represents a DTO object used in testing scenarios to simulate
    /// validation of multiple error cases for projector data during request handling.
    /// </summary>
    private readonly ProjectorNoIdDto _multipleErrorsDto;

    /// <summary>
    /// Represents a read-only instance of the <see cref="GlobalMapper"/> class.
    /// </summary>
    private readonly GlobalMapper _globalMapper;

    /// <summary>
    /// Represents the expected parameter names to check against validation errors in the
    /// <see cref="PostProjectorHandlerTests"/> unit tests.
    /// </summary>
    private static readonly string[] Expected =
    [
        "Orientation", "Dimensions", "Projection Area"
    ];

    /// <summary>
    /// Initializes a new instance of the <see cref="PostProjectorHandlerTests"/> class.
    /// Sets up valid and invalid DTOs and a strict mock for <see cref="IProjectorServices"/>.
    /// </summary>
    public PostProjectorHandlerTests()
    {
        _serviceMock = new Mock<IProjectorServices>(MockBehavior.Strict);

        _globalMapper = new GlobalMapper();

        _validDto = new ProjectorNoIdDto(
            Orientation: "North",
            Position: new PositionDto(1, 2, 3),
            Dimensions: new DimensionsDto(2.0, 1.0, 0.5),
            ProjectionArea: new ProjectionAreaDto(3, 2),
            ProjectedContent: "Slides"
        );

        _invalidDto = new ProjectorNoIdDto(
            Orientation: "InvalidDirection",
            Position: new PositionDto(1, 2, 3),
            Dimensions: new DimensionsDto(0.0, -1.0, 0.0), 
            ProjectionArea: new ProjectionAreaDto(0, 0),
            ProjectedContent: ""
        );

        _multipleErrorsDto = new ProjectorNoIdDto(
            Orientation: "invalid-orientation",
            Position: new PositionDto(100, 200, 300),
            Dimensions: new DimensionsDto(0, 0, 0),
            ProjectionArea: new ProjectionAreaDto(-1.0, -2.0),
            ProjectedContent: ""
        );
    }

    [Fact]
    public async Task HandleAsync_WhenDtoIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new PostProjectorRequest(_invalidDto);

        // Act
        var result = await PostProjectorHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1);

        // Assert
        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().Contain(v =>
            v.Parameter == "Projection Area" &&
            v.Message.Contains("Invalid projection area dimensions", StringComparison.OrdinalIgnoreCase)); 
    }

    [Fact]
    public async Task HandleAsync_WhenAdditionFails_ShouldReturnConflict()
    {
        var request = new PostProjectorRequest(_validDto);
        _serviceMock
            .Setup(s => s.AddProjectorAsync(It.IsAny<int>(), It.IsAny<Projector>()))
            .ReturnsAsync(false);

        var result = await PostProjectorHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 42);

        result.Result.Should().BeOfType<Conflict>();
    }

    [Fact]
    public async Task HandleAsync_WhenServiceSucceeds_ShouldReturnOk()
    {
        var request = new PostProjectorRequest(_validDto);
        _serviceMock
            .Setup(s => s.AddProjectorAsync(It.IsAny<int>(), It.IsAny<Projector>()))
            .ReturnsAsync(true);

        var result = await PostProjectorHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1);

        result.Result.Should().BeOfType<Ok<PostProjectorResponse>>();
    }

    [Fact]
    public async Task HandleAsync_WhenProjectorIsAdded_ShouldReturnExpectedDto()
    {
        var request = new PostProjectorRequest(_validDto);
        _serviceMock
            .Setup(s => s.AddProjectorAsync(It.IsAny<int>(), It.IsAny<Projector>()))
            .ReturnsAsync(true);

        var result = await PostProjectorHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1);
        var ok = result.Result.As<Ok<PostProjectorResponse>>();

        ok.Value!.Projector.Should().BeEquivalentTo(_validDto);
    }

    [Fact]
    public async Task HandleAsync_WhenMultipleFieldsAreInvalid_ShouldReturnMultipleValidationErrors()
    {
        // Arrange
        var request = new PostProjectorRequest(_multipleErrorsDto);

        // Act
        var result = await PostProjectorHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1);

        // Assert
        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);

        badRequest.Value.Should().HaveCountGreaterThan(2);
        badRequest.Value.Select(e => e.Parameter).Should().Contain(Expected);
    }


}
