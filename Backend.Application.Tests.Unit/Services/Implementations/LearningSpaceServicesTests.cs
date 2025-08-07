/*
Story: This task is part of the epic EPIC ID: SQL-LS-001 and is associated with PBIs #8, #10, #11, and #12.

Technical Tasks Performed:
- Implementation of unit tests
- Followed Clean Code principles
- Commented using XML documentation style
- Handled input parameters and validation

Participants (ECCI):
- *Bryan Ávila*: Implemented unit tests for reading and deleting learning spaces (ReadLearningSpacesAsync, DeleteLearningSpaceAsync).
- *Rolando Villavicencio*: Implemented unit tests for listing and deleting learning spaces (ListLearningSpacesAsync, DeleteLearningSpaceAsync).
- *Emmanuel Valenciano*: Wrote XML comments and implemented unit tests for updating learning spaces (UpdateLearningSpaceAsync).
*/

using FluentAssertions;
using Moq;
using System;
using System.Drawing;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations;

/// <summary>
/// Unit tests for the LearningSpaceServices class.
/// These tests validate how the service interacts with the repository.
/// </summary>
public class LearningSpaceServicesTests
{
    /// <summary>
    /// Mocked repository dependency for the service
    /// </summary>
    private readonly Mock<ILearningSpaceRepository> _learningSpaceRepositoryMock;

    /// <summary>
    // Service under test
    /// </summary>
    private readonly LearningSpaceServices _serviceUnderTest;

    /// <summary>
    /// Learning space entity to be used in tests.
    /// </summary>
    private readonly LearningSpace _learningSpaceToAdd;

    /// <summary>
    /// Expected list of learning spaces for testing retrieval methods.
    /// </summary>
    private readonly List<LearningSpace> _expectedSpaces;

    /// <summary>
    /// Expected paginated list of learning spaces for testing retrieval methods.
    /// </summary>
    private readonly PaginatedList<LearningSpace> _expectedPaginatedList;

    /// <summary>
    /// An empty list of learning spaces to be used in tests where no spaces are present.
    /// </summary>
    private readonly List<LearningSpace> _emptyList;

    /// <summary>
    /// An empty paginated list of learning spaces to be used in tests where no spaces are present.
    /// </summary>
    private readonly PaginatedList<LearningSpace> _emptyPaginatedList;

    /// <summary>
    /// Test floor ID used for testing retrieval methods.
    /// </summary>
    private const int _testFloorId = 1;

    /// <summary>
    /// Test learning space ID used for testing methods.
    /// </summary>
    private const int _testLearningSpaceId = 1;

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
    private const int _testTotalCount = 1;

    /// <summary>
    /// Constructor: sets up the mocked repository and the service instance.
    /// </summary>
    public LearningSpaceServicesTests()
    {
        // Strict behavior: any unconfigured call will throw
        _learningSpaceRepositoryMock = new Mock<ILearningSpaceRepository>(behavior: MockBehavior.Strict);
        _serviceUnderTest = new LearningSpaceServices(_learningSpaceRepositoryMock.Object);
        _learningSpaceToAdd = BuildValidLearningSpace();
        _expectedSpaces = new List<LearningSpace> { _learningSpaceToAdd };
        _expectedPaginatedList = new PaginatedList<LearningSpace>(
            _expectedSpaces,
            totalCount: _testTotalCount,
            pageSize: _testPageSize,
            pageIndex: _testPageIndex
        );
        _emptyList = new List<LearningSpace>();
        _emptyPaginatedList = PaginatedList<LearningSpace>.Empty(_testPageSize, _testPageIndex);
    }

