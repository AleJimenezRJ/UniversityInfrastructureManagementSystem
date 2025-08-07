using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Responses;

/// <summary>
/// Provides unit tests for the <see cref="GetFloorPaginatedListResponse"/> class,  ensuring that its properties are
/// correctly initialized and behave as expected.
/// </summary>
public class GetFloorPaginatedListResponseTests
{
    /// <summary>
    /// Represents a collection of floor data transfer objects.
    /// </summary>
    private readonly List<FloorDto> _floors;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFloorPaginatedListResponseTests"/> class.
    /// </summary>
    public GetFloorPaginatedListResponseTests()
    {
        _floors = [
            new FloorDto(1, 1),
            new FloorDto(2, 2),
            new FloorDto(3, 3)
        ];
    }

    /// <summary>
    /// Tests that the <see cref="GetFloorPaginatedListResponse"/> constructor initializes the Floors property correctly
    /// when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldInitializeFloors()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _floors.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response = new GetFloorPaginatedListResponse(
            _floors.AsReadOnly(),
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response.Floors.Should().BeEquivalentTo(_floors, because: "the Floors property should match the provided collection");
    }

    /// <summary>
    /// Tests that the constructor of <see cref="GetFloorPaginatedListResponse"/> correctly sets the <c>PageSize</c>
    /// property when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldSetPageSize()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _floors.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response = new GetFloorPaginatedListResponse(
            _floors.AsReadOnly(),
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response.PageSize.Should().Be(pageSize, because: "the PageSize property should match the provided value");
    }

    /// <summary>
    /// Tests that the constructor of <see cref="GetFloorPaginatedListResponse"/> correctly sets the <c>PageIndex</c>
    /// property when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldSetPageIndex()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _floors.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response = new GetFloorPaginatedListResponse(
            _floors.AsReadOnly(),
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response.PageIndex.Should().Be(pageIndex, because: "the PageIndex property should match the provided value");
    }

    /// <summary>
    /// Tests that the <see cref="GetFloorPaginatedListResponse"/> constructor correctly sets the <c>TotalCount</c>
    /// property when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldSetTotalCount()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _floors.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response = new GetFloorPaginatedListResponse(
            _floors.AsReadOnly(),
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response.TotalCount.Should().Be(totalCount, because: "the TotalCount property should match the provided value");
    }

    /// <summary>
    /// Tests that the constructor of <see cref="GetFloorPaginatedListResponse"/> correctly sets the <c>TotalPages</c>
    /// property when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldSetTotalPages()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _floors.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response = new GetFloorPaginatedListResponse(
            _floors.AsReadOnly(),
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response.TotalPages.Should().Be(totalPages, because: "the TotalPages property should match the calculated value");
    }

    /// <summary>
    /// Verifies that two instances of <see cref="GetFloorPaginatedListResponse"/> created with the same parameters are
    /// equivalent.
    /// </summary>
    [Fact]
    public void Constructor_WhenTwoResponsesAreCreatedWithSameParameters_ShouldBeEqual()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _floors.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response1 = new GetFloorPaginatedListResponse(
            _floors.AsReadOnly(),
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        var response2 = new GetFloorPaginatedListResponse(
            _floors.AsReadOnly(),
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response1.Should().BeEquivalentTo(response2, because: "two responses with the same parameters should be equal");
    }
}
