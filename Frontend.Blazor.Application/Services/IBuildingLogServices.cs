using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Defines the contract for building log repository operations.
/// </summary>
public interface IBuildingLogServices
{
    /// <summary>
    /// Asynchronously retrieves a list of all building logs.
    /// </summary>
    /// <returns></returns>
    Task<List<BuildingLog>> ListBuildingLogsAsync();
}
