using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services;

/// <summary>
/// Defines the contract for campus-related services.
/// </summary>
public interface ICampusServices
{
    /// <summary>
    /// Adds a new campus asynchronously.
    /// </summary>
    /// <param name="campus">The campus to add.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a value indicating whether the campus was successfully added.
    /// </returns>
    public Task<bool> AddCampusAsync(Campus campus);


    /// <summary>
    /// Retrieves a list of all existing campuses asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of campuses.
    /// </returns>
    public Task<IEnumerable<Campus>> ListCampusAsync();

    /// <summary>
    /// Deletes a campus from the database.
    /// </summary>
    /// <param name="campusName">The campus to be deleted.</param>
    /// <returns>
    /// The task result contains a boolean indicating success or failure in the delete operation.
    /// </returns>
    public Task<bool> DeleteCampusAsync(string campusName);

    /// <summary>
    /// Asynchronously retrieves a campus by its name.
    /// </summary>
    /// <param name="name">The name of the campus to retrieve. This value cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Campus"/> object if a
    /// campus with the specified name exists; otherwise, <see langword="null"/>.</returns>
    public Task<Campus?> GetByNameAsync(string name);
}
