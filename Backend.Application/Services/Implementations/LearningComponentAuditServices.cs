using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;

/// <summary>
/// Provides services for managing and retrieving audit records related to learning components.
/// </summary>
internal class LearningComponentAuditServices : ILearningComponentAuditServices
{
    private readonly ILearningComponentAuditRepository _learningComponentAuditRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningComponentAuditServices"/> class.
    /// </summary>
    /// <param name="learningComponentAuditRepository">
    /// The repository used to access learning component audit records.
    /// </param>
    public LearningComponentAuditServices(ILearningComponentAuditRepository learningComponentAuditRepository)
    {
        _learningComponentAuditRepository = learningComponentAuditRepository;
    }

    /// <summary>
    /// Retrieves all learning component audit records asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="LearningComponentAudit"/> records.
    /// </returns>
    public async Task<List<LearningComponentAudit>> ListLearningComponentAuditAsync()
    {
        return await _learningComponentAuditRepository.ListLearningComponentAuditAsync();
    }

    /// <summary>
    /// Retrieves a paginated list of learning component audit records asynchronously.
    /// </summary>
    /// <param name="pageSize">The number of records per page.</param>
    /// <param name="pageNumber">The zero-based page number to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedList{LearningComponentAudit}"/> with the requested page of audit records.
    /// </returns>
    public Task<PaginatedList<LearningComponentAudit>> GetPaginatedLearningComponentAuditAsync(int pageSize, int pageNumber)
    {
        return _learningComponentAuditRepository.GetPaginatedLearningComponentAuditAsync(pageSize, pageNumber);
    }
}
