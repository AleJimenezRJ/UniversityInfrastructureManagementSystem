using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.Spaces
{
    /// <summary>
    /// Unit tests for the <see cref="GetFloorPaginatedListHandler"/> class.
    /// These tests verify the different outcomes of retrieving a paginated list of floors,
    /// including successful retrieval, invalid parameters, not found, and other scenarios.
    /// </summary>
    public class GetFloorPaginatedListHandlerTests
    {
        /// <summary>
        /// Mock for the floor services used in the tests.
        /// </summary>
        private readonly Mock<IFloorServices> _serviceMock;

        /// <summary>
        /// A valid building ID that will be used in tests to simulate a successful retrieval.
        /// </summary>
        private readonly int _validBuildingId;

        /// <summary>
        /// An invalid building ID (e.g., negative value) that will be used in tests to simulate an error scenario.
        /// </summary>
        private readonly int _invalidBuildingId;

        /// <summary>
        /// A valid page size value for testing.
        /// </summary>
        private readonly int _validPageSize;

        /// <summary>
        /// An invalid page size value for testing.
        /// </summary>
        private readonly int _invalidPageSize;

        /// <summary>
        /// A valid page index value for testing.
        /// </summary>
        private readonly int _validPageIndex;

        /// <summary>
        /// An invalid page index value for testing.
        /// </summary>
        private readonly int _invalidPageIndex;

        /// <summary>
        /// Example floors for testing.
        /// </summary>
        private readonly List<Floor> _exampleFloors;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFloorPaginatedListHandlerTests"/> class.
        /// </summary>
        public GetFloorPaginatedListHandlerTests()
        {
            // Initialize the mock for the floor services
            _serviceMock = new Mock<IFloorServices>(MockBehavior.Strict);

            // Set test values
            _validBuildingId = 1;
            _invalidBuildingId = -1;
            _validPageSize = 10;
            _invalidPageSize = 0;
            _validPageIndex = 0;
            _invalidPageIndex = -1;

            // Create example floors
            _exampleFloors = new List<Floor>
            {
                new Floor(FloorNumber.Create(1)),
                new Floor(FloorNumber.Create(2)),
                new Floor(FloorNumber.Create(3))
            };
        }

        /// <summary>
        /// Tests that the handler returns an OK result with the correct data when valid parameters are provided.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task HandleAsync_WithValidParameters_ReturnsOkWithPaginatedData()
        {
            // Arrange
            var paginatedList = new PaginatedList<Floor>(_exampleFloors, 3, _validPageSize, _validPageIndex);

            _serviceMock.Setup(s => s.GetFloorsListPaginatedAsync(_validBuildingId, _validPageSize, _validPageIndex))
                .ReturnsAsync(paginatedList);

            // Act
            var result = await GetFloorPaginatedListHandler.HandleAsync(_serviceMock.Object, _validBuildingId, _validPageSize, _validPageIndex);

            // Assert
            result.Result.Should().BeOfType<Ok<GetFloorPaginatedListResponse>>();
            var okResult = result.Result as Ok<GetFloorPaginatedListResponse>;
            okResult.Should().NotBeNull();
            okResult!.Value!.Floors.Should().HaveCount(3);
            okResult!.Value.TotalCount.Should().Be(3);
            var floorsList = okResult.Value.Floors.ToList();
            floorsList[0].FloorNumber.Should().Be(_exampleFloors[0].Number.Value);
            floorsList[1].FloorNumber.Should().Be(_exampleFloors[1].Number.Value);
        }

        /// <summary>
        /// Tests that the handler returns a BadRequest result when an invalid building ID is provided.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task HandleAsync_WithInvalidBuildingId_ReturnsBadRequest()
        {
            // Act
            var result = await GetFloorPaginatedListHandler.HandleAsync(_serviceMock.Object, _invalidBuildingId, _validPageSize, _validPageIndex);

            // Assert
            result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
            
            var badRequest = result.Result as BadRequest<List<ValidationError>>;
            badRequest!.Value.Should().ContainSingle(e => 
                e.Parameter == "BuildingId" && 
                e.Message == "Invalid building id format.");
        }

        /// <summary>
        /// Tests that the handler returns a BadRequest result when an invalid page size is provided.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task HandleAsync_WithInvalidPageSize_ReturnsBadRequest()
        {
            // Act
            var result = await GetFloorPaginatedListHandler.HandleAsync(_serviceMock.Object, _validBuildingId, _invalidPageSize, _validPageIndex);

            // Assert
            result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
            
            var badRequest = result.Result as BadRequest<List<ValidationError>>;
            badRequest!.Value.Should().ContainSingle(e => 
                e.Parameter == "PageSize" && 
                e.Message.Contains("Page size must be greater than or equal to 1"));
        }

        /// <summary>
        /// Tests that the handler returns a BadRequest result when an invalid page index is provided.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task HandleAsync_WithInvalidPageIndex_ReturnsBadRequest()
        {
            // Act
            var result = await GetFloorPaginatedListHandler.HandleAsync(_serviceMock.Object, _validBuildingId, _validPageSize, _invalidPageIndex);

            // Assert
            result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
            
            var badRequest = result.Result as BadRequest<List<ValidationError>>;
            badRequest!.Value.Should().ContainSingle(e => 
                e.Parameter == "PageIndex" && 
                e.Message == "Page index must be greater than or equal to 0.");
        }

        /// <summary>
        /// Tests that the handler returns a NotFound result when the building is not found.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task HandleAsync_WhenNoFloorsFound_ReturnsNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetFloorsListPaginatedAsync(_validBuildingId, _validPageSize, _validPageIndex))
                .ThrowsAsync(new NotFoundException($"Building with Id '{_validBuildingId}' not found."));

            // Act
            var result = await GetFloorPaginatedListHandler.HandleAsync(_serviceMock.Object, _validBuildingId, _validPageSize, _validPageIndex);

            // Assert
            result.Result.Should().BeOfType<NotFound<string>>();
            
            var notFound = result.Result as NotFound<string>;
            notFound!.Value.Should().Be($"Building with Id '{_validBuildingId}' not found.");
        }

        /// <summary>
        /// Tests that the handler returns an OK result with an empty list when the paginated list is empty but valid.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task HandleAsync_WithEmptyPaginatedList_ReturnsOkWithEmptyList()
        {
            // Arrange
            var emptyList = new PaginatedList<Floor>(new List<Floor>(), 0, _validPageSize, _validPageIndex);
            
            _serviceMock.Setup(s => s.GetFloorsListPaginatedAsync(_validBuildingId, _validPageSize, _validPageIndex))
                .ReturnsAsync(emptyList);

            // Act
            var result = await GetFloorPaginatedListHandler.HandleAsync(_serviceMock.Object, _validBuildingId, _validPageSize, _validPageIndex);

            // Assert
            result.Result.Should().BeOfType<Ok<GetFloorPaginatedListResponse>>();
            
            var okResult = result.Result as Ok<GetFloorPaginatedListResponse>;
            
            okResult!.Value!.Floors.Should().BeEmpty();
        }
    }
}
