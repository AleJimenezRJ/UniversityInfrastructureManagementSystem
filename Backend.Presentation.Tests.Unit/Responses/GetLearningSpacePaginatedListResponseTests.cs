using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Responses;

/// <summary>
/// Provides unit tests for the <see cref="GetLearningSpacePaginatedListResponse"/> class, ensuring that its properties
/// are correctly initialized and behave as expected.
/// </summary>
public class GetLearningSpacePaginatedListResponseTests
{
    /// <summary>
    /// Represents a collection of learning spaces.
    /// </summary>
    private readonly IReadOnlyCollection<LearningSpaceListDto> _learningSpaces;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLearningSpacePaginatedListResponseTests"/> class.
    /// </summary>
    public GetLearningSpacePaginatedListResponseTests()
    {
        _learningSpaces = new List<LearningSpaceListDto>
        {
            new LearningSpaceListDto(1, "Space 1", "Description 1"),
            new LearningSpaceListDto(2, "Space 2", "Description 2"),
            new LearningSpaceListDto(3, "Space 3", "Description 3")
        };
    }

    /// <summary>
    /// Tests that the constructor of <see cref="GetLearningSpacePaginatedListResponse"/> initializes the <see
    /// cref="GetLearningSpacePaginatedListResponse.LearningSpaces"/> property correctly when valid parameters are
    /// provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldInitializeLearningSpaces()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _learningSpaces.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response = new GetLearningSpacePaginatedListResponse(
            _learningSpaces,
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response.LearningSpaces.Should().BeEquivalentTo(_learningSpaces, because: "the LearningSpaces property should match the provided collection");
    }

    /// <summary>
    /// Tests that the constructor of <see cref="GetLearningSpacePaginatedListResponse"/> correctly sets the
    /// <c>PageSize</c> property when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldSetPageSize()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _learningSpaces.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response = new GetLearningSpacePaginatedListResponse(
            _learningSpaces,
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response.PageSize.Should().Be(pageSize, because: "the PageSize property should match the provided value");
    }

    /// <summary>
    /// Tests that the constructor of <see cref="GetLearningSpacePaginatedListResponse"/> correctly sets the
    /// <c>PageIndex</c> property when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldSetPageIndex()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _learningSpaces.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response = new GetLearningSpacePaginatedListResponse(
            _learningSpaces,
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response.PageIndex.Should().Be(pageIndex, because: "the PageIndex property should match the provided value");
    }

    /// <summary>
    /// Verifies that the constructor of <see cref="GetLearningSpacePaginatedListResponse"/> correctly sets the
    /// <c>TotalCount</c> property when valid parameters are provided.
    /// </summary>
    [Fact] 
    public void Constructor_WhenValidParametersProvided_ShouldSetTotalCount()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _learningSpaces.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response = new GetLearningSpacePaginatedListResponse(
            _learningSpaces,
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response.TotalCount.Should().Be(totalCount, because: "the TotalCount property should match the provided value");
    }

    /// <summary>
    /// Tests that the constructor of <see cref="GetLearningSpacePaginatedListResponse"/> correctly sets the
    /// <c>TotalPages</c> property when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldSetTotalPages()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _learningSpaces.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response = new GetLearningSpacePaginatedListResponse(
            _learningSpaces,
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response.TotalPages.Should().Be(totalPages, because: "the TotalPages property should match the calculated value");
    }

    /// <summary>
    /// Verifies that two instances of <see cref="GetLearningSpacePaginatedListResponse"/>  created with the same
    /// parameters are considered equal.
    /// </summary>
    [Fact]
    public void Constructor_WhenTwoResponsesAreCreatedWithSameParameters_ShouldBeEqual()
    {
        // Arrange
        int pageSize = 10;
        int pageIndex = 1;
        int totalCount = _learningSpaces.Count;
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Act
        var response1 = new GetLearningSpacePaginatedListResponse(
            _learningSpaces,
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        var response2 = new GetLearningSpacePaginatedListResponse(
            _learningSpaces,
            pageSize,
            pageIndex,
            totalCount,
            totalPages);

        // Assert
        response1.Should().BeEquivalentTo(response2, because: "two responses with the same parameters should be equal");
    }
}
