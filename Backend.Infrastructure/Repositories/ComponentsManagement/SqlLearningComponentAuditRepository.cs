using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.ComponentsManagement;

/// <summary>
/// SQL-based implementation of <see cref="ILearningComponentAuditRepository"/> for managing
/// audit records related to learning components in the Theme Park database.
/// </summary>
internal class SqlLearningComponentAuditRepository : ILearningComponentAuditRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlLearningComponentAuditRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlLearningComponentAuditRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context for accessing audit records.</param>
    /// <param name="logger">The logger instance for logging errors and information.</param>
    public SqlLearningComponentAuditRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlLearningComponentAuditRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all learning component audit records, ordered by modification date descending.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of <see cref="LearningComponentAudit"/> records.
    /// </returns>
    public async Task<List<LearningComponentAudit>> ListLearningComponentAuditAsync()
    {
        try
        {
            return await _dbContext.LearningComponentAudits
                .OrderByDescending(a => a.ModifiedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing learning component audits");
            return new();
        }
    }

    /// <summary>
    /// Retrieves a paginated list of learning component audit records, ordered by modification date descending.
    /// </summary>
    /// <param name="pageSize">The number of records per page.</param>
    /// <param name="pageNumber">The zero-based page number to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="PaginatedList{LearningComponentAudit}"/> with the requested page of audit records.
    /// </returns>
    public async Task<PaginatedList<LearningComponentAudit>> GetPaginatedLearningComponentAuditAsync(int pageSize, int pageNumber)
    {
        try
        {
            var query = _dbContext.LearningComponentAudits
                .AsNoTracking()
                .OrderByDescending(a => a.ModifiedAt);

            var totalCount = await _dbContext.LearningComponentAudits.CountAsync();

            var audits = await query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<LearningComponentAudit>(
                audits,
                totalCount,
                pageSize,
                pageNumber
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paginated audits for page {PageNumber}", pageNumber);
            throw;
        }
    }
}
