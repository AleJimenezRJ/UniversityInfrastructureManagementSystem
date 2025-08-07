using Microsoft.Kiota.Abstractions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.Spaces;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.Spaces;

/// <summary>
/// KiotaLearningSpaceLogRepository is an implementation of ILearningSpaceLogRepository that uses Kiota to interact with the API.
/// </summary>
internal class KiotaLearningSpaceLogRepository : ILearningSpaceLogRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Constructor for the KiotaLearningSpaceLogRepository class.
    /// </summary>
    /// <param name="apiClient"></param>
    public KiotaLearningSpaceLogRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Asynchronously retrieves a list of all learning space logs.
    /// </summary>
    /// <returns>A list of <see cref="LearningSpaceLog"/> entries representing historical changes.</returns>
    public async Task<List<LearningSpaceLog>> ListLearningSpaceLogsAsync()
    {
        try
        {
            var response = await _apiClient.ListLearningSpaceLogs.GetAsync();
            
            if (response?.Logs == null)
            {
                return new List<LearningSpaceLog>();
            }

            return response.Logs.ToEntity();
        }
        catch (ApiException ex)
        {
            switch (ex.ResponseStatusCode)
            {
                case 404:
                    return new List<LearningSpaceLog>();
                default:
                    throw new DomainException("Ocurrió un error inesperado en el servidor. Por favor, intente nuevamente más tarde.");
            }
        }
    }
}

