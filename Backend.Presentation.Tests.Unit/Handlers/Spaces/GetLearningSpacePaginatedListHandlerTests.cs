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

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.Spaces
{
    /// <summary>
    /// Unit tests for the <see cref="GetLearningSpacePaginatedListHandler"/> class.
    /// These tests verify the different outcomes of retrieving a paginated list of learning spaces,
    /// including successful retrieval, invalid parameters, not found, and other scenarios.
    /// </summary>
    public class GetLearningSpacePaginatedListHandlerTests
    {
        /// <summary>
        /// Mock for the learning space services used in the tests.
        /// </summary>
        private readonly Mock<ILearningSpaceServices> _serviceMock;

        /// <summary>
        /// A valid floor ID that will be used in tests to simulate a successful retrieval.
        /// </summary>
        private readonly int _validFloorId;

        /// <summary>
        /// An invalid floor ID (e.g., negative value) that will be used in tests to simulate an error scenario.
        /// </summary>
        private readonly int _invalidFloorId;

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
        /// An invalid page index value for testing.
        /// </summary>
        private readonly string _searchText;

        /// <summary>
        /// Example learning spaces for testing.
        /// </summary>
        private readonly List<LearningSpace> _exampleLearningSpaces;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLearningSpacePaginatedListHandlerTests"/> class.
        /// </summary>
        public GetLearningSpacePaginatedListHandlerTests()
        {
            // Initialize the mock for the learning space services
            _serviceMock = new Mock<ILearningSpaceServices>(MockBehavior.Strict);

            // Set test values
            _validFloorId = 1;
            _invalidFloorId = -1;
            _validPageSize = 10;
            _invalidPageSize = 0;
            _validPageIndex = 0;
            _invalidPageIndex = -1;
            _searchText = "Test";

            // Create example learning spaces
            _exampleLearningSpaces = new List<LearningSpace>
            {
                new LearningSpace(
                    EntityName.Create("Physics Lab"),
                    LearningSpaceType.Create("Laboratory"),
                    Capacity.Create(30),
                    Size.Create(90),
                    Size.Create(100),
                    Size.Create(80),
                    Colors.Create("Blue"),
                    Colors.Create("Gray"),
                    Colors.Create("White")
                ),
                new LearningSpace(
                    EntityName.Create("Computer Lab"),
                    LearningSpaceType.Create("Laboratory"),
                    Capacity.Create(25),
                    Size.Create(85),
                    Size.Create(95),
                    Size.Create(75),
                    Colors.Create("Green"),
                    Colors.Create("White"),
                    Colors.Create("Gray")
                )
            };
        }

        /// <summary>
        /// Tests that the handler returns an OK result with the correct data when valid parameters are provided.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task HandleAsync_WithValidParameters_ReturnsOkWithPaginatedData()
        {
            
            var paginatedList = new PaginatedList<LearningSpace>(_exampleLearningSpaces, 2, _validPageSize, _validPageIndex);

            _serviceMock
                .Setup(s => s.GetLearningSpacesListPaginatedAsync(_validFloorId, _validPageSize, _validPageIndex, _searchText))
                .ReturnsAsync(paginatedList);

            // Act
            var result = await GetLearningSpacePaginatedListHandler.HandleAsync(
                _serviceMock.Object,
                _validFloorId,
                _validPageSize,
                _validPageIndex,
                _searchText);

            // Assert
            result.Result.Should().BeOfType<Ok<GetLearningSpacePaginatedListResponse>>();
            var okResult = result.Result as Ok<GetLearningSpacePaginatedListResponse>;
            okResult.Should().NotBeNull();
            okResult!.Value!.LearningSpaces.Should().HaveCount(2);
            okResult.Value.TotalCount.Should().Be(2);
            var learningSpacesList = okResult.Value.LearningSpaces.ToList();
            learningSpacesList[0].Name.Should().Be(_exampleLearningSpaces[0].Name.Name);
            learningSpacesList[1].Name.Should().Be(_exampleLearningSpaces[1].Name.Name);
        }

        /// <summary>
        /// Tests that the handler returns a BadRequest result when an invalid floor ID is provided.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task HandleAsync_WithInvalidFloorId_ReturnsBadRequest()
        {
            // Act
            var result = await GetLearningSpacePaginatedListHandler.HandleAsync(_serviceMock.Object, _invalidFloorId, _validPageSize, _validPageIndex);

            // Assert
            result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
            
            var badRequest = result.Result as BadRequest<List<ValidationError>>;
            badRequest!.Value.Should().ContainSingle(e => 
                e.Parameter == "FloorId" && 
                e.Message == "Invalid floor id format.");
        }

        /// <summary>
        /// Tests that the handler returns a BadRequest result when an invalid page size is provided.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task HandleAsync_WithInvalidPageSize_ReturnsBadRequest()
        {
            // Act
            var result = await GetLearningSpacePaginatedListHandler.HandleAsync(_serviceMock.Object, _validFloorId, _invalidPageSize, _validPageIndex);

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
            var result = await GetLearningSpacePaginatedListHandler.HandleAsync(_serviceMock.Object, _validFloorId, _validPageSize, _invalidPageIndex);

            // Assert
            result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
            
            var badRequest = result.Result as BadRequest<List<ValidationError>>;
            badRequest!.Value.Should().ContainSingle(e => 
                e.Parameter == "PageIndex" && 
                e.Message == "Page index must be greater than or equal to 0.");
        }

        /// <summary>
        /// Tests that the handler returns a NotFound result when the floor is not found.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task HandleAsync_WhenNoLearningSpacesFound_ReturnsNotFound()
        {

            _serviceMock
                .Setup(s => s.GetLearningSpacesListPaginatedAsync(_validFloorId, _validPageSize, _validPageIndex, _searchText))
                .ThrowsAsync(new NotFoundException($"Floor with Id '{_validFloorId}' not found."));

            // Act
            var result = await GetLearningSpacePaginatedListHandler.HandleAsync(
                _serviceMock.Object,
                _validFloorId,
                _validPageSize,
                _validPageIndex,
                _searchText);

            // Assert
            result.Result.Should().BeOfType<NotFound<string>>();

            var notFound = result.Result as NotFound<string>;
            notFound!.Value.Should().Be($"Floor with Id '{_validFloorId}' not found.");
        }

        /// <summary>
        /// Tests that the handler returns an OK result with an empty list when the paginated list is empty but valid.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task HandleAsync_WithEmptyPaginatedList_ReturnsOkWithEmptyList()
        {
            var emptyList = new PaginatedList<LearningSpace>(
                new List<LearningSpace>(), 0, _validPageSize, _validPageIndex);

            _serviceMock
                .Setup(s => s.GetLearningSpacesListPaginatedAsync(_validFloorId, _validPageSize, _validPageIndex, _searchText))
                .ReturnsAsync(emptyList);

            // Act
            var result = await GetLearningSpacePaginatedListHandler.HandleAsync(
                _serviceMock.Object,
                _validFloorId,
                _validPageSize,
                _validPageIndex,
                _searchText);

            // Assert
            var okResult = result.Result as Ok<GetLearningSpacePaginatedListResponse>;
            okResult!.Value!.LearningSpaces.Should().BeEmpty();
        }
    }
}
