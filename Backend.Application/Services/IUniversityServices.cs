using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services;

/// <summary>
/// Defines the contract for university-related services.
/// </summary>
public interface IUniversityServices
{
    /// <summary>
    /// Adds a new university asynchronously.
    /// </summary>
    /// <param name="university">The university to add.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a value indicating whether the university was successfully added.
    /// </returns>
    public Task<bool> AddUniversityAsync(University university);

    /// <summary>
    /// Retrieves a single university and its information asynchronously.
    /// </summary>
    /// <param name="name">The name of the university to be retreived</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the university with the given name.
    /// </returns>
    public Task<University?> GetByNameAsync(string name);

    /// <summary>
    /// Retrieves a list of all existing universities asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of universities.
    /// </returns>
    public Task<IEnumerable<University>> ListUniversityAsync();

    /// <summary>
    /// Deletes a university from the database.
    /// </summary>
    /// <param name="universityName">The name of the university to be deleted.</param>
    /// <returns>
    /// The task result contains a boolean indicating success or failure in the delete operation.
    /// </returns>
    public Task<bool> DeleteUniversityAsync(string universityName);
}
