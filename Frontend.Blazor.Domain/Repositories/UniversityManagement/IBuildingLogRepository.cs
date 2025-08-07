using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;

/// <summary>
/// Defines the contract for a repository that manages building logs.
/// </summary>
public interface IBuildingLogRepository
{
    /// <summary>
    /// Asynchronously retrieves a list of all building logs.
    /// </summary>
    /// <returns></returns>
    Task<List<BuildingLog>> ListBuildingLogsAsync();
}