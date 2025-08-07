using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services;

/// <summary>
/// Interface that defines service operations related to projectors.
/// </summary>
public interface IProjectorServices
{
    /// <summary>
    /// Retrieves all projectors currently stored in the system.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation, returning a list of <see cref="Projector"/> objects.
    /// </returns>
    Task<IEnumerable<Projector>> GetProjectorAsync();

    /// <summary>
    /// Adds a new projector to the system.
    /// </summary>
    /// <param name="learningSpaceId"></param>
    /// <param name="projector">The projector entity to update.</param>
    /// <returns>
    /// A task representing the asynchronous operation. Returns <c>true</c> if the projector was successfully added; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> AddProjectorAsync(int learningSpaceId, Projector projector);

    /// <summary>
    /// Update and existing projector in the system.
    /// </summary>
    /// <param name="learningSpaceId"></param>
    /// <param name="learningComponentId"></param>
    /// <param name="projector">The projector entity to update.</param>
    /// </returns>
    Task<bool> UpdateProjectorAsync(int learningSpaceId, int learningComponentId, Projector projector);
}   