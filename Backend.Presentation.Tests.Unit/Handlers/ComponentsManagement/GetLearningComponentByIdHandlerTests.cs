using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;


namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.ComponentsManagement;

/// <summary>
/// Unit tests for the <see cref="GetLearningComponentsByIdHandler"/> class.
/// </summary>
public class GetLearningComponentByIdHandlerTests
{
    /// <summary>
    /// Mock of the <see cref="ILearningComponentServices"/> used to simulate service behavior in tests.
    /// </summary>
    private readonly Mock<ILearningComponentServices> _serviceMock;
    /// <summary>
    /// A valid projector instance used for testing.
    /// </summary>
    private readonly Projector _validProjector;
    /// <summary>
    /// A valid whiteboard instance used for testing.
    /// </summary>
    private readonly Whiteboard _validWhiteboard;
    /// <summary>
    /// An invalid component instance used to test error handling in the handler.
    /// </summary>
    private readonly UnknownComponent _invalidComponent;

    /// <summary>
    /// Array containing the expected IDs of the returned learning components for test validation.
    /// </summary>
    private static readonly int[] Expected = [1, 2]; 
    
    /// <summary>
    /// Represents the default number of items per page used in pagination for tests
    /// within the LearningComponentServiceTests class.
    /// It ensures consistent handling of paginated data scenarios across unit tests.
    /// </summary>
    private readonly int _pageSize;

    /// <summary>
    /// Represents the index of the current page used for pagination or iteration during unit tests
    /// related to learning component service functionalities.
    /// Helps in validating scenarios involving paginated data retrieval.
    /// </summary>
    private readonly int _pageIndex;

    private readonly string _stringSearch;

    private readonly GlobalMapper _globalMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLearningComponentByIdHandlerTests"/> class.
    /// </summary>
    public GetLearningComponentByIdHandlerTests()
    {
        _pageSize = 10;
        _pageIndex = 0;
        _globalMapper = new GlobalMapper();

        _stringSearch = string.Empty;
        _serviceMock = new Mock<ILearningComponentServices>(MockBehavior.Strict);

        _validProjector = new Projector(
                "Slides",
                Area2D.Create(4, 5),
                1, Orientation.Create("North"),
                Coordinates.Create(1, 2, 3),
                Dimension.Create(1, 1, 1)
        );

        _validWhiteboard = new Whiteboard(
                Colors.Create("Red"), 
                2,
                Orientation.Create("East"),
                Coordinates.Create(4, 5, 6),
                Dimension.Create(1, 1, 1)
        );

        _invalidComponent = new UnknownComponent(
                id: 999,
                orientation: Orientation.Create("North"),
                dimensions: Dimension.Create(1, 1, 1),
                position: Coordinates.Create(0, 0, 0)
        );


    }

