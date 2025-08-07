using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.UniversityManagement;


namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.UniversityManagement;

/// <summary>
/// Provides methods to interact with building logs through an API client.
/// </summary>
/// <remarks>This class is responsible for retrieving building logs using the provided <see
/// cref="ApiClient"/>.</remarks>
internal class KiotaBuildingLogRepository : IBuildingLogRepository
{
    /// <summary>
    /// Represents a client for interacting with the API.
    /// </summary>
    /// <remarks>This field is used internally to make API requests. It is initialized once and used
    /// throughout the lifetime of the containing class to ensure consistent API communication.</remarks>
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaBuildingLogRepository"/> class with the specified API client.
    /// </summary>
    /// <param name="apiClient">The API client used to interact with the Kiota building log service. Cannot be null.</param>
    public KiotaBuildingLogRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Asynchronously retrieves a list of building logs.
    /// </summary>
    /// <remarks>This method fetches building logs from an external API and converts them into <see
    /// cref="BuildingLog"/> entities.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="BuildingLog"/>
    /// objects.</returns>
    public async Task<List<BuildingLog>> ListBuildingLogsAsync()
    {
        var response = await _apiClient.ListBuildingLogs.GetAsync();

        return response!.Select(dto => dto.ToEntity()).ToList();
    }
}
