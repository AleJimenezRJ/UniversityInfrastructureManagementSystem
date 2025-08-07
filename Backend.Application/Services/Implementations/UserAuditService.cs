using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
/// <summary>
/// Provides implementation for user audit-related operations defined in <see cref="IUserAuditService"/>.
/// </summary>
internal class UserAuditService : IUserAuditService
{
    private readonly IUserAuditRepository _userAuditRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAuditService"/> class.
    /// </summary>
    /// <param name="userAuditRepository"></param>
    public UserAuditService(IUserAuditRepository userAuditRepository)
    {
        _userAuditRepository = userAuditRepository;
    }

    /// <summary>
    /// Retrieves a list of user audits asynchronously.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    public async Task<List<UserAudit>> ListUserAuditAsync()
    {
        return await _userAuditRepository.ListUserAuditAsync();
    }

    /// <summary>
    /// Retrieves a paginated list of user audit records.
    /// </summary>
    /// <param name="pageSize">The maximum number of records to include in a single page. Must be greater than zero.</param>
    /// <param name="pageNumber">The page number to retrieve, starting from 1. Must be greater than zero.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains a  <see
    /// cref="PaginatedList{T}"/> of <see cref="UserAudit"/> objects for the specified page.</returns>
    public Task<PaginatedList<UserAudit>> GetPaginatedUserAuditAsync(int pageSize, int pageNumber)
    {
        return _userAuditRepository.GetPaginatedUserAuditAsync(pageSize, pageNumber);
    }



}

