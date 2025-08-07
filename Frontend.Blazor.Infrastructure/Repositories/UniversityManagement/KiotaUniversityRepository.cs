using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.UniversityManagement;

/// <summary>
/// Implements <see cref="IUniversityRepository"/> using the Kiota-generated API client
/// to manage <see cref="University"/> entities through CRUD operations.
/// </summary>
internal class KiotaUniversityRepository : IUniversityRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaUniversityRepository"/> class.
    /// </summary>
    /// <param name="apiClient">The Kiota-generated <see cref="ApiClient"/> used to communicate with the backend API.</param>
    public KiotaUniversityRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Adds a new <see cref="University"/> entity via the API.
    /// </summary>
    /// <param name="university">The university entity to be added.</param>
    /// <returns>A task representing the asynchronous operation. Returns <c>true</c> if the operation succeeds.</returns>
    public async Task<bool> AddUniversityAsync(University university)
    {
        await _apiClient.AddUniversity.PostAsync(UniversityDtoMappers.ToDto(university));
        return true;
    }

    /// <summary>
    /// Retrieves all <see cref="University"/> entities from the backend API.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. Contains a list of universities.</returns>
    /// <exception cref="NotFoundException">Thrown if no universities are found.</exception>
    public async Task<IEnumerable<University>> ListUniversityAsync()
    {
        var response = await _apiClient.ListUniversity.GetAsync();

        if (response?.University == null)
        {
            throw new NotFoundException("No university found.");
        }

        return UniversityDtoMappers.ToEntities(response.University);
    }

    /// <summary>
    /// Retrieves a <see cref="University"/> entity by its name.
    /// </summary>
    /// <param name="name">The name of the university to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. Contains the university entity if found.</returns>
    /// <exception cref="NotFoundException">Thrown if a university with the given name is not found.</exception>
    public async Task<University?> GetByNameAsync(string name)
    {
        var dto = await _apiClient.ListUniversity[name].GetAsync();

        if (dto?.University == null)
        {
            throw new NotFoundException($"University with name {name} was not found.");
        }

        return UniversityDtoMappers.ToEntity(dto.University);
    }

    /// <summary>
    /// Deletes a <see cref="University"/> entity by its name.
    /// </summary>
    /// <param name="universityName">The name of the university to delete.</param>
    /// <returns>A task representing the asynchronous operation. Returns <c>true</c> if deletion succeeds.</returns>
    /// <exception cref="NotFoundException">Thrown if a university with the specified name does not exist.</exception>
    public async Task<bool> DeleteUniversityAsync(string universityName)
    {
        try
        {
            await _apiClient.University[universityName].DeleteAsync();
            return true;
        }
        catch (Exception)
        {
            throw new NotFoundException($"University with name {universityName} not found.");
        }
    }
}
