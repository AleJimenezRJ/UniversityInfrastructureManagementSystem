using Microsoft.Kiota.Abstractions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Floors.Item.LearningSpaces.Paginated;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.Spaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.Spaces;

/// <summary>
/// Repository for handling LearningSpace operations using the Kiota-generated API client.
/// </summary>
internal class KiotaLearningSpaceRepository : ILearningSpaceRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaLearningSpaceRepository"/> class.
    /// </summary>
    /// <param name="apiClient">Injected API client used to send requests.</param>
    public KiotaLearningSpaceRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Retrieves the list of learning spaces for a specific floor.
    /// </summary>
    /// <param name="floorId">Unique identifier of the floor.</param>
    /// <returns>
    /// A list of <see cref="LearningSpaceOverviewDto"/> objects if any exist, or <c>null</c> if none are found.
    /// </returns>
    public async Task<List<LearningSpaceOverviewDto>?> ListLearningSpacesAsync(int floorId)
    {
        try
        {
            var learningSpacesListResponse = await _apiClient
                .Floors[floorId]
                .LearningSpaces
                .GetAsync();

            return learningSpacesListResponse?.LearningSpaces?.Select(ls => new LearningSpaceOverviewDto(
                (int)ls.LearningSpaceId!,
                ls.Name!,
                ls.Type!
            )).ToList();
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 400:
                    throw new ValidationException("Error de validación desconocido.");
                case 404:
                    throw new NotFoundException("No se encontró el piso solicitado.");
                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
        }
    }

    /// <summary>
    /// Retrieves the list of paginated learning spaces for a specific floor.
    /// </summary>
    /// <param name="floorId">Unique identifier of the floor.</param>
    /// <param name="pageSize">Size of the page to retrieve.</param>
    /// <param name="pageIndex">Index of the page to retrieve.</param>
    /// <returns>
    /// A list of <see cref="LearningSpaceOverviewDto"/> objects if any exist, or <c>null</c> if none are found.
    /// </returns>
    public async Task<PaginatedList<LearningSpaceOverviewDto>?> ListLearningSpacesPaginatedAsync(int floorId, int pageSize, int pageIndex, string searchText)
    {
        try
        {
            var paginatedRequestBuilder = await _apiClient.Floors[floorId].LearningSpaces.Paginated.GetAsync(
                requestConfiguration => {
                    requestConfiguration.QueryParameters.PageSize = pageSize;
                    requestConfiguration.QueryParameters.PageIndex = pageIndex;
                    requestConfiguration.QueryParameters.SeachText = searchText;
                });

            if (paginatedRequestBuilder?.LearningSpaces is not null)
            {
                return new PaginatedList<LearningSpaceOverviewDto>(
                    paginatedRequestBuilder.LearningSpaces.Select(ls => new LearningSpaceOverviewDto(
                        (int)ls.LearningSpaceId!,
                        ls.Name!,
                        ls.Type!
                    )).ToList(),
                    paginatedRequestBuilder.TotalCount ?? 0,
                    paginatedRequestBuilder.PageSize ?? pageSize,
                    paginatedRequestBuilder.PageIndex ?? pageIndex
                );
            }

            return null;
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 400:
                    throw new ValidationException("El identificador asociado al piso es inválido o los parámetros de paginación son incorrectos.");
                case 404:
                    throw new NotFoundException("No se encontró el piso solicitado.");
                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
        }
    }

    /// <summary>
    /// Retrieves a specific learning space by its identifier.
    /// </summary>
    /// <param name="learningSpaceId">Unique identifier of the learning space.</param>
    /// <returns>
    /// A <see cref="LearningSpace"/> entity if found, or <c>null</c> if it does not exist.
    /// </returns>
    public async Task<LearningSpace?> ReadLearningSpaceAsync(int learningSpaceId)
    {
        try
        {
            var response = await _apiClient.LearningSpaces[learningSpaceId].GetAsync();
            var dto = response?.LearningSpace; 

            return dto is not null ? LearningSpaceDtoMapper.ToEntity(dto) : null;
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            switch ((ex.ResponseStatusCode))
            {
                case 404:
                    throw new NotFoundException("No se encontró el espacio de aprendizaje solicitado.");
                default:
                    throw new DomainException("Ocurrió un error inesperado al leer el espacio de aprendizaje.");
            }
        }
    }


    /// <summary>
    /// Creates a new learning space in the specified floor.
    /// </summary>
    /// <param name="floorId">Unique identifier of the floor.</param>
    /// <param name="learningSpace">The <see cref="LearningSpace"/> entity to create.</param>
    /// <returns>
    /// <c>true</c> if the creation was successful; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> CreateLearningSpaceAsync(int floorId, LearningSpace learningSpace)
    {
        try
        {
            var createLearningSpaceResponse = await _apiClient
                .Floors[floorId]
                .LearningSpaces
                .PostAsync(new PostLearningSpaceRequest { LearningSpace = learningSpace.ToDto() });

            return createLearningSpaceResponse is not null;
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 400:
                    throw new ValidationException("Error de validación desconocido.");
                case 404:
                    throw new NotFoundException("No se encontró el piso solicitado.");
                case 409:
                    throw new DuplicatedEntityException("Ya existe un espacio de aprendizaje con ese nombre en este piso.");
                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
        }
    }

    /// <summary>
    /// Updates an existing learning space with new values.
    /// </summary>
    /// <param name="learningSpaceId">Unique identifier of the learning space to update.</param>
    /// <param name="learningSpace">The updated <see cref="LearningSpace"/> entity.</param>
    /// <returns>
    /// <c>true</c> if the update was successful; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> UpdateLearningSpaceAsync(int learningSpaceId,LearningSpace learningSpace)
    {
        try
        {
            var updateLearningSpaceResponse = await _apiClient
                .LearningSpaces[learningSpaceId]
                .PutAsync(new PutLearningSpaceRequest { LearningSpace = LearningSpaceDtoMapper.ToDto(learningSpace) });

            return updateLearningSpaceResponse is not null;
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 400:
                    throw new ValidationException("Error de validación desconocido.");

                case 409:
                    throw new NotFoundException("Ya existe un espacio de aprendizaje con ese nombre.");

                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
            
        }
    }

    /// <summary>
    /// Deletes a specific learning space by its identifier.
    /// </summary>
    /// <param name="learningSpaceId">Unique identifier of the learning space to delete.</param>
    /// <returns>
    /// <c>true</c> if the deletion was successful; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> DeleteLearningSpaceAsync(int learningSpaceId)
    {
        try
        {
            await _apiClient.LearningSpaces[learningSpaceId].DeleteAsync();
            return true;
        }
        catch (Microsoft.Kiota.Abstractions.ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 404:
                    throw new NotFoundException("No se encontró el espacio de aprendizaje a eliminar.");
                default:
                    throw new DomainException("Ocurrió un error al intentar eliminar el espacio.");
            }
        }
    }
}

