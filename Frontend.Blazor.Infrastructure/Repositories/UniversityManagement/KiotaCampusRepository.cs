using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.UniversityManagement;

/// <summary>
/// Implements <see cref="ICampusRepository"/> using the Kiota-generated API client
/// to manage <see cref="Campus"/> entities with basic CRUD operations.
/// </summary>
internal class KiotaCampusRepository : ICampusRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaCampusRepository"/> class.
    /// </summary>
    /// <param name="apiClient">The Kiota-generated <see cref="ApiClient"/> used for backend communication.</param>
    public KiotaCampusRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Adds a new <see cref="Campus"/> entity to the backend via the API.
    /// </summary>
    /// <param name="campus">The <see cref="Campus"/> entity to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// Returns <c>true</c> if the operation succeeds.
    /// </returns>
    public async Task<bool> AddCampusAsync(Campus campus)
    {
        await _apiClient.AddCampus.PostAsync(CampusDtoMappers.ToDto(campus));
        return true;
    }

    /// <summary>
    /// Retrieves a list of all <see cref="Campus"/> entities from the backend.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a list of <see cref="Campus"/> entities.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown when no campuses are found.</exception>
    public async Task<IEnumerable<Campus>> ListCampusAsync()
    {
        var response = await _apiClient.ListCampus.GetAsync();

        if (response?.Campus == null)
        {
            throw new NotFoundException("No campus found.");
        }

        return CampusDtoMappers.ToEntities(response.Campus);
    }

    /// <summary>
    /// Retrieves a <see cref="Campus"/> entity by its name.
    /// </summary>
    /// <param name="name">The name of the campus to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the <see cref="Campus"/> entity if found; otherwise, throws exception.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown when a campus with the specified name is not found.</exception>
    public async Task<Campus?> GetByNameAsync(string name)
    {
        var dto = await _apiClient.ListCampus[name].GetAsync();

        if (dto?.Campus == null)
        {
            throw new NotFoundException($"Campus with name {name} was not found.");
        }

        return CampusDtoMappers.ToEntity(dto.Campus);
    }

    /// <summary>
    /// Deletes a <see cref="Campus"/> entity by its name.
    /// </summary>
    /// <param name="campusName">The name of the campus to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// Returns <c>true</c> if the campus is deleted successfully.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown when the campus with the specified name does not exist.</exception>
    public async Task<bool> DeleteCampusAsync(string campusName)
    {
        try
        {
            await _apiClient.DeleteCampus[campusName].DeleteAsync();
            return true;
        }
        catch (Exception)
        {
            throw new NotFoundException($"Campus with name {campusName} not found.");
        }
    }
}
