using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services;

/// <summary>
/// Defines the contract for the application layer services related to LearningSpace entities. 
/// Provides methods to handle business logic and coordinate operations for managing learning spaces.
/// </summary>
public interface ILearningSpaceServices
{
    /// <summary>
    /// Retrieves a learning space by its name on a specific floor of a building.
    /// </summary>
    /// <param name="learningSpaceId">The unique identifier of the learning space.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="LearningSpace"/>
    /// if found, or <c>null</c> if no matching learning space exists.
    /// </returns>
    Task<LearningSpace?> GetLearningSpaceAsync(int learningSpaceId);
    
    /// <summary>
    /// Retrieves all learning spaces for a specific floor in a building.
    /// </summary>
    /// <param name="floorId">The internal Id of the floor.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="LearningSpace"/>
    /// objects in the specified building and floor, or <c>null</c> if none are found.
    /// </returns>
    Task<List<LearningSpace>?> GetLearningSpacesListAsync(int floorId);

    /// <summary>
    /// Retrieves learning spaces for a specific floor in a building with pagination.
    /// </summary>
    /// <param name="floorId">The internal Id of the floor.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="pageIndex">The index of the current page</param>
    /// <param name="searchText">The search text to filter the learning spaces by name</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a paginated list of <see cref="LearningSpace"/>
    /// objects in the specified building and floor.
    /// </returns>
    Task<PaginatedList<LearningSpace>> GetLearningSpacesListPaginatedAsync(int floorId, int pageSize, int pageIndex, string searchText);

    /// <summary>
    /// Asynchronously handles the creation of a new learning space by applying business logic 
    /// and delegating persistence operations to the repository layer.
    /// </summary>
    /// <param name="floorId">The internal Id of the floor where the learning space will be created.</param>
    /// <param name="learningSpace">The <see cref="LearningSpace"/> entity to be created.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is <c>true</c> if the learning space was successfully created; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> CreateLearningSpaceAsync(int floorId, LearningSpace learningSpace);

    /// <summary>
    /// Asynchronously handles the update of an existing learning space by applying business logic
    /// and delegating persistence operations to the repository layer.
    /// </summary>
    /// <param name="learningSpaceId">The identifier of the learning space that will be updated.</param>
    /// <param name="learningSpace">The <see cref="LearningSpace"/> entity to be updated.</param>
    /// A task that represents the asynchronous operation. The task result is <c>true</c> if the learning space was successfully updated; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> UpdateLearningSpaceAsync(int learningSpaceId, LearningSpace learningSpace);

    /// <summary>
    /// Asynchronously handles the deletion of a learning space by applying business logic
    /// </summary>
    /// <param name="learningSpaceId">The identifier of the learning space that will be deleted.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is <c>true</c> if the learning space was successfully deleted; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> DeleteLearningSpaceAsync(int learningSpaceId);
}