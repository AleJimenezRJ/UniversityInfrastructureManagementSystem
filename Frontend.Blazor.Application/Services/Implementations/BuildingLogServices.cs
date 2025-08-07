using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Provides implementation for building log-related operations defined in <see cref="IBuildingLogServices"/>.
/// </summary>
internal class BuildingLogServices : IBuildingLogServices
{
    private readonly IBuildingLogRepository _buildingLogRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingLogServices"/> class.
    /// </summary>
    /// <param name="buildingLogRepository"></param>
    public BuildingLogServices(IBuildingLogRepository buildingLogRepository)
    {
        _buildingLogRepository = buildingLogRepository;
    }

    /// <summary>
    /// Retrieves a list of building logs asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task<List<BuildingLog>> ListBuildingLogsAsync()
    {
        return await _buildingLogRepository.ListBuildingLogsAsync();
    }
 
}
