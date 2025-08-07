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
/// Contains unit tests for the <see cref="PutProjectorHandler"/> class, which handles updating projectors.
/// Tests include validation of input DTOs, service update results, and response types.
/// </summary>
public class PutProjectorHandlerTests
{
    /// <summary>
    /// Mock object of the <see cref="IProjectorServices"/> interface used in unit tests
    /// to simulate the behavior of projector service operations.
    /// Configured to verify method interactions and provide controlled responses
    /// for validating different test scenarios involving the <see cref="PutProjectorHandler"/> class.
    /// </summary>
    private readonly Mock<IProjectorServices> _serviceMock;

    /// <summary>
    /// Represents a read-only instance of the <see cref="GlobalMapper"/> class.
    /// </summary>
    private readonly GlobalMapper _globalMapper;

    /// <summary>
    /// Represents a valid instance of <see cref="ProjectorNoIdDto"/> used for unit testing in the <c>PutProjectorHandlerTests</c> class.
    /// This variable is initialized with appropriate data to simulate valid input values for test scenarios involving a projector components.
    /// </summary>
    private readonly ProjectorNoIdDto _validDto;

    /// <summary>
    /// Represents an invalid instance of the <see cref="ProjectorNoIdDto"/> class, used in unit tests
    /// to simulate scenarios involving faulty or improperly configured projector data. This variable is
    /// specifically initialized with values that do not conform to expected formats or validation constraints,
    /// to test error handling and validation workflows in the <see cref="PutProjectorHandler"/> class.
    /// </summary>
    private readonly ProjectorNoIdDto _invalidDto;

    /// <summary>
    /// An instance of <see cref="ProjectorNoIdDto"/> that contains intentionally invalid values
    /// for testing scenarios involving multiple validation errors in the context of
    /// the <see cref="PutProjectorHandler"/> functionality.
    /// This variable is used to evaluate the robustness of input validation,
    /// ensuring proper handling and reporting of multiple field errors.
    /// </summary>
    private readonly ProjectorNoIdDto _multipleErrorsDto;

    /// <summary>
    /// Represents the expected validation error parameter names when handling invalid projector update requests
    /// in the <see cref="PutProjectorHandler"/> tests. Used to ensure that the collection of validation errors
    /// contains the correct fields related to projector configuration issues, such as orientation or dimensions.
    /// </summary>
    private static readonly string[] Expected = ["Orientation", "Dimensions"];

    /// <summary>
    /// Initializes a new instance of the <see cref="PutProjectorHandlerTests"/> class.
    /// Sets up mock services and test DTOs for use in test cases.
    /// </summary>
    public PutProjectorHandlerTests()
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

    /// <summary>
    /// Tests that a bad request is returned when the DTO is invalid.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenDtoIsInvalid_ShouldReturnBadRequest()
    {
        var request = new PutProjectorRequest(_invalidDto);

        var result = await PutProjectorHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1, 1);

        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().Contain(v =>
            v.Parameter == "Projection Area" &&
            v.Message.Contains("Invalid projection area dimensions", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Tests that a conflict result is returned when the update fails in the service.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenUpdateFails_ShouldReturnConflict()
    {
        var request = new PutProjectorRequest(_validDto);
        _serviceMock.Setup(s =>
            s.UpdateProjectorAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Projector>())
        ).ReturnsAsync(false);

        var result = await PutProjectorHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1, 2);

        result.Result.Should().BeOfType<Conflict>();
    }

    /// <summary>
    /// Tests that an OK result is returned when the service update succeeds.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenServiceSucceeds_ShouldReturnOk()
    {
        var request = new PutProjectorRequest(_validDto);
        _serviceMock.Setup(s =>
            s.UpdateProjectorAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Projector>())
        ).ReturnsAsync(true);

        var result = await PutProjectorHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1, 2);

        result.Result.Should().BeOfType<Ok<PutProjectorResponse>>();
    }

    /// <summary>
    /// Tests that the returned DTO matches the expected values when the projector is updated.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenProjectorIsUpdated_ShouldReturnExpectedDto()
    {
        var request = new PutProjectorRequest(_validDto);
        _serviceMock.Setup(s =>
            s.UpdateProjectorAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Projector>())
        ).ReturnsAsync(true);

        var result = await PutProjectorHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1, 2);
        var ok = result.Result.As<Ok<PutProjectorResponse>>();

        ok.Value!.Projector.Should().BeEquivalentTo(_validDto);
    }

    /// <summary>
    /// Tests that multiple validation errors are returned when multiple fields are invalid in the DTO.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenMultipleFieldsAreInvalid_ShouldReturnMultipleValidationErrors()
    {
        var request = new PutProjectorRequest(_multipleErrorsDto);

        var result = await PutProjectorHandler.HandleAsync(_serviceMock.Object, _globalMapper, request, 1, 1);

        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().HaveCountGreaterThanOrEqualTo(2);
        badRequest.Value.Select(e => e.Parameter)
            .Should().Contain(Expected);
    }
}
