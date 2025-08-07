using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Service for managing generic learning components (projectors, whiteboards, etc.).
/// </summary>
internal class LearningComponentServices : ILearningComponentServices
{
    /// <summary>
    /// Repository for accessing learning component data.
    /// </summary>
    private readonly ILearningComponentRepository _learningComponentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningComponentServices"/> class.
    /// </summary>
    /// <param name="learningComponentRepository"></param>
    public LearningComponentServices(ILearningComponentRepository learningComponentRepository)
    {
        _learningComponentRepository = learningComponentRepository;
    }

    /// <summary>
    /// Retrieves all learning components in the system.
    /// </summary>
    /// <param name="pageSize">The maximum number of learning components to retrieve per page.</param>
    /// <param name="pageIndex">The page index for pagination.</param>
    /// <returns>A list of all learning components.</returns>
    public async Task<IEnumerable<LearningComponent>> GetLearningComponentAsync(int pageSize, int pageIndex)
    {
        return await _learningComponentRepository.GetAllAsync(pageSize, pageIndex);
    }

    /// <summary>
    /// Retrieves learning components for a specific learning space.
    /// </summary>
    /// <param name="learningSpaceId">Identifier of the learning space (e.g., classroom, lab).</param>
    /// <param name="pageSize">The maximum number of learning components to retrieve per page.</param>
    /// <param name="pageIndex">The page index for pagination.</param>
    /// <returns>A list of learning components assigned to the specified learning space.</returns>
    public async Task<PaginatedList<LearningComponent>> GetLearningComponentsByIdAsync(int learningSpaceId, int pageSize, int pageIndex, string stringSearch)
    {
        return await _learningComponentRepository.GetLearningComponentsByIdAsync(learningSpaceId, pageSize, pageIndex, stringSearch);
    }

    /// <summary>
    /// Asynchronously retrieves a single learning component by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the learning component to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, containing the <see cref="LearningComponent"/> if found.</returns>
    public async Task<LearningComponent> GetSingleLearningComponentByIdAsync(int id)
    {
        return await _learningComponentRepository.GetSingleLearningComponentAsync(id);
    }

    /// <summary>
    /// Asynchronously deletes a learning component by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the learning component to be deleted.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean indicating whether the deletion was successful.</returns>
    public async Task<bool> DeleteLearningComponentAsync(int id)
    {
        return await _learningComponentRepository.DeleteComponentAsync(id);
    }

    public async Task<bool> UpdateLearningComponentAsync(int learningSpaceId, LearningComponent learningComponent)
    {
        return await _learningComponentRepository.UpdateAsync(learningSpaceId, learningComponent);
    }
}
