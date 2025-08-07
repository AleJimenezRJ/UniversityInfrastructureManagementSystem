using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

public record GetBuildingPaginatedListResponse
{
    /// <summary>
    /// Gets the collection of building DTOs.
    /// </summary>
    public IReadOnlyCollection<ListBuildingDto> Buildings { get; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets the last ID of the items in the current page.
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
    /// Initializes a new instance of the <see cref="GetBuildingPaginatedListResponse"/> class.
    /// </summary>
    /// <param name="buildings">The collection of building DTOs.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="pageIndex">The page index for pagination.</param>    /// <param name="totalCount">The total count of items across all pages.</param>
    /// <param name="totalPages">The total number of pages.</param>
    public GetBuildingPaginatedListResponse(
        IReadOnlyCollection<ListBuildingDto> buildings,
        int pageSize,
        int pageIndex,
        int totalCount,
        int totalPages)
    {
        Buildings = buildings;
        PageSize = pageSize;
        PageIndex = pageIndex;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }
}