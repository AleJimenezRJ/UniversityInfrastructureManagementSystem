using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;

/// <summary>
/// Repository interface for managing whiteboard components.
/// </summary>
public interface IWhiteboardRepository : ILearningComponentRepository
{
    /// <summary>
    /// Retrieves all whiteboards in the system.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Whiteboard>> GetAllAsync();
}