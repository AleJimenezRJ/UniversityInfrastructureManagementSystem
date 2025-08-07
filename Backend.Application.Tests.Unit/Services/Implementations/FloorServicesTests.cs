using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations;

/*
Story: This task corresponds to the epic EPIC ID: SQL-FL-001 and is associated with PBIs #109, #110 and #111.

Technical Tasks Performed:
- Implementation of unit tests
- Follow Clean Code Principles
- Commented using XML style
- Handle Input Parameters and Validation

Participants:
    - Anderson Vargas
    - Keylor Palacios
*/

public class FloorServicesTests
{
    /// <summary>
    /// Mock repository for testing the FloorServices class.
    /// </summary>
    private readonly Mock<IFloorRepository> _floorRepositoryMock;

    /// <summary>
    /// Instance of the FloorServices class under test.
    /// </summary>
    private readonly FloorServices _serviceUnderTest;

    /// <summary>
    /// An empty list of floors to be used in tests where no floors are present.
    /// </summary>
    private readonly List<Floor> _emptyFloorList;

    /// <summary>
    /// A non-empty list of floors to be used in tests where floors are present.
    /// </summary>
    private readonly List<Floor> _nonEmptyFloorList;

    /// <summary>
    /// A valid building ID to be used in tests that require a valid building context.
    /// </summary>
    private readonly int _buildingId;

    /// <summary>
    /// An invalid building ID to be used in tests that require an invalid building context.
    /// </summary>
    private readonly int _invalidBuildingId;

    /// <summary>
    /// Expected paginated list of floors for testing retrieval methods.
    /// </summary>
    private readonly PaginatedList<Floor> _expectedPaginatedList;

    /// <summary>
    /// An empty paginated list of floors to be used in tests where no floors are present.
    /// </summary>
    private readonly PaginatedList<Floor> _emptyPaginatedList;

    /// <summary>
    /// Test Page Size constant for pagination parameters
    /// </summary>
    private const int _testPageSize = 10;

    /// <summary>
    /// Test Page Index constant for pagination parameters
    /// </summary>
    private const int _testPageIndex = 0;

    /// <summary>
    /// Test total count constant for pagination parameters
    /// </summary>
    private const int _testTotalCount = 2;

    /// <summary>
    /// Initializes a new instance of the <see cref="FloorServicesTests"/> class.
    /// </summary>
    public FloorServicesTests()
    {
        _floorRepositoryMock = new Mock<IFloorRepository>(behavior: MockBehavior.Strict);
        _serviceUnderTest = new FloorServices(_floorRepositoryMock.Object);
        _emptyFloorList = new List<Floor>();
        _nonEmptyFloorList = new List<Floor>
        {
            new Floor(floorId: 1, FloorNumber.Create(1)),
            new Floor(floorId: 1, FloorNumber.Create(2))
        };
        _buildingId = 1;
        _invalidBuildingId = -1;
         _expectedPaginatedList = new PaginatedList<Floor>(
            _nonEmptyFloorList,
            totalCount: _testTotalCount,
            pageSize: _testPageSize,
            pageIndex: _testPageIndex
        );
        _emptyPaginatedList = PaginatedList<Floor>.Empty(_testPageSize, _testPageIndex);
    }

