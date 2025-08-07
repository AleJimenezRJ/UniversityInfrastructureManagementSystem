using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.UniversityManagement;

/// <summary>
/// Repository implementation for managing <see cref="Area"/> entities using the Kiota-generated API client.
/// </summary>
internal class KiotaAreaRepository : IAreaRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaAreaRepository"/> class with the specified API client.
    /// </summary>
    /// <param name="apiClient">The Kiota-generated <see cref="ApiClient"/> used for API communication.</param>
    public KiotaAreaRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Adds a new <see cref="Area"/> to the backend via the API.
    /// </summary>
    /// <param name="area">The <see cref="Area"/> entity to be added.</param>
    /// <returns>A task representing the asynchronous operation. Returns <c>true</c> if the operation is successful.</returns>
    public async Task<bool> AddAreaAsync(Area area)
    {
        await _apiClient.AddArea.PostAsync(AreaDtoMappers.ToDto(area));
        return true;
    }

    /// <summary>
    /// Retrieves all <see cref="Area"/> entities from the backend API.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// The task result contains an <see cref="IEnumerable{Area}"/> with all retrieved areas.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown when the API returns no area data.</exception>
    public async Task<IEnumerable<Area>> ListAreaAsync()
    {
        var response = await _apiClient.ListAreas.GetAsync();

        if (response?.Area == null)
        {
            throw new NotFoundException("No areas found.");
        }

        return AreaDtoMappers.ToEntities(response.Area);
    }

    /// <summary>
    /// Retrieves a specific <see cref="Area"/> by its name.
    /// </summary>
    /// <param name="name">The name of the area to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the <see cref="Area"/> if found; otherwise, <c>null</c>.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown when an area with the specified name is not found.</exception>
    public async Task<Area?> GetByNameAsync(string name)
    {
        var dto = await _apiClient.ListAreas[name].GetAsync();

        if (dto?.Area == null)
        {
            throw new NotFoundException($"Area with name '{name}' was not found.");
        }

        return AreaDtoMappers.ToEntity(dto.Area);
    }

    /// <summary>
    /// Deletes an <see cref="Area"/> by its name.
    /// </summary>
    /// <param name="name">The name of the area to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// Returns <c>true</c> if the area was successfully deleted.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown when the area to be deleted is not found.</exception>
    public async Task<bool> DeleteAreaAsync(string name)
    {
        try
        {
            await _apiClient.Area[name].DeleteAsync();
            return true;
        }
        catch (Exception)
        {
            throw new NotFoundException($"Area with name '{name}' not found.");
        }
    }
}