    /// <summary>
    /// Helper method to build a valid LearningSpace entity using domain value objects.
    /// This ensures the entity passes all internal validations.
    /// </summary>
    private static LearningSpace BuildValidLearningSpace()
    {
        return new LearningSpace(
            EntityName.Create("Sala 1"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(40),
            Domain.ValueObjects.Spaces.Size.Create(4.5),
            Domain.ValueObjects.Spaces.Size.Create(6.0),
            Domain.ValueObjects.Spaces.Size.Create(8.0),
            Colors.Create("Red"),
            Colors.Create("White"),
            Colors.Create("Gray")
        );
    }

    /// <summary>
    /// Happy path: repository successfully creates a learning space.
    /// Service should return true.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_WithValidLearningSpace_ReturnsTrue()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd))
            .ReturnsAsync(true);

        var result = await _serviceUnderTest.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Negative path: repository fails (returns false).
    /// Service should reflect that by also returning false.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_WhenRepositoryFails_ReturnsFalse()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd))
            .ReturnsAsync(false);

        var result = await _serviceUnderTest.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Exceptional path: repository throws an exception (e.g., DB error).
    /// The service should propagate the exception unchanged.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_WhenRepositoryThrows_Throws()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd))
            .ThrowsAsync(new Exception("DB failure"));

        var act = async () => await _serviceUnderTest.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("DB failure");
    }

    /// <summary>
    /// Ensures the repository method is called exactly once during the creation.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_CallsRepositoryExactlyOnce()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd))
            .ReturnsAsync(true);

        var result = await _serviceUnderTest.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd);

        _learningSpaceRepositoryMock.Verify(
            r => r.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd),
            Times.Once,
            "the repository should be called exactly once"
        );
    }

    /// <summary>
    /// The service should return the same value returned by the repository (true or false).
    /// </summary>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CreateLearningSpaceAsync_PropagatesRepositoryResult(bool expectedResult)
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd))
            .ReturnsAsync(expectedResult);

        var result = await _serviceUnderTest.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd);

        result.Should().Be(expectedResult, "the service should forward the repository result");
    }


    /// <summary>
    /// When the repository throws a NotFoundException, the service should propagate it.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_WhenRepositoryThrowsNotFound_ThrowsNotFoundException()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd))
            .ThrowsAsync(new NotFoundException("Floor not found"));

        var act = async () => await _serviceUnderTest.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Floor not found");
    }

    /// <summary>
    /// When the repository throws a DuplicatedEntityException, the service should propagate it.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_WhenRepositoryThrowsDuplicate_ThrowsDuplicatedEntityException()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd))
            .ThrowsAsync(new DuplicatedEntityException("Learning space already exists"));

        var act = async () => await _serviceUnderTest.CreateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd);

        await act.Should().ThrowAsync<DuplicatedEntityException>()
            .WithMessage("Learning space already exists");
    }


    /// <summary>
    /// Tests that the service retrieves a learning space by its ID.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetLearningSpaceAsync_WithExistingId_ReturnsLearningSpace()
    {
        var expected = BuildValidLearningSpace();
        _learningSpaceRepositoryMock
            .Setup(r => r.ReadLearningSpaceAsync(_testLearningSpaceId))
            .ReturnsAsync(expected);

        var result = await _serviceUnderTest.GetLearningSpaceAsync(_testLearningSpaceId);

        result.Should().Be(expected);
    }

    /// <summary>
    /// Tests that the service returns null when trying to retrieve a learning space with a non-existing ID.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetLearningSpaceAsync_WithNonExistingId_ReturnsNull()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.ReadLearningSpaceAsync(_testLearningSpaceId))
            .ReturnsAsync((LearningSpace?)null);

        var result = await _serviceUnderTest.GetLearningSpaceAsync(_testLearningSpaceId);

        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that the service correctly propagates exceptions thrown by the repository.
    /// </summary>
    [Fact]
    public async Task GetLearningSpaceAsync_WhenRepositoryThrowsNotFoundException_ThrowsNotFoundException()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.ReadLearningSpaceAsync(_testLearningSpaceId))
            .ThrowsAsync(new NotFoundException("Learning space not found"));

        var act = async () => await _serviceUnderTest.GetLearningSpaceAsync(_testLearningSpaceId);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Learning space not found");
    }

    /// <summary>
    /// If repository returns true, the update operation should succeed.
    /// </summary>
    [Fact]
    public async Task UpdateLearningSpaceAsync_WhenRepositoryReturnsTrue_ShouldReturnTrue()
    {
        _learningSpaceRepositoryMock
            .Setup(repo => repo.UpdateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd))
            .ReturnsAsync(true);

        var result = await _serviceUnderTest.UpdateLearningSpaceAsync(1, _learningSpaceToAdd);

        result.Should().BeTrue(because: "the repository successfully updated the learning space");
    }

    /// <summary>
    /// If repository returns false, the service should return false indicating failure.
    /// </summary>
    [Fact]
    public async Task UpdateLearningSpaceAsync_WhenRepositoryReturnsFalse_ShouldReturnFalse()
    {
        _learningSpaceRepositoryMock
            .Setup(repo => repo.UpdateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd))
            .ReturnsAsync(false);

        var result = await _serviceUnderTest.UpdateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd);

        result.Should().BeFalse(because: "the repository was unable to update the learning space");
    }

    /// <summary>
    /// If a concurrency conflict occurs during update, the service should throw ConcurrencyConflictException.
    /// </summary>
    [Fact]
    public async Task UpdateLearningSpaceAsync_WhenConcurrencyConflict_ThrowsConcurrencyConflictException()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.UpdateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd))
            .ThrowsAsync(new ConcurrencyConflictException("Concurrency conflict occurred"));

        var act = async () => await _serviceUnderTest.UpdateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd);

        await act.Should().ThrowAsync<ConcurrencyConflictException>()
            .WithMessage("Concurrency conflict occurred");
    }

    /// <summary>
    /// If a duplicate entity is detected during update, the service should throw DuplicatedEntityException.
    /// </summary>
    [Fact]
    public async Task UpdateLearningSpaceAsync_WhenDuplicateFound_ThrowsDuplicatedEntityException()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.UpdateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd))
            .ThrowsAsync(new DuplicatedEntityException("Duplicate entity found"));

        var act = async () => await _serviceUnderTest.UpdateLearningSpaceAsync(_testLearningSpaceId, _learningSpaceToAdd);

        await act.Should().ThrowAsync<DuplicatedEntityException>()
            .WithMessage("Duplicate entity found");
    }

    /// <summary>

    /// Tests that the service retrieves a list of learning spaces for a specific floor ID.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteLearningSpaceAsync_WithExistingId_ReturnsTrue()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.DeleteLearningSpaceAsync(_testLearningSpaceId))
            .ReturnsAsync(true);

        var result = await _serviceUnderTest.DeleteLearningSpaceAsync(_testLearningSpaceId);


        result.Should().BeTrue();
    }


