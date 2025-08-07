using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

/// <summary>
/// Represents the response for a paginated list of floors request.
/// </summary>
public record GetFloorPaginatedListResponse
{
    /// <summary>
    /// Gets the collection of floors DTOs.
    /// </summary>
    public IReadOnlyCollection<FloorDto> Floors { get; }

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
    /// Initializes a new instance of the <see cref="GetFloorPaginatedListResponse"/> class.
    /// </summary>
    /// <param name="floors">The collection of floors DTOs.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="pageIndex">The index of the current page.</param>
    /// <param name="totalCount">The total count of items across all pages.</param>
    /// <param name="totalPages">The total number of pages.</param>
    public GetFloorPaginatedListResponse(
        IReadOnlyCollection<FloorDto> floors,
        int pageSize,
        int pageIndex,
        int totalCount,
        int totalPages)
    {
        Floors = floors;
        PageSize = pageSize;
        PageIndex = pageIndex;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }
}
