using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Interface that defines service operations related to whiteboards.
/// </summary>
public interface IWhiteboardServices
{

    /// <summary>
    /// Adds a new whiteboard to the system.
    /// </summary>
    /// <param name="whiteboard">The whiteboard entity to add.</param>
    /// </returns>
    Task<bool> AddWhiteboardAsync(int learningSpaceId, Whiteboard whiteboard);

    /// <summary>
    /// Update an existing Whiteboard in the system.
    /// </summary>
    /// <param name="whiteboard">The whiteboard entity to update.</param>
    /// </returns>
    public Task<bool> UpdateWhiteboardAsync(int learningSpaceId, Whiteboard whiteboard);

    /// Deletes a whiteboard from the system based on its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteWhiteboardAsync(int id);

}
