using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response for a paginated list of user audit records.
/// </summary>
public record GetUserAuditPaginatedListResponse
{
    /// <summary>
    /// Gets the collection of user audit DTOs.
    /// </summary>
    public IReadOnlyCollection<UserAuditDto> Audits { get; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// Gets the total count of items across all pages.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserAuditPaginatedListResponse"/> class.
    /// </summary>
    /// <param name="audits">The collection of user audit DTOs.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="totalCount">The total count of items across all pages.</param>
    /// <param name="totalPages">The total number of pages.</param>
    public GetUserAuditPaginatedListResponse(
        IReadOnlyCollection<UserAuditDto> audits,
        int pageSize,
        int pageNumber,
        int totalCount,
        int totalPages)
    {
        Audits = audits;
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }
}