    /// <summary>
    /// Tests the GetFloorsListAsync method when the repository returns an empty list.
    /// </summary>
    [Fact]
    public async Task GetFloorsListAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.ListFloorsAsync(_buildingId))
            .ReturnsAsync(_emptyFloorList);

        // Act
        var resultList = await _serviceUnderTest.GetFloorsListAsync(_buildingId);

        // Assert
        resultList.Should().BeSameAs(_emptyFloorList,
            because: "service should forward whatever the repository returns");
    }

    /// <summary>
    /// Tests the GetFloorsListAsync method when the repository returns a non-empty list.   
    /// </summary>
    [Fact]
    public async Task GetFloorsListAsync_WhenRepositoryReturnsNonEmptyList_ReturnsNonEmptyList()
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.ListFloorsAsync(_buildingId))
            .ReturnsAsync(_nonEmptyFloorList);

        // Act
        var resultList = await _serviceUnderTest.GetFloorsListAsync(_buildingId);

        // Assert
        resultList.Should().BeSameAs(_nonEmptyFloorList,
            because: "service should forward whatever the repository returns");
    }

    /// <summary>
    /// Tests the AddFloorAsync method when given valid parameters.
    /// </summary>
    [Fact]
    public async Task AddFloorAsync_WhenGivenValidParameters_CallsCreateOnRepository()
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.CreateFloorAsync(_buildingId))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.CreateFloorAsync(_buildingId);

        // Assert
        _floorRepositoryMock.Verify(
            floorRepository => floorRepository.CreateFloorAsync(_buildingId),
            Times.Once,
            failMessage: "Service should always call CreateFloorAsync on the repository to create a new floor");
    }

    /// <summary>
    /// Tests the AddFloorAsync method when given invalid parameters.
    /// </summary>
    [Fact]
    public async Task AddFloorAsync_WhenGivenInvalidParameters_CallsCreateOnRepository()
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.CreateFloorAsync(_invalidBuildingId))
            .ReturnsAsync(false);

        // Act
        var result = await _serviceUnderTest.CreateFloorAsync(_invalidBuildingId);

        // Assert
        _floorRepositoryMock.Verify(
            floorRepository => floorRepository.CreateFloorAsync(_invalidBuildingId),
            Times.Once,
            failMessage: "Service should call CreateFloorAsync on the repository even with invalid parameters");
    }

    /// <summary>
    /// Tests the AddFloorAsync method when given valid parameters and checks the returned result.
    /// </summary>
    /// <param name="expectedResult">The possible result from the repository.</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AddFloorAsync_WhenGivenValidParameters_ReturnsResultFromRepository(bool expectedResult)
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.CreateFloorAsync(_invalidBuildingId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _serviceUnderTest.CreateFloorAsync(_invalidBuildingId);

        // Assert
        result.Should().Be(expectedResult,
            because: "service should return whatever the repository returns");
    }

    /// <summary>
    /// Tests the DeleteFloorAsync method when given a valid building ID.
    /// </summary>
    [Fact]
    public async Task DeleteFloorAsync_WhenGivenValidFloorId_CallsDeleteOnRepository()
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.DeleteFloorAsync(_buildingId))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.DeleteFloorAsync(_buildingId);

        // Assert
        _floorRepositoryMock.Verify(
            floorRepository => floorRepository.DeleteFloorAsync(_buildingId),
            Times.Once,
            failMessage: "Service should call DeleteFloorAsync on the repository for a valid floor ID");
    }

    /// <summary>
    /// Tests the DeleteFloorAsync method when given an invalid building ID.
    /// </summary>
    [Fact]
    public async Task DeleteFloorAsync_WhenGivenInvalidFloorId_CallsDeleteOnRepository()
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.DeleteFloorAsync(_invalidBuildingId))
            .ReturnsAsync(false);

        // Act
        var result = await _serviceUnderTest.DeleteFloorAsync(_invalidBuildingId);

        // Assert
        _floorRepositoryMock.Verify(
            floorRepository => floorRepository.DeleteFloorAsync(_invalidBuildingId),
            Times.Once,
            failMessage: "Service should call DeleteFloorAsync on the repository even with invalid floor ID");
    }

    /// <summary>
    /// Tests the DeleteFloorAsync method when given valid parameters and checks the returned result.
    /// </summary>
    /// <param name="expectedResult">The possible result from the repository.</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteFloorAsync_WhenGivenValidParameters_ReturnsResultFromRepository(bool expectedResult)
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.DeleteFloorAsync(_invalidBuildingId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _serviceUnderTest.DeleteFloorAsync(_invalidBuildingId);

        // Assert
        result.Should().Be(expectedResult,
            because: "service should return whatever the repository returns");
    }

    /// <summary>
    /// Tests the GetFloorsListPaginatedAsync method when the repository returns an empty paginated list.
    /// </summary>
    [Fact]
    public async Task GetFloorsListPaginatedAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyPaginatedList()
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.ListFloorsPaginatedAsync(
                _buildingId, 
                _testPageSize, 
                _testPageIndex))
            .ReturnsAsync(_emptyPaginatedList);

        // Act
        var result = await _serviceUnderTest.GetFloorsListPaginatedAsync(
            _buildingId, 
            _testPageSize, 
            _testPageIndex);

        // Assert
        result.Should().BeEmpty("the paginated list should be empty");
    }

    /// <summary>
    /// Tests the GetFloorsListPaginatedAsync method when the repository returns a non-empty paginated list.
    /// </summary>
    [Fact]
    public async Task GetFloorsListPaginatedAsync_WhenRepositoryReturnsNonEmptyList_ReturnsPaginatedList()
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.ListFloorsPaginatedAsync(
                _buildingId, 
                _testPageSize, 
                _testPageIndex))
            .ReturnsAsync(_expectedPaginatedList);

        // Act
        var result = await _serviceUnderTest.GetFloorsListPaginatedAsync(
            _buildingId, 
            _testPageSize, 
            _testPageIndex);

        // Assert
        result.Should().BeSameAs(_expectedPaginatedList,
            because: "service should return the same paginated list returned by the repository");
    }

    /// <summary>
    /// Tests that the GetFloorsListPaginatedAsync method calls the repository with correct parameters.
    /// </summary>
    [Fact]
    public async Task GetFloorsListPaginatedAsync_WhenGivenValidBuildingId_CallsListPaginatedOnRepository()
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.ListFloorsPaginatedAsync(
                _buildingId, 
                _testPageSize, 
                _testPageIndex))
            .ReturnsAsync(_expectedPaginatedList);

        // Act
        var result = await _serviceUnderTest.GetFloorsListPaginatedAsync(
            _buildingId, 
            _testPageSize, 
            _testPageIndex);

        // Assert
        _floorRepositoryMock.Verify(
            floorRepository => floorRepository.ListFloorsPaginatedAsync(
                _buildingId, 
                _testPageSize, 
                _testPageIndex),
            Times.Once,
            "the repository's paginated list method should be called exactly once with the correct parameters");
    }

    /// <summary>
    /// Tests that the service propagates exceptions thrown by the repository.
    /// </summary>
    [Fact]
    public async Task GetFloorsListPaginatedAsync_WhenRepositoryThrowsNotFoundException_ThrowsNotFoundException()
    {
        // Arrange
        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.ListFloorsPaginatedAsync(
                _buildingId, 
                _testPageSize, 
                _testPageIndex))
            .ThrowsAsync(new NotFoundException($"Building with id {_buildingId} does not exist."));

        // Act
        var act = async () => await _serviceUnderTest.GetFloorsListPaginatedAsync(
            _buildingId, 
            _testPageSize, 
            _testPageIndex);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Building with id {_buildingId} does not exist.",
            "the service should propagate the NotFoundException from the repository");
    }
    /// <summary>
    /// Tests that the service correctly handles different page sizes.
    /// </summary>
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public async Task GetFloorsListPaginatedAsync_WithDifferentPageSizes_CorrectlyPropagatesPageSize(int pageSize)
    {
        // Arrange
        var customPaginatedList = new PaginatedList<Floor>(
            _emptyFloorList,
            totalCount: _testTotalCount,
            pageSize: pageSize,
            pageIndex: _testPageIndex
        );

        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.ListFloorsPaginatedAsync(
                _buildingId,
                pageSize,
                _testPageIndex))
            .ReturnsAsync(customPaginatedList);

        // Act
        var result = await _serviceUnderTest.GetFloorsListPaginatedAsync(
            _buildingId,
            pageSize,
            _testPageIndex);

        // Assert
        result.PageSize.Should().Be(pageSize, "the page size in the result should match the requested size");
    }

    /// <summary>
    /// Tests that the service correctly handles different page indexes.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetFloorsListPaginatedAsync_WithDifferentPageIndexes_CorrectlyPropagatesPageIndex(int pageIndex)
    {
        // Arrange
        var customPaginatedList = new PaginatedList<Floor>(
            _emptyFloorList,
            totalCount: _testTotalCount,
            pageSize: _testPageSize,
            pageIndex: pageIndex
        );

        _floorRepositoryMock
            .Setup(floorRepository => floorRepository.ListFloorsPaginatedAsync(
                _buildingId,
                _testPageSize,
                pageIndex))
            .ReturnsAsync(customPaginatedList);

        // Act
        var result = await _serviceUnderTest.GetFloorsListPaginatedAsync(
            _buildingId,
            _testPageSize,
            pageIndex);

        // Assert
        result.PageIndex.Should().Be(pageIndex, "the page index in the result should match the requested index");
    }

    
}
