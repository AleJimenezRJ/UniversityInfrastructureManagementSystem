namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

/// <summary>
/// Repository interface for managing user audit records.
/// </summary>
public interface IUserAuditRepository
{
    /// <summary>
    /// Asynchronously retrieves a list of user audit records from the system.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    Task<List<UserAudit>> ListUserAuditAsync();


    /// <summary>
    /// Retrieves a paginated list of user audit records.
    /// <param name="pageSize">The maximum number of audit records to include in a single page. Must be greater than zero.</param>
    /// <param name="pageNumber">The page number to retrieve, starting from 1. Must be greater than zero.</param>
    /// <returns>A <see cref="PaginatedList{UserAudit}"/> containing the user audit records for the specified page. If no records
    /// exist for the given page, the list will be empty.</returns>
    Task<PaginatedList<UserAudit>> GetPaginatedUserAuditAsync(int pageSize, int pageNumber);
}
