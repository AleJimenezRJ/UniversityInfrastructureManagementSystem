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
internal class SqlCampusRepository : ICampusRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlCampusRepository"/> class with the specified database context.
    /// </summary>
    /// <param name="dbContext">The database context for accessing and managing data.</param>
    public SqlCampusRepository(ThemeParkDataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Asynchronously adds a new area entity to the database.
    /// </summary>
    /// <param name="campus">The <see cref="Campus"/> entity to be added.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result is <c>true</c> if the area was successfully added; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> AddCampusAsync(Campus campus)
    {
        var existing = await _dbContext.Campus
            .FirstOrDefaultAsync(c => c.Name == campus.Name);

        if (existing != null)
            throw new DuplicatedEntityException($"A campus with the name '{campus.Name!.Name}' already exists.");

        _dbContext.Campus.Add(campus);
        return await SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously retrieves an <see cref="Campus"/> entity by its name.
    /// </summary>
    /// <param name="name">The name of the area to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// The task result is the <see cref="Campus"/> entity if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<Campus?> GetByNameAsync(string name)
    {
        if (!EntityName.TryCreate(name, out var entityName) || entityName is null)
            return null;
        return await _dbContext.Campus
            .Include(c => c.University) // Include related University entity
                .FirstOrDefaultAsync(campus => campus.Name == entityName);
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
    /// Asynchronously retrieves a list of all campus entities from the database.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// The task result is a collection of <see cref="Campus"/> entities.
    /// </returns>
    public async Task<IEnumerable<Campus>> ListCampusAsync()
    {
        return await _dbContext.Campus
            .Include(c => c.University)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously attempts to delete a campus entity by its name.
    /// </summary>
    /// <param name="campusName">The name of the campus to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result is <c>true</c> if the campus was successfully deleted; otherwise, <c>false</c>.
    /// Currently not implemented.
    /// </returns>
    public async Task<bool> DeleteCampusAsync(string campusName)
    {

        // Validate the campus name
        if (!EntityName.TryCreate(campusName, out var validName) || validName == null)
            return false;

        // Search the campus by name
        var existing = await _dbContext.Campus
            .FirstOrDefaultAsync(c => c.Name == validName);

        if (existing == null)
            return false;

        // Remove campus
        _dbContext.Campus.Remove(existing);
        return await SaveChangesAsync();
    }

}

