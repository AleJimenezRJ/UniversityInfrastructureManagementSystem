namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

/// <summary>
/// A generic class that provides pagination functionality for collections.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class PaginatedList<T> : List<T>
{
    /// <summary>
    /// Gets the total count of items across all pages.
    /// </summary>
    public int TotalCount { get; private set; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; private set; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Gets the zero-based index of the current page in the paginated list.
    /// </summary>
    public int PageIndex { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedList{T}"/> class.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="totalCount">The total count of items across all pages.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="pageIndex">The index of the current page</param>
    public PaginatedList(List<T> items, int totalCount, int pageSize, int pageIndex)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        AddRange(items);
    }

    /// <summary>
    /// Creates a new empty paginated list.
    /// </summary>
    /// <param name="pageSize">The page size.</param>
    /// <param name="pageIndex">The page index for pagination.</param>
    /// <returns>An empty paginated list.</returns>
    public static PaginatedList<T> Empty(int pageSize, int pageIndex)
    {
        return new PaginatedList<T>(new List<T>(), 0, pageSize, pageIndex);
    }
}