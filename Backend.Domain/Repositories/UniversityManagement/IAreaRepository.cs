using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;

/// <summary>
/// Defines the contract for repository operations related to <see cref="Area"/> entities.
/// Provides abstraction for persistence operations involving areas.
/// </summary>
public interface IAreaRepository
{
    /// <summary>
    /// Asynchronously adds a new <see cref="Area"/> to the database.
    /// </summary>
    /// <param name="area">The <see cref="Area"/> entity to be added.</param>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// The task result is <c>true</c> if the area was successfully added; otherwise, <c>false</c>.
    /// </returns>
    public Task<bool> AddAreaAsync(Area area);

    /// <summary>
    /// Retrieves a list of the existing areas in the database.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that resolves to a collection of area entities.
    /// </returns>
    public Task<IEnumerable<Area>> ListAreaAsync();

    /// <summary>
    /// Asynchronously retrieves an <see cref="Area"/> entity by its name.
    /// </summary>
    /// <param name="name">The name of the area to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// The task result is the <see cref="Area"/> entity if found; otherwise, <c>null</c>.
    /// </returns>
    /// 
    public Task<Area?> GetByNameAsync(string name);

    /// <summary>
    /// Deletes an area in the data base
    /// </summary>
    /// <param name="name"> Name of the area entity to be deleted </param>
    /// <returns>
    /// A <see cref="Task"/> that represent the deleted area if true.
    ///</returns>
    public Task<bool> DeleteAreaAsync(string name);
}
