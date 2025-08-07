using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Implements the <see cref="ICampusServices"/> interface, providing methods to manage campus-related operations.
/// </summary>
internal class CampusServices : ICampusServices
{
    private readonly ICampusRepository _campusRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CampusServices"/> class.
    /// </summary>
    /// <param name="campusRepository">The repository used to manage campus data.</param>
    public CampusServices(ICampusRepository campusRepository)
    {
        _campusRepository = campusRepository;
    }

    /// <summary>
    /// Adds a new campus to the data store asynchronously.
    /// </summary>
    /// <param name="campus">The campus to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result indicates whether the campus was successfully added.
    /// </returns>
    public Task<bool> AddCampusAsync(Campus campus)
    {
        return _campusRepository.AddCampusAsync(campus);
    }

    /// <summary>
    /// Retrieves a list of all existing campuses asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a collection of campuses.
    /// </returns>
    public Task<IEnumerable<Campus>> ListCampusAsync()
    {
        return _campusRepository.ListCampusAsync();
    }

    /// <summary>
    /// Deletes a campus by name asynchronously.
    /// </summary>
    /// <param name="campusName">The name of the campus to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result indicates whether the campus was successfully deleted.
    /// </returns>
    public Task<bool> DeleteCampusAsync(string campusName)
    {
        return _campusRepository.DeleteCampusAsync(campusName);
    }

    /// <summary>
    /// Asynchronously retrieves a campus by its name.
    /// </summary>
    /// <param name="name">The name of the campus to retrieve. This value cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Campus"/> object if a
    /// campus with the specified name exists; otherwise, <see langword="null"/>.</returns>
    public Task<Campus?> GetByNameAsync(string name)
    {
        return _campusRepository.GetByNameAsync(name);
    }
}
