using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;

/// <summary>
/// This interface defines the operations performed related to University entity in the system.
/// </summary>
public interface IUniversityRepository
{
    /// <summary>
    ///  Asynchronously adds a new University to the data base
    /// </summary>
    /// <param name="university">The university entity to be added to the database</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result bool indicates whether the building was successfully added.
    ///</returns>
    public Task<bool> AddUniversityAsync(University university);

    /// <summary>
    /// Retrieves a university by its name
    /// </summary>
    /// <param name="Name"> Name of the university to be retrieved</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the found university with that name.
    /// </returns>
    public Task<University?> GetByNameAsync(string name);

    /// <summary>
    /// Retrieves a list of the existing universities
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that resolves to a collection of universities entities.
    /// </returns>
    public Task<IEnumerable<University>> ListUniversityAsync();

    /// <summary>
    /// Deletes a University in the data base
    /// </summary>
    /// <param name="universityName"> Name of university entity to be deleted </param>
    /// <returns>
    /// A <see cref="Task"/> that represent the deleted University if true.
    ///</returns>
    public Task<bool> DeleteUniversityAsync(string universityName);
}
