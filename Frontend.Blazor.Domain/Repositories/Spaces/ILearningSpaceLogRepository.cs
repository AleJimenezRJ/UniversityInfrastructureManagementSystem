using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.Spaces;

/// <summary>
/// Defines the contract for a repository that manages learning space logs.
/// </summary>
public interface ILearningSpaceLogRepository
{
    /// <summary>
    /// Asynchronously retrieves a list of all learning space logs.
    /// </summary>
    /// <returns>A list of <see cref="LearningSpaceLog"/> entries representing historical changes.</returns>
    Task<List<LearningSpaceLog>> ListLearningSpaceLogsAsync();
}
