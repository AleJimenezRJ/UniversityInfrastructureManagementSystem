using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;


namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.ComponentsManagement;

/// <summary>
/// SQL implementation of the <see cref="IProjectorRepository"/> interface.
/// </summary>
internal class SqlProjectorRepository : SqlLearningComponentRepository, IProjectorRepository
{
    /// <summary>
    /// The database context used to interact with the database.
    /// </summary>
    private readonly ThemeParkDataBaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlProjectorRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger instance.</param>
    public SqlProjectorRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlLearningComponentRepository> logger)
        : base(dbContext, logger)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves all projectors in the system.
    /// </summary>
    /// <returns>A collection of projectors.</returns>
    public async Task<IEnumerable<Projector>> GetAllAsync()
    {
        return await _dbContext.Projectors
            .Where(lc => !lc.IsDeleted)
            .ToListAsync();
    }
}
