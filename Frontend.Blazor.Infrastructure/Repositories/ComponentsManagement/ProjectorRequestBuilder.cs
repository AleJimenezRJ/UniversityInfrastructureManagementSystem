using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.ComponentsManagement;

/// <summary>
/// Implements <see cref="ILearningComponentRequestBuilder"/> for handling HTTP requests related to <see cref="Projector"/> components.
/// Responsible for mapping domain entities to DTOs and sending POST/PUT requests to the backend API for projector operations.
/// </summary>
public class ProjectorRequestBuilder : ILearningComponentRequestBuilder
{
    private readonly ApiClient _apiClient;
    private readonly GlobalMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectorRequestBuilder"/> class.
    /// </summary>
    /// <param name="apiClient">The API client used to send HTTP requests.</param>
    public ProjectorRequestBuilder(ApiClient apiClient)
    {
        _apiClient = apiClient;
        _mapper = new GlobalMapper();
    }

    /// <summary>
    /// Determines if this builder can handle the specified <see cref="LearningComponent"/> (must be a <see cref="Projector"/>).
    /// </summary>
    /// <param name="learningComponent">The learning component to check.</param>
    /// <returns><c>true</c> if the component is a <see cref="Projector"/>; otherwise, <c>false</c>.</returns>
    public bool CanHandle(LearningComponent learningComponent) => learningComponent is Projector;

    /// <summary>
    /// Sends a POST request to create a new <see cref="Projector"/> in the specified learning space.
    /// </summary>
    /// <param name="learningComponent">The projector entity to create.</param>
    /// <param name="learningSpaceId">The identifier of the learning space.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task PostAsync(LearningComponent learningComponent, int learningSpaceId)
    {
        var projector = _mapper.ToDtoNoId(learningComponent) as ProjectorNoIdDto;
        var request = new PostProjectorRequest
        {
            Projector = projector
        };
        await _apiClient.LearningSpaces[learningSpaceId].LearningComponent.Projector.PostAsync(request);
    }

    /// <summary>
    /// Sends a PUT request to update an existing <see cref="Projector"/> in the specified learning space.
    /// </summary>
    /// <param name="learningComponent">The projector entity to update.</param>
    /// <param name="learningSpaceId">The identifier of the learning space.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task PutAsync(LearningComponent learningComponent, int learningSpaceId)
    {
        var projector = _mapper.ToDtoNoId(learningComponent) as ProjectorNoIdDto;
        var projectorReq = new PutProjectorRequest { Projector = projector };
        await _apiClient.LearningSpaces[learningSpaceId].LearningComponent.Projector[learningComponent.ComponentId].PutAsync(projectorReq);
    }
}
