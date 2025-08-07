using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Service for managing whiteboard components.
/// </summary>
internal class WhiteboardServices : IWhiteboardServices
{
    /// <summary>
    /// The repository used to interact with whiteboard data.
    /// </summary>
    private readonly IWhiteboardRepository _whiteboardRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="WhiteboardServices"/> class.
    /// </summary>
    /// <param name="whiteboardRepository"></param>
    public WhiteboardServices(IWhiteboardRepository whiteboardRepository)
    {
        _whiteboardRepository = whiteboardRepository;
    }


    /// <summary>
    /// Adds a new whiteboard to the system.
    /// </summary>
    /// <param name="whiteboard">The whiteboard to add.</param>
    /// <returns><c>true</c> if the whiteboard was successfully added; otherwise, <c>false</c>.</returns>
    public async Task<bool> AddWhiteboardAsync(int learningSpaceId, Whiteboard whiteboard)
    {
        return await _whiteboardRepository.AddComponentAsync(learningSpaceId, whiteboard);
    }

    /// <summary>

    /// Update an existing Whiteboard in the system.
    /// </summary>
    /// <param name="whiteboard">The whiteboard entity to update.</param>
    /// </returns>
    public Task<bool> UpdateWhiteboardAsync(int learningSpaceId, Whiteboard whiteboard)
    {
        return _whiteboardRepository.UpdateAsync(learningSpaceId, whiteboard);
    }

    /// Deletes a whiteboard from the system based on its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteWhiteboardAsync(int id)
    {
        return await _whiteboardRepository.DeleteComponentAsync(id);

    }
}