    /// <summary>
    /// Tests that the handler returns a BadRequest when no components are found for a valid learning space ID.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenValidComponentsAreReturned_ShouldReturnOk()
    {
        // Arrange
        int learningSpaceId = 1;

        _serviceMock
            .Setup(lcService => lcService.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, _stringSearch))
            .ReturnsAsync(new PaginatedList<LearningComponent>(
                [_validProjector, _validWhiteboard], 
                2, 
                _pageSize,
                _pageIndex)
            );

        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, learningSpaceId);

        // Assert
        result.Result.Should().BeOfType<Ok<GetLearningComponentsByIdResponse>>();
    }

    /// <summary>
    /// Tests that the handler returns a BadRequest when no components are found for a valid learning space ID.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenOnlyWhiteboardIsReturned_ShouldMapCorrectWhiteboardDtoType()
    {
        // Arrange
        int learningSpaceId = 1;
        _serviceMock.Setup(s => s.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, _stringSearch))
            .ReturnsAsync(new PaginatedList<LearningComponent>(
                [_validWhiteboard],
                1,
                _pageSize,
                _pageIndex)
            );

        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, learningSpaceId);
        var response = result.Result.As<Ok<GetLearningComponentsByIdResponse>>().Value;

        // Assert
        response!.LearningComponents[0].Should().BeOfType<WhiteboardDto>();
    }

    /// <summary>
    /// Tests that the handler returns a BadRequest when only a projector is returned for a valid learning space ID.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenOnlyProjectorIsReturned_ShouldMapCorrectProjectorDtoType()
    {
        // Arrange 
        int learningSpaceId = 1;
        _serviceMock.Setup(s => s.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, _stringSearch))
            .ReturnsAsync(new PaginatedList<LearningComponent>(
                [_validProjector],
                1,
                _pageSize,
                _pageIndex)
            );

        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, learningSpaceId);
        var response = result.Result.As<Ok<GetLearningComponentsByIdResponse>>().Value;

        // Assert
        response!.LearningComponents[0].Should().BeOfType<ProjectorDto>();
    }

    /// <summary>
    /// Tests that the handler returns a BadRequest when multiple valid components are returned for a valid learning space ID.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenMultipleValidComponentsAreReturned_ShouldReturnDtosAssignableToLearningComponentDto()
    {
        // Arrange
        int learningSpaceId = 1;

        _serviceMock
            .Setup(lcService => lcService.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, _stringSearch))
            .ReturnsAsync(new PaginatedList<LearningComponent>(
                [_validProjector, _validWhiteboard], 
                2, 
                _pageSize,
                _pageIndex)
            );

        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, learningSpaceId);
        var response = result.Result.As<Ok<GetLearningComponentsByIdResponse>>().Value;

        // Assert
        response!.LearningComponents.Should().AllBeAssignableTo<LearningComponentDto>();
    }

    /// <summary>
    /// Tests that the handler correctly maps multiple components to their respective IDs in the response.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenMultipleComponentsAreReturned_ShouldMapCorrectIdsInResponse()
    {
        // Arrange
        int learningSpaceId = 1;

        _serviceMock
            .Setup(lcService => lcService.GetLearningComponentsByIdAsync(learningSpaceId, _pageSize, _pageIndex, _stringSearch))
            .ReturnsAsync(new PaginatedList<LearningComponent>(
                [_validProjector, _validWhiteboard], 
                2, 
                _pageSize,
                _pageIndex)
            );

        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, learningSpaceId);
        var response = result.Result.As<Ok<GetLearningComponentsByIdResponse>>().Value;

        // Assert
        response!.LearningComponents.Select(component => component.Id).Should().Contain(Expected);
    }
    /// <summary>
    /// Tests that the handler returns a BadRequest when an invalid learning space ID is provided.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_WhenIdIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        int invalidId = -99;

        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, invalidId);

        // Assert
        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        Assert.Single(badRequest.Value!);
        Assert.Equal("Invalid learning space id format.", badRequest.Value![0].Message);
    }


    /// <summary>
    /// Tests that the handler returns a BadRequest when the page size is invalid (outside the allowed range).
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenPageSizeIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        int validLearningSpaceId = 1;
        int invalidPageSize = 25;

        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, validLearningSpaceId, "", invalidPageSize, _pageIndex);

        // Assert
        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().ContainSingle(v => v.Message.Contains("Page size must be greater"));
    }



    /// <summary>
    /// Tests that the handler returns a BadRequest when the page index is invalid (less than zero).
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenPageIndexIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        int validLearningSpaceId = 1;
        int invalidPageIndex = -1;

        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, validLearningSpaceId, "", _pageSize, invalidPageIndex);

        // Assert
        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().ContainSingle(v => v.Message.Contains("Page index must be greater"));
    }


    /// <summary>
    /// Tests that the handler returns a BadRequest when both the page size and page index are invalid.
    /// Ensures that all validation errors are returned in the response.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task HandleAsync_WhenPageSizeAndPageIndexAreInvalid_ReturnsAllValidationErrors()
    {
        // Arrange
        int validLearningSpaceId = 1;
        int invalidPageSize = 0;
        int invalidPageIndex = -5;

        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, validLearningSpaceId, "", invalidPageSize, invalidPageIndex);

        // Assert
        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().HaveCount(2);
    }


    [Fact]
    /// <summary>
    /// Tests that the handler returns a BadRequest when an unknown component type is received by the mapper.
    /// Ensures that the error message indicates no ID-based mapper is registered for the unknown type.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    public async Task HandleAsync_WhenMapperReceivesUnknownComponentType_ReturnsBadRequestWithNotSupportedMessage()
    {
        // Arrange
        int validId = 2;

        var components = new PaginatedList<LearningComponent>(
            [new UnknownComponent(999, Orientation.Create("North"), Dimension.Create(1, 1, 1), Coordinates.Create(0, 0, 0))],
            1,
            _pageSize,
            _pageIndex
        );

        _serviceMock.Setup(s => s.GetLearningComponentsByIdAsync(validId, _pageSize, _pageIndex, _stringSearch))
                    .ReturnsAsync(components);

        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(_serviceMock.Object, _globalMapper, validId);

        // Assert
        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().ContainSingle();
        badRequest.Value![0].Message.Should().Contain("No ID-based mapper registered");

    }



    /// <summary>
    /// Represents an unknown learning component type used for testing error handling in the handler.
    /// </summary>
    private class UnknownComponent : LearningComponent
    {
        public UnknownComponent(int id, Orientation orientation, Dimension dimensions, Coordinates position)
            : base(id, orientation, dimensions, position) { }
    }

    /// <summary>
    /// Represents a throwing component within a learning system.
    /// This class inherits from <see cref="LearningComponent"/> and is initialized with predefined
    /// values for its identifier, orientation, dimensions, and coordinates.
    /// </summary>
    private class ThrowingComponent : LearningComponent
    {
        public ThrowingComponent()
            : base(1, Orientation.Create("North"), Dimension.Create(1, 1, 1), Coordinates.Create(1, 1, 1)) { }
    }

}
