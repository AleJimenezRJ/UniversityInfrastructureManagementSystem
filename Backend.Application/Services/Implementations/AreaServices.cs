using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;

/// <summary>
/// Provides application-level services for managing theme park areas.
/// Encapsulates the logic for interacting with the area repository.
/// </summary>
internal class AreaServices : IAreaServices
{
    /// <summary>
    /// The repository used to perform operations related to areas.
    /// </summary>
    private readonly IAreaRepository _areaRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AreaServices"/> class.
    /// </summary>
    /// <param name="areaRepository">The repository used to persist and retrieve area entities.</param>
    public AreaServices(IAreaRepository areaRepository)
    {
        _areaRepository = areaRepository;
    }

    /// <summary>
    /// Asynchronously adds a new area to the database.
    /// </summary>
    /// <param name="area">The area entity to be added.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result is <c>true</c> if the area was successfully added; otherwise, <c>false</c>.
    /// </returns>
    public Task<bool> AddAreaAsync(Area area)
    {
        return _areaRepository.AddAreaAsync(area);
    }

    /// <summary>
    /// Retrieves a list of all existing areas asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a collection of areas.
    /// </returns>
    public Task<IEnumerable<Area>> ListAreaAsync()
    {
        return _areaRepository.ListAreaAsync();
    }

    /// <summary>
    /// Asynchronously retrieves an <see cref="Area"/> entity by its name.
    /// </summary>
    /// <param name="name">The name of the area to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// The task result is the <see cref="Area"/> entity if found; otherwise, <c>null</c>.
    /// </returns>
    public Task<Area?> GetByNameAsync(string name)
    {
        return _areaRepository.GetByNameAsync(name);
    }

    /// <summary>
    /// Deletes an area in the data base
    /// </summary>
    /// <param name="name"> Name of the area entity to be deleted </param>
    /// <returns>
    /// A <see cref="Task"/> that represent the deleted area if true.
    ///</returns>
    public Task<bool> DeleteAreaAsync(string name)
    {
        return _areaRepository.DeleteAreaAsync(name);
    }
}