/// <summary>
/// Tests that the service returns the same paginated list that the repository returns
/// when the repository returns a non-empty paginated list.
/// </summary>
[Fact]
public async Task GetLearningSpacesListPaginatedAsync_WhenRepositoryReturnsNonEmptyList_ReturnsPaginatedList()
    {
        const string searchText = "Test";

        // Arrange
        _learningSpaceRepositoryMock
            .Setup(r => r.ListLearningSpacesPaginatedAsync(
                _testFloorId,
                _testPageSize,
                _testPageIndex,
                searchText))
            .ReturnsAsync(_expectedPaginatedList);

        // Act
        var result = await _serviceUnderTest.GetLearningSpacesListPaginatedAsync(
            _testFloorId,
            _testPageSize,
            _testPageIndex,
            searchText);

        // Assert
        result.Should().BeSameAs(_expectedPaginatedList,
            because: "the service should return the same paginated list returned by the repository");
    }

    /// <summary>
    /// Tests that the service returns an empty paginated list when the repository returns one.
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListPaginatedAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyPaginatedList()
    {
        const string searchText = "";

        // Arrange
        _learningSpaceRepositoryMock
            .Setup(r => r.ListLearningSpacesPaginatedAsync(
                _testFloorId,
                _testPageSize,
                _testPageIndex,
                searchText))
            .ReturnsAsync(_emptyPaginatedList);

        // Act
        var result = await _serviceUnderTest.GetLearningSpacesListPaginatedAsync(
            _testFloorId,
            _testPageSize,
            _testPageIndex,
            searchText);

        // Assert
        result.Should().BeEmpty(because: "when repository returns an empty paginated list, the paginated list returned by the service should be empty too");
    }

    /// <summary>
    /// Tests that the service propagates exceptions thrown by the repository.
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListPaginatedAsync_WhenRepositoryThrowsNotFoundException_ThrowsNotFoundException()
    {
        const string searchText = "Test";

        // Arrange
        _learningSpaceRepositoryMock
            .Setup(r => r.ListLearningSpacesPaginatedAsync(
                _testFloorId,
                _testPageSize,
                _testPageIndex,
                searchText))
            .ThrowsAsync(new NotFoundException("Floor not found"));

        // Act
        var act = async () => await _serviceUnderTest.GetLearningSpacesListPaginatedAsync(
            _testFloorId,
            _testPageSize,
            _testPageIndex,
            searchText);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Floor not found",
            because: "the service should propagate the NotFoundException from the repository");
    }

    /// <summary>
    /// Tests that the service correctly handles different page size parameters.
    /// </summary>
    /// <param name="pageSize">The page size to test</param>
    [Theory]
    [InlineData(5)]   // Small page size
    [InlineData(10)]  // Medium page size
    [InlineData(20)]  // Larger page size
    public async Task GetLearningSpacesListPaginatedAsync_WithDifferentPageSizes_CallsRepositoryWithCorrectParameter(
        int pageSize)
    {
        const string searchText = "Test";

        // Arrange
        var expectedPaginatedList = new PaginatedList<LearningSpace>(
            _expectedSpaces,
            totalCount: _testTotalCount,
            pageSize: pageSize,
            pageIndex: _testPageIndex
        );

        _learningSpaceRepositoryMock
            .Setup(r => r.ListLearningSpacesPaginatedAsync(
                _testFloorId,
                pageSize,
                _testPageIndex,
                searchText))
            .ReturnsAsync(expectedPaginatedList);

        // Act
        var result = await _serviceUnderTest.GetLearningSpacesListPaginatedAsync(
            _testFloorId,
            pageSize,
            _testPageIndex,
            searchText);

        // Assert
        result.PageSize.Should().Be(pageSize, "the page size in the result should match the requested size");
    }

    /// <summary>
    /// Tests that the service correctly handles different page index parameters.
    /// </summary>
    /// <param name="pageSize">The page size</param>
    [Theory]
    [InlineData(0)] // First page
    [InlineData(1)] // Second page
    [InlineData(2)] // Third page 
    public async Task GetLearningSpacesListPaginatedAsync_WithDifferentPageIndexes_CallsRepositoryWithCorrectParameter(
        int pageIndex)
    {
        const string searchText = "Test";

        // Arrange
        var expectedPaginatedList = new PaginatedList<LearningSpace>(
            _expectedSpaces,
            totalCount: _testTotalCount,
            pageSize: _testPageSize,
            pageIndex: pageIndex
        );

        _learningSpaceRepositoryMock
            .Setup(r => r.ListLearningSpacesPaginatedAsync(
                _testFloorId,
                _testPageSize,
                pageIndex,
                searchText))
            .ReturnsAsync(expectedPaginatedList);

        // Act
        var result = await _serviceUnderTest.GetLearningSpacesListPaginatedAsync(
            _testFloorId,
            _testPageSize,
            pageIndex,
            searchText);

        // Assert
        result.PageIndex.Should().Be(pageIndex, "the page index in the result should match the requested index");
    }

    /// <summary>
    /// Tests that the service calls the repository method with the exact same parameters
    /// </summary>
    /// <param name="pageSize">The page size parameter to test</param>
    /// <param name="pageIndex">The page index parameter to test</param>
    [Theory]
    [InlineData(5, 0)] // First page
    [InlineData(10, 1)] // Second page
    [InlineData(20, 2)] // Third page 
    public async Task GetLearningSpacesListPaginatedAsync_WithDifferentParameters_CallsRepositoryWithCorrectParameters(
        int pageSize, int pageIndex)
    {
        const string searchText = "Test";

        // Arrange
        var expectedPaginatedList = new PaginatedList<LearningSpace>(
            _expectedSpaces,
            totalCount: _testTotalCount,
            pageSize: pageSize,
            pageIndex: pageIndex
        );

        _learningSpaceRepositoryMock
            .Setup(r => r.ListLearningSpacesPaginatedAsync(
                _testFloorId,
                pageSize,
                pageIndex,
                searchText))
            .ReturnsAsync(expectedPaginatedList);

        // Act
        var result = await _serviceUnderTest.GetLearningSpacesListPaginatedAsync(
            _testFloorId,
            pageSize,
            pageIndex,
            searchText);

        // Assert
        _learningSpaceRepositoryMock.Verify(
            r => r.ListLearningSpacesPaginatedAsync(
                _testFloorId,
                pageSize,
                pageIndex,
                searchText),
            Times.Once,
            "the repository should be called with the exact pagination parameters provided"
        );
    }


    /// <summary>
    /// Tests that the service calls the repository's ListLearningSpacesAsync method
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListAsync_WhenGivenValidFloorId_CallsListOnRepository()
    {
        _learningSpaceRepositoryMock
            .Setup(r => r.ListLearningSpacesAsync(_testFloorId))
            .ReturnsAsync(_expectedSpaces);

        // Act
        var result = await _serviceUnderTest.GetLearningSpacesListAsync(_testFloorId);

        // Assert
        _learningSpaceRepositoryMock.Verify(
            r => r.ListLearningSpacesAsync(_testFloorId),
            Times.Once,
            failMessage: "Service should always call ListLearningSpacesAsync on repository when listing learning spaces for a floor");
    }

    /// <summary>
    /// Tests that the service retrieves a list of learning spaces when the repository returns a non-empty list.
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListAsync_WhenRepositoryReturnsNonEmptyList_ReturnsList()
    {
        // Arrange
        _learningSpaceRepositoryMock
            .Setup(r => r.ListLearningSpacesAsync(_testFloorId))
            .ReturnsAsync(_expectedSpaces);

        // Act
        var result = await _serviceUnderTest.GetLearningSpacesListAsync(_testFloorId);

        // Assert
        result.Should().BeSameAs(_expectedSpaces,
            because: "service should forward whatever the repository returns");
    }

    /// <summary>
    /// Tests that the service returns an empty list when the repository returns one.
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListAsync_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        _learningSpaceRepositoryMock
            .Setup(r => r.ListLearningSpacesAsync(_testFloorId))
            .ReturnsAsync(_emptyList);

        // Act
        var result = await _serviceUnderTest.GetLearningSpacesListAsync(_testFloorId);

        // Assert
        result.Should().BeEmpty(because: "when repository returns an empty list, the list returned by the service should be empty too");
    }

    /// <summary>
    /// Tests that the service returns throws an exception when the repository does not find the floor,
    /// with a specific message.
    /// </summary>
    [Fact]
    public async Task GetLearningSpacesListAsync_WhenRepositoryThrowsNotFoundException_ThrowsNotFoundException()
    {
        // Arrange
        _learningSpaceRepositoryMock
            .Setup(r => r.ListLearningSpacesAsync(_testFloorId))
            .ThrowsAsync(new NotFoundException($"Floor with id {_testFloorId} not found."));

        // Act
        var act = async () => await _serviceUnderTest.GetLearningSpacesListAsync(_testFloorId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Floor with id {_testFloorId} not found.");
    }

    /// <summary>
    /// Tests that the service calls the repository's DeleteLearningSpaceAsync method
    /// </summary>
    [Fact]
    public async Task DeleteLearningSpaceAsync_WhenGivenValidId_CallsDeleteOnRepository()
    {
        // Arrange
        _learningSpaceRepositoryMock
            .Setup(r => r.DeleteLearningSpaceAsync(_testLearningSpaceId))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.DeleteLearningSpaceAsync(_testLearningSpaceId);

        // Assert
        _learningSpaceRepositoryMock.Verify(
            r => r.DeleteLearningSpaceAsync(_testLearningSpaceId),
            Times.Once,
            failMessage: "Service should always call DeleteLearningSpaceAsync on repository when deleting a LearningSpace");
    }

    /// <summary>
    /// Tests that the service throws an exception when the repository thows an exception,
    /// with a specific message.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DeleteLearningSpaceAsync_WhenLearningSpaceNotFound_ThrowsNotFoundException()
    {
        // Arrange
        _learningSpaceRepositoryMock
            .Setup(r => r.DeleteLearningSpaceAsync(_testLearningSpaceId))
            .ThrowsAsync(new NotFoundException($"Learning space with the Id '{_testLearningSpaceId}' not found."));

        // Act
        var act = async () => await _serviceUnderTest.DeleteLearningSpaceAsync(_testLearningSpaceId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Learning space with the Id '{_testLearningSpaceId}' not found.");
    }
}
