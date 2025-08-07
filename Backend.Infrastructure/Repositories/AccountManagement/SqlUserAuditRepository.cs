using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Provides data access operations for user audit logs in the database.
/// </summary>
internal class SqlUserAuditRepository : IUserAuditRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlUserAuditRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlUserAuditRepository"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public SqlUserAuditRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlUserAuditRepository> logger)
    => (_dbContext, _logger) = (dbContext, logger);

    /// <summary>
    /// Retrieves a list of user audits from the database asynchronously.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    public async Task<List<UserAudit>> ListUserAuditAsync()
    {
        try
        {
            return await _dbContext.UserAudits
                .OrderByDescending(a => a.ModifiedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing user audits");
            return new();
        }
    }

    /// <summary>
    /// Retrieves a paginated list of user audit records.
    /// </summary>
    /// <param name="pageSize">The number of audit records to include in each page. Must be greater than zero.</param>
    /// <param name="pageNumber">The zero-based index of the page to retrieve. Must be greater than or equal to zero.</param>
    /// <returns>A <see cref="PaginatedList{T}"/> containing the user audit records for the specified page, along with pagination
    /// metadata.</returns>
    public async Task<PaginatedList<UserAudit>> GetPaginatedUserAuditAsync(int pageSize, int pageNumber)
    {
        try
        {
            var query = _dbContext.UserAudits
                .AsNoTracking()
                .OrderByDescending(a => a.ModifiedAt); // Ordenamos por última modificación

            var totalCount = await _dbContext.UserAudits.CountAsync();

            var audits = await query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<UserAudit>(
                audits,
                totalCount,
                pageSize,
                pageNumber
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paginated audits for page {PageNumber}", pageNumber);
            throw new DomainException($"An unexpected error occurred while retrieving user audits: {ex.Message}");
        }
    }


}
