using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// This interface defines the operations performed related to Area entities in the system.
/// </summary>
public interface IAreaServices
{
    /// <summary>
    ///  Asynchronously adds a new area to the data base
    /// </summary>
    /// <param name="area"> Name of the area entity to be added </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result bool indicates whether the area was successfully added or not.
    ///</returns>
    public Task<bool> AddAreaAsync(Area area);

    /// <summary>
    /// Retrieves a list of all existing areas asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of areas.
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
