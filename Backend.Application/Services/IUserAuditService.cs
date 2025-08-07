using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services;

/// <summary>
/// Defines the contract for user audit services.
/// </summary>
public interface IUserAuditService
{
    /// <summary>
    /// Records an audit entry for a user action.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation, containing a boolean indicating success or failure.
    /// </returns>
    public Task<List<UserAudit>> ListUserAuditAsync();

    /// <summary>
    /// A method to retrieve a paginated list of user audits.
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="pageNumber"></param>
    /// <returns>
    /// A list of <see cref="UserAudit"/> entries wrapped in a <see cref="PaginatedList{T}"/> object.
    /// </returns>
    Task<PaginatedList<UserAudit>> GetPaginatedUserAuditAsync(int pageSize, int pageNumber);
}
