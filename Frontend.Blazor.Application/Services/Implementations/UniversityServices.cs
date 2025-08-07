using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Implements the <see cref="IUniversityServices"/> interface, providing methods to manage university-related operations.
/// </summary>
internal class UniversityServices : IUniversityServices
{
    /// <summary>
    /// The repository used to perform operations related to universities.
    /// </summary>
    private readonly IUniversityRepository _universityRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UniversityServices"/> class.
    /// </summary>
    /// <param name="universityRepository">The repository used to manage university data.</param>
    public UniversityServices(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    /// <summary>
    /// Adds a new university to the data store asynchronously.
    /// </summary>
    /// <param name="university">The university entity to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result indicates whether the university was successfully added.
    /// </returns>
    public Task<bool> AddUniversityAsync(University university)
    {
        return _universityRepository.AddUniversityAsync(university);
    }

    /// <summary>
    /// Retrieves a single university and its information asynchronously.
    /// </summary>
    /// <param name="name">The name of the university to be retreived</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the university with the given name.
    /// </returns>
    public Task<University?> GetByNameAsync(string name)
    {
        return _universityRepository.GetByNameAsync(name);
    }

    /// <summary>
    /// Retrieves a list of all existing universities asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a collection of university entities.
    /// </returns>
    public Task<IEnumerable<University>> ListUniversityAsync()
    {
        return _universityRepository.ListUniversityAsync();
    }

    /// <summary>
    /// Deletes a university asynchronously by name.
    /// </summary>
    /// <param name="universityName">The name of the university to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result indicates whether the university was successfully deleted.
    /// </returns>
    public Task<bool> DeleteUniversityAsync(string universityName)
    {
        return _universityRepository.DeleteUniversityAsync(universityName);
    }
}

