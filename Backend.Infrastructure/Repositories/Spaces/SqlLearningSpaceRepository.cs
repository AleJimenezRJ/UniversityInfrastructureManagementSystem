using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;

/// <summary>
/// Repository that implements <see cref="ILearningSpaceRepository"/> using SQL Server and Entity Framework Core.
/// Handles CRUD operations and queries for <see cref="LearningSpace"/> entities.
/// </summary>
internal class SqlLearningSpaceRepository : ILearningSpaceRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlLearningSpaceRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlLearningSpaceRepository"/> class.
    /// </summary>
    /// <param name="dbContext">EF Core database context for the Theme Park system.</param>
    /// <param name="logger">Logger for logging actions and exceptions.</param>
    public SqlLearningSpaceRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlLearningSpaceRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new learning space under the specified floor.
    /// </summary>
    /// <param name="floorId">ID of the floor where the learning space will be added.</param>
    /// <param name="learningSpace">The <see cref="LearningSpace"/> to create.</param>
    /// <returns>True if creation is successful.</returns>
    /// <exception cref="NotFoundException">If the specified floor doesn't exist.</exception>
    /// <exception cref="DuplicatedEntityException">If a learning space with the same name already exists.</exception>
    /// <exception cref="ConcurrencyConflictException">If a concurrency conflict occurs during saving.</exception>
    public async Task<bool> CreateLearningSpaceAsync(int floorId, LearningSpace learningSpace)
    {
        var floorExists = await _dbContext.Floors.AnyAsync(f => f.FloorId == floorId);
        if (!floorExists)
        {
            _logger.LogWarning("Floor with Id '{FloorId}' not found.", floorId);
            throw new NotFoundException($"Floor with Id '{floorId}' not found.");
        }

        try
        {
            _dbContext.Entry(learningSpace).Property("FloorId").CurrentValue = floorId;
            await _dbContext.LearningSpaces.AddAsync(learningSpace);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Learning space created successfully in floor {FloorId}.", floorId);
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict creating the learning space '{LearningSpaceName}' on floor '{FloorId}'.", learningSpace.Name.Name, floorId);
            throw new ConcurrencyConflictException($"Concurrency conflict creating the learning space '{learningSpace.Name.Name}' on floor '{floorId}'.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
        {
            _logger.LogWarning(ex, "Unable to create learning space '{LearningSpaceName}'. The specified floor ID '{FloorId}' does not exist.", learningSpace.Name.Name, floorId);
            throw new NotFoundException($"Unable to create learning space '{learningSpace.Name.Name}'. The specified floor ID '{floorId}' does not exist.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            _logger.LogWarning(ex, "A learning space with the name '{LearningSpaceName}' already exists on floor '{FloorId}'.", learningSpace.Name.Name, floorId);
            throw new DuplicatedEntityException($"A learning space with the name '{learningSpace.Name.Name}' already exists on this floor.");
        }
    }

    /// <summary>
    /// Updates an existing learning space with new values.
    /// </summary>
    /// <param name="learningSpaceId">ID of the learning space to update.</param>
    /// <param name="learningSpace">Updated values of the learning space.</param>
    /// <returns>True if update is successful.</returns>
    /// <exception cref="NotFoundException">If the learning space is not found.</exception>
    /// <exception cref="DuplicatedEntityException">If a name conflict occurs.</exception>
    /// <exception cref="ConcurrencyConflictException">If a concurrency conflict occurs during update.</exception>
    public async Task<bool> UpdateLearningSpaceAsync(int learningSpaceId, LearningSpace learningSpace)
    {
        var space = await _dbContext.LearningSpaces.FirstOrDefaultAsync(ls => ls.LearningSpaceId == learningSpaceId);
        if (space is null)
        {
            _logger.LogWarning("Learning space with the Id '{LearningSpaceId}' not found.", learningSpaceId);
            throw new NotFoundException($"Learning space with the Id '{learningSpaceId}' not found.");
        }

        try
        {
            space.Name = learningSpace.Name;
            space.Type = learningSpace.Type;
            space.MaxCapacity = learningSpace.MaxCapacity;
            space.Height = learningSpace.Height;
            space.Width = learningSpace.Width;
            space.Length = learningSpace.Length;
            space.ColorFloor = learningSpace.ColorFloor;
            space.ColorWalls = learningSpace.ColorWalls;
            space.ColorCeiling = learningSpace.ColorCeiling;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Learning space '{LearningSpaceName}' (Id: {LearningSpaceId}) successfully updated.", space.Name.Name, learningSpaceId);
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict updating learning space with Id '{LearningSpaceId}'.", learningSpaceId);
            throw new ConcurrencyConflictException($"Concurrency conflict updating learning space with Id '{learningSpaceId}'. It may have been updated or deleted by another user.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            _logger.LogWarning(ex, "A learning space with the name '{LearningSpaceName}' already exists on this floor.", learningSpace.Name.Name);
            throw new DuplicatedEntityException($"A learning space with the name '{learningSpace.Name.Name}' already exists on this floor.");
        }
    }

    /// <summary>
    /// Reads a single learning space by ID.
    /// </summary>
    /// <param name="learningSpaceId">ID of the learning space to read.</param>
    /// <returns>The found <see cref="LearningSpace"/> or throws if not found.</returns>
    /// <exception cref="NotFoundException">If no learning space with that ID exists.</exception>
    public async Task<LearningSpace?> ReadLearningSpaceAsync(int learningSpaceId)
    {
        var learningSpace = await _dbContext.LearningSpaces.FirstOrDefaultAsync(ls => ls.LearningSpaceId == learningSpaceId);

        if (learningSpace is null)
        {
            _logger.LogWarning("Learning Space with id '{LearningSpaceId}' not found.", learningSpaceId);
            throw new NotFoundException($"Learning Space with id '{learningSpaceId}' not found.");
        }

        return learningSpace;
    }

    /// <summary>
    /// Deletes a learning space by its ID.
    /// </summary>
    /// <param name="learningSpaceId">The ID of the learning space to delete.</param>
    /// <returns>True if deletion was successful.</returns>
    /// <exception cref="NotFoundException">If the learning space does not exist.</exception>
    /// <exception cref="ConcurrencyConflictException">If the deletion conflicted with concurrent updates.</exception>
    public async Task<bool> DeleteLearningSpaceAsync(int learningSpaceId)
    {
        var learningSpace = await _dbContext.LearningSpaces.FirstOrDefaultAsync(ls => ls.LearningSpaceId == learningSpaceId);

        if (learningSpace is null)
        {
            _logger.LogWarning("Learning space with the Id '{LearningSpaceId}' not found.", learningSpaceId);
            throw new NotFoundException($"Learning space with the Id '{learningSpaceId}' not found.");
        }

        try
        {
            _dbContext.LearningSpaces.Remove(learningSpace);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict deleting learning space with the Id '{LearningSpaceId}'.", learningSpaceId);
            throw new ConcurrencyConflictException($"Concurrency conflict deleting learning space with the Id '{learningSpaceId}'. It may have been updated or deleted by another user.");
        }
    }

    /// <summary>
    /// Lists all learning spaces for a given floor.
    /// </summary>
    /// <param name="floorId">The floor ID to filter learning spaces.</param>
    /// <returns>A list of <see cref="LearningSpace"/> entities on the specified floor.</returns>
    /// <exception cref="NotFoundException">If the floor does not exist.</exception>
    public async Task<List<LearningSpace>?> ListLearningSpacesAsync(int floorId)
    {
        var floorExists = await _dbContext.Floors.AnyAsync(f => f.FloorId == floorId);

        if (!floorExists)
        {
            _logger.LogWarning("Floor with Id '{FloorId}' not found.", floorId);
            throw new NotFoundException($"Floor with Id '{floorId}' not found.");
        }

        try
        {
            var learningSpaces = await _dbContext.LearningSpaces
                .Where(ls => EF.Property<int>(ls, "FloorId") == floorId)
                .ToListAsync();

            return learningSpaces;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Database update error while listing learning spaces for floorId '{FloorId}'.", floorId);
            throw new DbUpdateException($"Database update error while listing learning spaces for floorId {floorId}.");
        }
    }

    /// <summary>
    /// Lists learning spaces for a floor with pagination.
    /// </summary>
    /// <param name="floorId">The floor ID to filter by.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageIndex">The index of the page to retrieve (zero-based).</param>
    /// <param name="searchText">Optional search text to filter learning spaces by name or type.</param>
    /// <returns>A paginated list of <see cref="LearningSpace"/> entities.</returns>
    /// <exception cref="NotFoundException">If the floor does not exist.</exception>
    public async Task<PaginatedList<LearningSpace>> ListLearningSpacesPaginatedAsync(
    int floorId, int pageSize, int pageIndex, string searchText)
{
    try
    {
        var floorExists = await _dbContext.Floors.AnyAsync(f => f.FloorId == floorId);
        if (!floorExists)
        {
            _logger.LogWarning("Floor with Id '{FloorId}' not found.", floorId);
            throw new NotFoundException($"Floor with Id '{floorId}' not found.");
        }

        // Base query to get learning spaces for the specified floor
        var query = _dbContext.LearningSpaces
            .AsNoTracking()
            .Where(ls => EF.Property<int>(ls, "FloorId") == floorId);

        // Get total count before applying search filter
        var totalCount = await query.CountAsync();

        // Get all items for this floor to perform filtering in memory
        var learningSpaces = await query.ToListAsync();

        // Apply search filter in memory
        var filteredSpaces = learningSpaces;
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            filteredSpaces = learningSpaces
                .Where(ls => ls.Name.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) || 
                            ls.Type.Value.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Order by name
        filteredSpaces = filteredSpaces
            .OrderBy(ls => ls.Name.Name)
            .ToList();

        // Calculate filtered count
        var filteredCount = filteredSpaces.Count;

        // Apply pagination in memory
        var paginatedItems = filteredSpaces
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedList<LearningSpace>(paginatedItems, filteredCount, pageSize, pageIndex);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving paginated learning spaces for floor {FloorId}.", floorId);
        throw;
    }
}

}
