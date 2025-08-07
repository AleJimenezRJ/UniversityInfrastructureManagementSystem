using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;

/// <summary>
/// SQL-based implementation of <see cref="ILearningSpaceLogRepository"/>.
/// </summary>
internal class SqlLearningSpaceLogRepository : ILearningSpaceLogRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlLearningSpaceLogRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlLearningSpaceLogRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The EF Core database context.</param>
    /// <param name="logger">The logger instance for this repository.</param>
    public SqlLearningSpaceLogRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlLearningSpaceLogRepository> logger)
        => (_dbContext, _logger) = (dbContext, logger);

    /// <summary>
    /// Retrieves a list of learning space logs from the database asynchronously.
    /// </summary>
    /// <returns>A list of <see cref="LearningSpaceLog"/> entries ordered by most recent modifications.</returns>
    public async Task<List<LearningSpaceLog>> ListLearningSpaceLogsAsync()
    {
        try
        {
            return await _dbContext.LearningSpaceLog
                .OrderByDescending(ls => ls.ModifiedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing learning space logs");
            return new();
        }
    }
}
