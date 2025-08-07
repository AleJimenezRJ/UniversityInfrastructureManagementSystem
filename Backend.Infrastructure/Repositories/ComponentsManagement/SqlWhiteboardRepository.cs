using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.ComponentsManagement;

/// <summary>
/// SQL implementation of the <see cref="IWhiteboardRepository"/> interface.
/// </summary>
internal class SqlWhiteboardRepository : SqlLearningComponentRepository, IWhiteboardRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlWhiteboardRepository"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public SqlWhiteboardRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlLearningComponentRepository> logger)
        : base(dbContext, logger)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves all whiteboards in the system.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Whiteboard>> GetAllAsync()
    {
        return await _dbContext.Whiteboards
            .Where(lc => !lc.IsDeleted)
            .ToListAsync();
    }
}
