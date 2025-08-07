using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;

/// <summary>
/// Defines a contract for building and sending HTTP requests related to <see cref="LearningComponent"/> entities
/// in the context of a learning space. Implementations are responsible for handling specific types of learning components
/// and providing logic for POST and PUT operations.
/// </summary>
public interface ILearningComponentRequestBuilder
{
    /// <summary>
    /// Determines whether this builder can handle the specified <see cref="LearningComponent"/> type.
    /// </summary>
    /// <param name="learningComponent">The learning component to check.</param>
    /// <returns><c>true</c> if this builder can handle the component; otherwise, <c>false</c>.</returns>
    bool CanHandle(LearningComponent learningComponent);

    /// <summary>
    /// Sends a POST request to create the specified <see cref="LearningComponent"/> in the given learning space.
    /// </summary>
    /// <param name="learningComponent">The learning component to create.</param>
    /// <param name="learningSpaceId">The identifier of the learning space.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PostAsync(LearningComponent learningComponent, int learningSpaceId);

    /// <summary>
    /// Sends a PUT request to update the specified <see cref="LearningComponent"/> in the given learning space.
    /// </summary>
    /// <param name="learningComponent">The learning component to update.</param>
    /// <param name="learningSpaceId">The identifier of the learning space.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task PutAsync(LearningComponent learningComponent, int learningSpaceId);
}
