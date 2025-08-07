using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;

/// <summary>
/// Interface for managing learning components (projectors, whiteboards, etc.) in the system.
/// </summary>
public interface ILearningComponentRepository
{
    /// <summary>
    /// Retrieves all learning components in the system.
    /// </summary>
    /// <param name="pageSize">The maximum number of learning components to retrieve per page.</param>
    /// <param name="pageIndex">The page index for pagination.</param>
    /// <returns>A collection of learning components</returns>
    Task<IEnumerable<LearningComponent>> GetAllAsync(int pageSize, int pageIndex);

    /// <summary>
    /// Adds a learning component to a specified learning space.
    /// </summary>
    /// <param name="learningSpaceId">The unique identifier of the learning space where the component will be added.</param>
    /// <param name="learningComponent">The learning component to be added to the learning space.</param>
    /// <returns>A task representing the asynchronous operation, containing a boolean value indicating whether the component was successfully added.</returns>
    Task<bool> AddComponentAsync(int learningSpaceId, LearningComponent learningComponent);

    /// <summary>
    /// Retrieves a single learning component based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the learning component to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the requested learning component.</returns>
    Task<LearningComponent?> GetSingleLearningComponentAsync(int id);

    /// <summary>
    /// Retrieves learning components for a specified learning space, with pagination support.
    /// </summary>
    /// <param name="learningSpaceId">The identifier of the learning space to retrieve components for.</param>
    /// <param name="pageSize">The maximum number of learning components to retrieve per page.</param>
    /// <param name="pageIndex"></param>
    /// <param name="stringSearch">The string to filter the results on.</param>
    /// <returns>A collection of learning components for the specified learning space.</returns>
    Task<PaginatedList<LearningComponent>> GetLearningComponentsByIdAsync(int learningSpaceId, int pageSize, int pageIndex, string stringSearch);

    /// <summary>
    /// Deletes a specific learning component from the system based on its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the learning component to be deleted.</param>
    /// <returns>A task representing the asynchronous operation, containing a boolean value indicating whether the deletion was successful.</returns>
    Task<bool> DeleteComponentAsync(int id);

    /// <summary>
    /// Updates an existing learning component in the specified learning space.
    /// </summary>
    /// <param name="learningSpaceId">The identifier of the learning space containing the component to be updated.</param>
    /// <param name="learningComponentId">The unique identifier of the learning component to update.</param>
    /// <param name="learningComponent">The updated information for the learning component.</param>
    /// <returns>A task that resolves to a boolean indicating whether the update was successful.</returns>
    Task<bool> UpdateAsync(int learningSpaceId, int learningComponentId, LearningComponent learningComponent);
}


