using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.ComponentsManagement;

/// <summary>
/// Implements <see cref="ILearningComponentRepository"/> using the Kiota-generated API client
/// to perform operations on learning components such as projectors and whiteboards.
/// </summary>
internal class KiotaLearningComponentRepository : ILearningComponentRepository
{
    private readonly ApiClient _apiClient;
    private readonly GlobalMapper _mapper;
    private readonly List<ILearningComponentRequestBuilder> _builders;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaLearningComponentRepository"/> class.
    /// </summary>
    /// <param name="apiClient">The Kiota API client used to communicate with the backend service.</param>
    public KiotaLearningComponentRepository(ApiClient apiClient, IEnumerable<ILearningComponentRequestBuilder> builders)
    {
        _apiClient = apiClient;
        _mapper = new GlobalMapper();
        _builders = builders.ToList();
    }

    /// <summary>
    /// Adds a new learning component (projector or whiteboard) to a specific learning space.
    /// </summary>
    /// <param name="learningSpaceId">The ID of the learning space where the component will be added.</param>
    /// <param name="learningComponent">The learning component to add.</param>
    /// <returns>A task representing the asynchronous operation, containing true if successful; otherwise false.</returns>
    public async Task<bool> AddComponentAsync(int learningSpaceId, LearningComponent learningComponent)
    {
        try
        {
            var builder = _builders.Find(b => b.CanHandle(learningComponent));
            if (builder is null)
                throw new NotSupportedException("Unsupported component type for addition.");
            await builder.PostAsync(learningComponent,learningSpaceId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Deletes a learning component by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the learning component to delete.</param>
    /// <returns>A task representing the asynchronous operation, containing true if successful; otherwise false.</returns>
    public async Task<bool> DeleteComponentAsync(int id)
    {
        try
        {
            await _apiClient.LearningComponent[id].DeleteAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Retrieves all learning components associated with a specific learning space.
    /// </summary>
    /// <param name="learningSpaceId">The ID of the learning space.</param>
    /// <param name="pageSize">The maximum number of learning components to retrieve per page.</param>
    /// <param name="pageIndex">The page index for pagination.</param>
    /// <returns>A task representing the asynchronous operation, containing the list of learning components.</returns>
    /// <exception cref="NotFoundException">Thrown when no components are found.</exception>
    public async Task<PaginatedList<LearningComponent>> GetLearningComponentsByIdAsync(int learningSpaceId, int pageSize, int pageIndex, string stringSearch)
    {
        var response = await _apiClient.LearningSpaces[learningSpaceId].LearningComponent
            .GetAsync(x =>
            {
                x.QueryParameters.StringSearch = stringSearch;
                x.QueryParameters.PageSize = pageSize;
                x.QueryParameters.PageIndex= pageIndex;
            });

        if (response?.LearningComponents is null)
        {
            throw new NotFoundException("No components found.");
        }

        if (!response.LearningComponents.Any())
        {
            return PaginatedList<LearningComponent>.Empty(pageSize, pageIndex);
        }

        var components = response.LearningComponents
            .Select(dto => _mapper.ToEntityFromIdDto<LearningComponent>(dto))
            .ToList();

        return new PaginatedList<LearningComponent>(
            components,
            (int)response.TotalCount!,
            (int)response.PageSize!,
            (int)response.PageIndex!
        );
    }

    /// <summary>
    /// Retrieves all learning components in the system.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the list of all learning components.</returns>
    /// <exception cref="NotFoundException">Thrown when no components are found.</exception>
    public async Task<IEnumerable<LearningComponent>> GetAllAsync(int pageSize, int pageIndex)
    {
        var response = await _apiClient.LearningComponent.GetAsync(x =>
        {
            x.QueryParameters.PageSize = pageSize;
            x.QueryParameters.PageIndex= pageIndex;
        });

        var components = new List<LearningComponent>();

        if (response?.LearningComponents is null)
        {
            throw new NotFoundException("No components found.");
        }
        else if (!response.LearningComponents.Any())
        {
            return components;
        }

        return response.LearningComponents
            .Select(dto => _mapper.ToEntityFromIdDto<LearningComponent>(dto))
            .ToList();
    }

    /// <summary>
    /// Retrieves a single learning component by its ID.
    /// </summary>
    /// <param name="id">The ID of the learning component.</param>
    /// <returns>A task representing the asynchronous operation, containing the learning component.</returns>
    /// <exception cref="NotFoundException">Thrown when the component is not found or not supported.</exception>
    Task<LearningComponent> ILearningComponentRepository.GetSingleLearningComponentAsync(int id)
    {
        return GetSingleLearningComponentInternalAsync(id);
    }

    /// <summary>
    /// Not implemented. Updates an existing learning component.
    /// </summary>
    /// <param name="learningSpaceId">The ID of the learning space associated with the component.</param>
    /// <param name="learningComponent">The updated component data.</param>
    /// <returns>Throws a <see cref="NotImplementedException"/>.</returns>
    public async Task<bool> UpdateAsync(int learningSpaceId, LearningComponent learningComponent)
    {
        try
        {
            var builder = _builders.Find(b => b.CanHandle(learningComponent));
            if (builder is null)
                throw new NotSupportedException("Unsupported component type for update.");
            await builder.PutAsync(learningComponent, learningSpaceId);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Retrieves a single learning component by its ID by searching all existing components.
    /// </summary>
    /// <param name="id">The ID of the learning component.</param>
    /// <returns>A task representing the asynchronous operation, containing the matching component.</returns>
    /// <exception cref="NotFoundException">Thrown when the component is not found.</exception>
    private async Task<LearningComponent> GetSingleLearningComponentInternalAsync(int id)
    {
        try
        {
            var response = await _apiClient.LearningComponent[id].GetAsync();

            if (response?.LearningComponent is null)
                throw new NotFoundException("Learning component not found");

            return _mapper.ToEntityFromIdDto<LearningComponent>(response.LearningComponent);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error retrieving component with id {id}: {ex.Message}");
            throw new NotFoundException($"Learning component with id {id} not found");
        }
    }
}