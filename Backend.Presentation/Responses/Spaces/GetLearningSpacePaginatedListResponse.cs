using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

/// <summary>
/// Represents the response for a paginated list of learning spaces request.
/// </summary>
public record GetLearningSpacePaginatedListResponse
{
    /// <summary>
    /// Gets the collection of learning space DTOs.
    /// </summary>
    public IReadOnlyCollection<LearningSpaceListDto> LearningSpaces { get; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// The index of the current page.
    /// </summary>
    public int PageIndex { get; }

    /// <summary>
    /// Gets the total count of items across all pages.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLearningSpacePaginatedListResponse"/> class.
    /// </summary>
    /// <param name="learningSpaces">The collection of learning space DTOs.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="pageIndex">The index of the current page.</param>
    /// <param name="totalCount">The total count of items across all pages.</param>
    /// <param name="totalPages">The total number of pages.</param>
    public GetLearningSpacePaginatedListResponse(
        IReadOnlyCollection<LearningSpaceListDto> learningSpaces,
        int pageSize,
        int pageIndex,
        int totalCount,
        int totalPages)
    {
        LearningSpaces = learningSpaces;
        PageSize = pageSize;
        PageIndex = pageIndex;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }
}
