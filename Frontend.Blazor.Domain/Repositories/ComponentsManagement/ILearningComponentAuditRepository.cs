using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;

/// <summary>
/// Defines the contract for accessing and managing audit records related to learning components.
/// </summary>
public interface ILearningComponentAuditRepository
{
    /// <summary>
    /// Retrieves all learning component audit records asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="LearningComponentAudit"/> records.
    /// </returns>
    Task<List<LearningComponentAudit>> ListLearningComponentAuditAsync();

    /// <summary>
    /// Retrieves a paginated list of learning component audit records asynchronously.
    /// </summary>
    /// <param name="pageSize">The number of records per page.</param>
    /// <param name="pageNumber">The zero-based page number to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedList{LearningComponentAudit}"/> with the requested page of audit records.
    /// </returns>
    Task<PaginatedList<LearningComponentAudit>> GetPaginatedLearningComponentAuditAsync(int pageSize, int pageNumber);
}
