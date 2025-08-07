using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;

/// <summary>
/// Defines the contract for the LearningSpace repository, which provides 
/// methods to manage and persist <see cref="LearningSpace"/> entities in the system.
/// </summary>
public interface ILearningSpaceRepository
{
    /// <summary>
    /// Retrieves a learning space by its name on a specific floor of a building.
    /// </summary>
    /// <param name="learningSpaceId">The unique identifier of the learning space.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="LearningSpace"/>
    /// if found, or <c>null</c> if no matching learning space exists.
    /// </returns>
    Task<LearningSpace?> ReadLearningSpaceAsync(int learningSpaceId);
    /// <summary>
    /// Lists all learning spaces available on a specific floor in a building.
    /// </summary>
    /// <param name="floorId">The internal Id of the floor.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="LearningSpace"/>
    /// entities in the specified building and floor, or <c>null</c> if none are found.
    /// </returns>
    Task<List<LearningSpace>?> ListLearningSpacesAsync(int floorId);

    /// <summary>
    /// Lists learning spaces available on a specific floor in a building with pagination.
    /// </summary>
    /// <param name="floorId">The internal Id of the floor.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="pageIndex">The index of the current page.</param>
    /// <param name="searchText">The search text to filter the learning spaces by name.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a paginated list of <see cref="LearningSpace"/>
    /// entities in the specified building and floor.
    /// </returns>
    Task<PaginatedList<LearningSpace>> ListLearningSpacesPaginatedAsync(int floorId, int pageSize, int pageIndex, string searchText);

    /// <summary>
    /// Asynchronously creates a new learning space.
    /// </summary>
    /// <param name="floorId">The internal Id of the floor where the learning space will be created.</param>
    /// <param name="learningSpace">The <see cref="LearningSpace"/> entity to be created.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is <c>true</c> if the learning space was successfully created; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> CreateLearningSpaceAsync(int floorId, LearningSpace learningSpace);

    /// <summary>
    /// Asynchronously updates an existing learning space.
    /// </summary>
    /// <param name="learningSpaceId">The identifier of the learning space that will be updated.</param>
    /// <param name="learningSpace">The <see cref="LearningSpace"/> entity to be updated.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is <c>true</c> if the update was successful; otherwise, <c>false</c> (for example, if the learning space was not found).
    /// </returns>
    Task<bool> UpdateLearningSpaceAsync(int learningSpaceId, LearningSpace learningSpace);

    /// <summary>
    /// Asynchronously deletes a learning space by its name on a specific floor of a building.
    /// </summary>
    /// <param name="learningSpaceId">The identifier of the learning space that will be deleted.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is <c>true</c> if the learning space was successfully deleted; otherwise, <c>false</c> (for example, if the learning space was not found).
    /// </returns>
    Task<bool> DeleteLearningSpaceAsync(int learningSpaceId);
}