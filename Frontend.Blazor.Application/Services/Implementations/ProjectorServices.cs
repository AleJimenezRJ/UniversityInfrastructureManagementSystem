using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Service for managing projector components.
/// </summary>
internal class ProjectorServices : IProjectorServices
{
    /// <summary>
    /// The repository used to interact with projector data.
    /// </summary>
    private readonly IProjectorRepository _projectorRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectorServices"/> class.
    /// </summary>
    /// <param name="projectorRepository"></param>
    public ProjectorServices(IProjectorRepository projectorRepository)
    {
        _projectorRepository = projectorRepository;
    }

    /// <summary>
    /// Adds a new projector to the system.
    /// </summary>
    /// <param name="projector">The projector to add.</param>
    /// <returns><c>true</c> if the projector was successfully added; otherwise, <c>false</c>.</returns>
    public async Task<bool> AddProjectorAsync(int learningSpaceId, Projector projector)
    {
        return await _projectorRepository.AddComponentAsync(learningSpaceId, projector);
    }

    /// <summary>
    /// Update and existing projector in the system.
    /// </summary>
    /// <param name="projector">The projector entity to update.</param>
    /// </returns>
    public Task<bool> UpdateProjectorAsync(int learningSpaceId, Projector projector)
    {
        return _projectorRepository.UpdateAsync(learningSpaceId, projector);
    }
}