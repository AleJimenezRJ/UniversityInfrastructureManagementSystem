using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.UniversityManagement;

/// <summary>
/// SQL Server implementation of the <see cref="IUniversityRepository"/> interface.
/// Provides methods for persisting and managing area entities in the database.
/// </summary>
internal class SqlUniversityRepository : IUniversityRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlUniversityRepository"/> class with the specified database context.
    /// </summary>
    /// <param name="dbContext">The database context for accessing and managing data.</param>
    public SqlUniversityRepository(ThemeParkDataBaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Asynchronously adds a new university entity to the database.
    /// </summary>
    /// <param name="university">The <see cref="University"/> entity to be added.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result is <c>true</c> if the university was successfully added; otherwise, <c>false</c>.
    public async Task<bool> AddUniversityAsync(University university)
    {
        var existing = await _dbContext.University
            .FirstOrDefaultAsync(u => u.Name == university.Name);

        if (existing != null)
            throw new DuplicatedEntityException($"A university with the name '{university.Name!.Name}' already exists.");

        _dbContext.University.Add(university);
        return await SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously retreives university entity from the database.
    /// </summary>
    /// <param name="name">The name of the university to be retreived</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task returns the corresponding 
    /// University entity if found.
    public async Task<University?> GetByNameAsync(string name)
    {
        if (!EntityName.TryCreate(name, out var entityName) || entityName is null)
            return null;
        return await _dbContext.University
            .FirstOrDefaultAsync(university => university.Name == entityName);
    }

    /// <summary>
    /// Retrieves a list of all universities from the database asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an
    /// <see cref="IEnumerable{University}"/> of all university entities.
    /// </returns>
    public async Task<IEnumerable<University>> ListUniversityAsync()
    {
        return await _dbContext.University.ToListAsync();
    }

    /// <summary>
    /// Deletes a university from the database by its name, if it exists.
    /// </summary>
    /// <param name="universityName">The name of the university to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is
    /// <c>true</c> if the university was successfully deleted; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> DeleteUniversityAsync(string universityName)
    {
        // Validate the university name
        if (!EntityName.TryCreate(universityName, out var validName) || validName == null)
            return false;

        // Search the university by name
        var existing = await _dbContext.University
            .FirstOrDefaultAsync(u => u.Name == validName);

        if (existing == null)
            return false;

        // Remove university
        _dbContext.University.Remove(existing);
        return await SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously saves all pending changes to the database.
    /// </summary>
    /// <returns>
    /// <c>true</c> if one or more changes were successfully saved; otherwise, <c>false</c>.
    /// </returns>
    private async Task<bool> SaveChangesAsync() =>
        await _dbContext.SaveChangesAsync() > 0;
}
