using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Defines the contract for learning space log service operations.
/// </summary>
public interface ILearningSpaceLogServices
{
    /// <summary>
    /// Asynchronously retrieves a list of all learning space logs.
    /// </summary>
    /// <returns>A list of <see cref="LearningSpaceLog"/> entries representing logged changes.</returns>
    Task<List<LearningSpaceLog>> ListLearningSpaceLogsAsync();
}
