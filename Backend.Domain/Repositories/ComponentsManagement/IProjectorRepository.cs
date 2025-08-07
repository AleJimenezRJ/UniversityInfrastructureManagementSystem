using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;

/// <summary>
/// Repository interface for managing projector components.
/// </summary>
public interface IProjectorRepository : ILearningComponentRepository
{

    /// <summary>
    /// Retrieves all projectors in the system.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Projector>> GetAllAsync();
}