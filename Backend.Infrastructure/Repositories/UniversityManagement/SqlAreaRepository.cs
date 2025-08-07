using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.UniversityManagement;

/// <summary>
/// SQL Server implementation of the <see cref="IAreaRepository"/> interface.
/// Provides methods for persisting and managing area entities in the database.
/// </summary>
internal class SqlAreaRepository : IAreaRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    public readonly ICampusRepository _campusRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlAreaRepository"/> class with the specified database context.
    /// </summary>
    /// <param name="dbContext">The database context for accessing and managing data.</param>
    /// <param name="campusRepository"> The campus repository for managing campus-related data.</param>
    /// <param name="areaRepository">The area repository for managing area-related data.</param>"
    public SqlAreaRepository(ThemeParkDataBaseContext dbContext, ICampusRepository campusRepository)
    {
        _dbContext = dbContext;
        _campusRepository = campusRepository;
    }

    /// <summary>
    /// Asynchronously adds a new area entity to the database.
    /// </summary>
    /// <param name="area">The <see cref="Area"/> entity to be added.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result is <c>true</c> if the area was successfully added; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> AddAreaAsync(Area area)
    {
        if (area.Campus is null)
        {
            throw new ArgumentException("Area must include a Campus.", nameof(area));
        }

        var existing = await _dbContext.Area
            .FirstOrDefaultAsync(a => a.Name == area.Name);

        if (existing != null)
            throw new DuplicatedEntityException($"An area with the name '{area.Name!.Name}' already exists.");

        _dbContext.Area.Add(area);
        return await SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves a list of the existing areas in the database.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// The task result is a collection of <see cref="Area"/> entities.
    /// </returns>
    public async Task<IEnumerable<Area>> ListAreaAsync()
    {
        return await _dbContext.Area
            .Include(a => a.Campus)
                .ThenInclude(c => c.University)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves an <see cref="Area"/> entity by its name.
    /// </summary>
    /// <param name="name">The name of the area to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// The task result is the <see cref="Area"/> entity if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<Area?> GetByNameAsync(string name)
    {
        if (!EntityName.TryCreate(name, out var entityName) || entityName is null)
        {
            return null;
        }
        return await _dbContext.Area
            .Include(c => c.Campus)
                .ThenInclude(u => u.University)
                    .FirstOrDefaultAsync(a => a.Name == entityName);
    }

    /// <summary>
    /// Asynchronously saves all pending changes to the database.
    /// </summary>
    /// <returns>
    /// <c>true</c> if one or more changes were successfully saved; otherwise, <c>false</c>.
    /// </returns>
    private async Task<bool> SaveChangesAsync() =>
        await _dbContext.SaveChangesAsync() > 0;

    /// <summary>
    /// Asynchronously attempts to delete a area entity by its name.
    /// </summary>
    /// <param name="name">The name of the area to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result is <c>true</c> if the area was successfully deleted; otherwise, <c>false</c>.
    /// Currently not implemented.
    /// </returns>
    public async Task<bool> DeleteAreaAsync(string name)
    {

        // Validate the area name
        if (!EntityName.TryCreate(name, out var validName) || validName == null)
            return false;

        // Search the area by name
        var existing = await _dbContext.Area
            .FirstOrDefaultAsync(a => a.Name == validName);

        if (existing == null)
            return false;

        // Remove area
        _dbContext.Area.Remove(existing);
        return await SaveChangesAsync();
    }
}