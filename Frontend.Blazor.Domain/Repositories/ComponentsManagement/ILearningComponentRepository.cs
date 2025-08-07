using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;

/// <summary>
/// Interface for managing learning components (projectors, whiteboards, etc.) in the system.
/// </summary>
public interface ILearningComponentRepository
{
    /// <summary>
    /// Retrieves all learning components in the system.
    /// </summary>
    /// <returns></returns>
    /// <param name="pageSize">The maximum number of learning components to retrieve per page.</param>
    /// <param name="pageIndex">The page index for pagination.</param>
    /// <returns>A collection of learning components</returns>
    Task<IEnumerable<LearningComponent>> GetAllAsync(int pageSize, int pageIndex);

    /// <summary>
    /// Adds a new learning component to the database.
    /// </summary>
    /// <param name="learningComponent"></param>
    /// <returns></returns>
    Task<bool> AddComponentAsync(int learningSpaceId, LearningComponent learningComponent);

    /// <summary>
    /// Retrieves learning components for a specific learning space.
    /// </summary>
    /// <param name="learningSpaceId">The identifier of the learning space to retrieve components for.</param>
    /// <param name="pageSize">The maximum number of learning components to retrieve per page.</param>
    /// <param name="pageIndex"></param>
    /// <returns>A collection of learning components for the specified learning space.</returns>
    Task<PaginatedList<LearningComponent>> GetLearningComponentsByIdAsync(int learningSpaceId, int pageSize, int pageIndex, string stringSearch);


    /// <summary>
    /// Deletes a learning component from the database based on its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteComponentAsync(int id);

    /// <summary>
    /// Updates an existing learning component in the database.
    /// </summary>
    /// <param name="learningSpaceId"></param>
    /// <param name="learningComponent"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync(int learningSpaceId, LearningComponent learningComponent);

    /// <summary>
    /// Retrieves a single learning component based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the learning component to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the requested learning component.</returns>
    Task<LearningComponent> GetSingleLearningComponentAsync(int id);
}


