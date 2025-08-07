using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.UniversityManagement;

/// <summary>
/// Implements <see cref="IBuildingsRepository"/> using the Kiota-generated API client
/// to manage <see cref="Building"/> entities with full CRUD operations.
/// </summary>
internal class KiotaBuildingsRepository : IBuildingsRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaBuildingsRepository"/> class.
    /// </summary>
    /// <param name="apiClient">The Kiota-generated <see cref="ApiClient"/> used for API communication.</param>
    public KiotaBuildingsRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Adds a new <see cref="Building"/> entity via the API.
    /// </summary>
    /// <param name="building">The <see cref="Building"/> entity to be added.</param>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// Returns <c>true</c> if the operation is successful.
    /// </returns>
    public async Task<bool> AddBuildingAsync(Building building)
    {
        try
        {
            await _apiClient.AddBuilding.PostAsync(BuildingDtoMappers.ToDto(building));
            return true;
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 400:
                    throw new ValidationException("Error de validación desconocido.");
                case 404:
                    throw new NotFoundException("No se encontró el recurso solicitado.");
                case 409:
                    throw new DuplicatedEntityException("El edificio ya se encuentra en la base de datos");
                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
        }
    }

    /// <summary>
    /// Updates an existing <see cref="Building"/> entity identified by its ID.
    /// </summary>
    /// <param name="building">The updated <see cref="Building"/> entity.</param>
    /// <param name="buildingId">The ID of the building to update.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// Returns <c>true</c> if the update is successful.
    /// </returns>
    public async Task<bool> UpdateBuildingAsync(Building building, int buildingId)
    {
        try
        {
            await _apiClient.UpdateBuilding[buildingId].PutAsync(BuildingDtoMappers.ToDto(building));
            return true;

        }

        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 400:
                    throw new ValidationException("Error de validación desconocido.");
                case 404:
                    throw new NotFoundException("No se encontró el edificio solicitado.");
                case 409:
                    throw new DuplicatedEntityException("El edificio ya se encuentra en la base de datos");
                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
        }

    }

    /// <summary>
    /// Retrieves a <see cref="Building"/> entity by its ID, including its associated area, campus, and university.
    /// </summary>
    /// <param name="buildingId">The ID of the building to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the <see cref="Building"/> entity if found; otherwise, <c>null</c>.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown when the building is not found.</exception>
    public async Task<Building?> DisplayBuildingAsync(int buildingId)
    {
        try
        {
            var dto = await _apiClient.ListBuilding[buildingId].GetAsync();

            if (dto?.Building == null)
            {
                throw new NotFoundException($"Building with ID {buildingId} not found.");
            }
            return BuildingDtoMappers.ToEntity(dto.Building);

        }

        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 400:
                    throw new ValidationException("Error de validación desconocido.");
                case 404:
                    throw new NotFoundException("No se encontró el edificio solicitado.");
                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
        }
    }

    /// <summary>
    /// Retrieves all <see cref="Building"/> entities from the backend.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a collection of <see cref="Building"/> entities.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown when no buildings are found.</exception>
    public async Task<IEnumerable<Building>> ListBuildingAsync()
    {
        var response = await _apiClient.ListBuildings.GetAsync();

        if (response?.Buildings == null)
        {
            throw new NotFoundException("No buildings found.");
        }

        return BuildingDtoMappers.ToEntities(response.Buildings);
    }

    /// <summary>
    /// Deletes a <see cref="Building"/> by its ID.
    /// </summary>
    /// <param name="buildingId">The ID of the building to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// Returns <c>true</c> if the deletion is successful.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown when the building to delete is not found.</exception>
    public async Task<bool> DeleteBuildingAsync(int buildingId)
    {
        try
        {
            await _apiClient.DeleteBuilding[buildingId].DeleteAsync();
            return true;
        }
        catch (Exception)
        {
            throw new NotFoundException($"Building with ID {buildingId} not found.");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of buildings from an external API.
    /// </summary>
    /// <param name="pageSize">The number of buildings to retrieve per page.</param>
    /// <param name="pageIndex">The page index for pagination.</param>    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedList{Building}"/>
    /// which includes the list of buildings and pagination metadata.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown when the API response is null or does not contain any buildings.
    /// </exception>
    public async Task<PaginatedList<Building>> ListBuildingPaginatedAsync(int pageSize, int pageIndex, string? search = null)
    {
        var response = await _apiClient.ListPaginatedBuildings.GetAsync(x =>
        {
            x.QueryParameters.PageSize = pageSize;
            x.QueryParameters.PageIndex = pageIndex;

            if (!string.IsNullOrWhiteSpace(search))
            {
                x.QueryParameters.SearchText = search;
            }
        });

        var buildings = new List<Building>();

        if (response?.Buildings == null)
        {
            throw new NotFoundException("No se encontraron edificios en el sistema");
        }

        if (response.Buildings.Count == 0)
        {
            return PaginatedList<Building>.Empty(pageSize, pageIndex);
        }

        buildings.AddRange(BuildingDtoMappers.ToEntities(response.Buildings));

        return new PaginatedList<Building>(
            buildings,
            (int)response.TotalCount!,
            (int)response.PageSize!,
            (int)response.PageIndex!
        );
    }

}