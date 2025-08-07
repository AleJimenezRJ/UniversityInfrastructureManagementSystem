using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.ComponentsManagement;

/// <summary>
/// Implements <see cref="ILearningComponentRequestBuilder"/> for handling HTTP requests related to <see cref="Whiteboard"/> components.
/// Responsible for mapping domain entities to DTOs and sending POST/PUT requests to the backend API for whiteboard operations.
/// </summary>
public class WhiteboardRequestBuilder : ILearningComponentRequestBuilder
{
    private readonly ApiClient _apiClient;
    private readonly GlobalMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="WhiteboardRequestBuilder"/> class.
    /// </summary>
    /// <param name="apiClient">The API client used to send HTTP requests.</param>
    public WhiteboardRequestBuilder(ApiClient apiClient)
    {
        _apiClient = apiClient;
        _mapper = new GlobalMapper();
    }

    /// <summary>
    /// Determines if this builder can handle the specified <see cref="LearningComponent"/> (must be a <see cref="Whiteboard"/>).
    /// </summary>
    /// <param name="learningComponent">The learning component to check.</param>
    /// <returns><c>true</c> if the component is a <see cref="Whiteboard"/>; otherwise, <c>false</c>.</returns>
    public bool CanHandle(LearningComponent learningComponent) => learningComponent is Whiteboard;

    /// <summary>
    /// Sends a POST request to create a new <see cref="Whiteboard"/> in the specified learning space.
    /// </summary>
    /// <param name="learningComponent">The whiteboard entity to create.</param>
    /// <param name="learningSpaceId">The identifier of the learning space.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task PostAsync(LearningComponent learningComponent, int learningSpaceId)
    {
        var whiteboard = _mapper.ToDtoNoId(learningComponent) as WhiteboardNoIdDto;
        var request = new PostWhiteboardRequest
        {
            Whiteboard = whiteboard
        };
        await _apiClient.LearningSpaces[learningSpaceId].LearningComponent.Whiteboard.PostAsync(request);
    }

    /// <summary>
    /// Sends a PUT request to update an existing <see cref="Whiteboard"/> in the specified learning space.
    /// </summary>
    /// <param name="learningComponent">The whiteboard entity to update.</param>
    /// <param name="learningSpaceId">The identifier of the learning space.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task PutAsync(LearningComponent learningComponent, int learningSpaceId)
    {
        var whiteboard = _mapper.ToDtoNoId(learningComponent) as WhiteboardNoIdDto;
        var whiteboardReq = new PutWhiteboardRequest { Whiteboard = whiteboard };
        await _apiClient.LearningSpaces[learningSpaceId].LearningComponent.Whiteboard[learningComponent.ComponentId].PutAsync(whiteboardReq);
    }
}

