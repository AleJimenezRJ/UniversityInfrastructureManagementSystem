using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services;

/// <summary>
/// Interface that defines service operations related to whiteboards.
/// </summary>
public interface IWhiteboardServices
{
    /// <summary>
    /// Retrieves all whiteboards currently stored in the system.
    /// </summary>
    /// </returns>
    Task<IEnumerable<Whiteboard>> GetWhiteboardAsync();

    /// <summary>
    /// Adds a new whiteboard to the system.
    /// </summary>
    /// <param name="whiteboard">The whiteboard entity to add.</param>
    /// </returns>
    Task<bool> AddWhiteboardAsync(int learningSpaceId, Whiteboard whiteboard);

    /// <summary>
    /// Update an existing Whiteboard in the system.
    /// </summary>
    /// <param name="learningSpaceId"></param>
    /// <param name="learningComponentId"></param>
    /// <param name="whiteboard">The whiteboard entity to update.</param>
    /// </returns>
    public Task<bool> UpdateWhiteboardAsync(int learningSpaceId, int learningComponentId, Whiteboard whiteboard);
}
