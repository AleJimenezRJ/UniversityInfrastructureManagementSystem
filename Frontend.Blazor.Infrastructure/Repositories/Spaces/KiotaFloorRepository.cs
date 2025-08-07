using Microsoft.Kiota.Abstractions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Buildings.Item.Floors.Paginated;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.Spaces;

internal class KiotaFloorRepository : IFloorRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaFloorRepository"/> class.
    /// </summary>
    /// <param name="apiClient">The injected API client used to send requests.</param>
    public KiotaFloorRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Adds a new floor to a building.
    /// </summary>
    /// <param name="buildingId">The internal Id of a building</param>
    /// <returns>A <see cref="Task"/> result that is true if the floor was successfully created;
    /// otherwise, false. </returns>
    public async Task<bool> CreateFloorAsync(int buildingId)
    {
        try
        {
            var createFloorResponse = await _apiClient
                .Buildings[buildingId]
                .Floors
                .PostAsync();

            return createFloorResponse is not null;
        }
        catch (ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
        }
    }

    /// <summary>
    /// Deletes a specific floor in a building.
    /// </summary>
    /// <param name="floorId">The internal Id of the floor entity</param>
    /// <returns>
    /// A <see cref="Task"/> result that is true if the floor was successfully deleted;
    /// otherwise, false.
    /// </returns>
    public async Task<bool> DeleteFloorAsync(int floorId)
    {
        try
        {
            await _apiClient.Floors[floorId].DeleteAsync();
            return true;
        }
        catch (ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 404:
                    throw new NotFoundException("El piso seleccionado ya no existe dentro del edificio seleccionado.");
                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
        }
    }

    /// <summary>
    /// List all floors available in a building.
    /// </summary>
    /// <param name="buildingId">The internal Id of the building.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Floor"/>
    /// entities in the specified building, or <c>null</c> if none are found
    /// </returns>
    public async Task<List<Floor>?> ListFloorsAsync(int buildingId)
    {
        try
        {
            var floorListResponse = await _apiClient
                .Buildings[buildingId]
                .Floors
                .GetAsync();

            return floorListResponse?.Floors?.Select(floor =>
                new Floor(
                    (int)floor.FloorId!,
                    FloorNumber.Create(floor.FloorNumber)
                )).ToList();
        }
        catch (ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 400:
                    throw new ValidationException("El identificador asociado al edificio es inválido.");

                case 404:
                    throw new NotFoundException("El piso seleccionado ya no existe.");

                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
        }
    }

    /// <summary>
    /// Lists floors available in a building with pagination.
    /// </summary>
    /// <param name="buildingId">The internal Id of the building.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="pageIndex">The index of the current page (1-based).</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a paginated list of <see cref="Floor"/>
    /// entities in the specified building.
    /// </returns>
    public async Task<PaginatedList<Floor>?> ListFloorsPaginatedAsync(int buildingId, int pageSize, int pageIndex)
    {
        try
        {
            var paginatedRequestBuilder = await _apiClient.Buildings[buildingId].Floors.Paginated.GetAsync(
                requestConfiguration => {
                    requestConfiguration.QueryParameters.PageSize = pageSize;
                    requestConfiguration.QueryParameters.PageIndex = pageIndex;
                });

            if (paginatedRequestBuilder?.Floors is not null)
            {
                return new PaginatedList<Floor>(
                    paginatedRequestBuilder.Floors.Select(floor => 
                        new Floor(
                            (int)floor.FloorId!,
                            FloorNumber.Create(floor.FloorNumber)
                        )).ToList(),
                    paginatedRequestBuilder.TotalCount ?? 0,
                    paginatedRequestBuilder.PageIndex ?? pageIndex,
                    paginatedRequestBuilder.PageSize ?? pageSize
                );
            }

            return null;
        }
        catch (ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 400:
                    throw new ValidationException("El identificador asociado al edificio es inválido o los parámetros de paginación son incorrectos.");
                case 404:
                    throw new NotFoundException("No se encontró el edificio solicitado.");
                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
        }
    }
}
