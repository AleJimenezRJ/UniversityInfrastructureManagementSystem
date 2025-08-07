using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Interface that defines service operations related to projectors.
/// </summary>
public interface IProjectorServices
{

    /// <summary>
    /// Adds a new projector to the system.
    /// </summary>
    /// <param name="projector">The projector entity to update.</param>
    /// <returns>
    /// A task representing the asynchronous operation. Returns <c>true</c> if the projector was successfully added; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> AddProjectorAsync(int learningSpaceId, Projector projector);

    /// <summary>
    /// Update and existing projector in the system.
    /// </summary>
    /// <param name="projector">The projector entity to update.</param>
    /// </returns>
    Task<bool> UpdateProjectorAsync(int learningSpaceId, Projector projector);
}   