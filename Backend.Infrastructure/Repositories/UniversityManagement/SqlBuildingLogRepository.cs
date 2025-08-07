using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;


namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.UniversityManagement;

/// <summary>
/// SQL-based implementation of <see cref="IBuildingLogRepository"/> 
/// </summary>
internal class SqlBuildingLogRepository : IBuildingLogRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlBuildingLogRepository> _logger;


    public SqlBuildingLogRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlBuildingLogRepository> logger)
    => (_dbContext, _logger) = (dbContext, logger);


    /// <summary>
    /// Retrieves a list of building logs from the database asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task<List<BuildingLog>> ListBuildingLogsAsync()
    {
        try
        {
            return await _dbContext.BuildingLog
                .OrderByDescending(bl => bl.ModifiedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing building logs");
            return new();
        }
    }
}