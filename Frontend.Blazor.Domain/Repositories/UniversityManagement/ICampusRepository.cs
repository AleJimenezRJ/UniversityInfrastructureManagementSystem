using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;

/// <summary>
/// This interface defines the operations performed related to Campus entities in the system.
/// </summary>
public interface ICampusRepository
{
    /// <summary>
    ///  Asynchronously adds a new Campus to the data base
    /// </summary>
    /// <param name="campus"> Name of Campus entity to be added </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result bool indicates whether the Campus was successfully added.
    ///</returns>
    public Task<bool> AddCampusAsync(Campus campus);

    /// <summary>
    /// Retrieves a list of the existing campus
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that resolves to a collection of campus entities.
    /// </returns>
    public Task<IEnumerable<Campus>> ListCampusAsync();

    /// <summary>
    /// Deletes a campus in the data base
    /// </summary>
    /// <param name="campusName"> Name of campus entity to be deleted </param>
    /// <returns>
    /// A <see cref="Task"/> that represent the deleted campus if true.
    ///</returns>
    public Task<bool> DeleteCampusAsync(string campusName);

    /// <summary>
    /// Asynchronously retrieves an <see cref="Campus"/> entity by its name.
    /// </summary>
    /// <param name="name">The name of the area to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// The task result is the <see cref="Campus"/> entity if found; otherwise, <c>null</c>.
    /// </returns>
    public Task<Campus?> GetByNameAsync(string name);
}