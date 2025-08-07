using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.ComponentsManagement;

internal class KiotaLearningComponentAuditRepository : ILearningComponentAuditRepository
{
    private readonly ApiClient _apiClient;

    public KiotaLearningComponentAuditRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<LearningComponentAudit>> ListLearningComponentAuditAsync()
    {
        var response = await _apiClient.LearningComponentLogs.GetAsync();
        Console.WriteLine(response?.GetType().FullName);

        return response!.Select(dto => dto.ToEntity()).ToList();

  
    }


    public async Task<PaginatedList<LearningComponentAudit>> GetPaginatedLearningComponentAuditAsync(int pageSize, int pageIndex)
    {
        var response = await _apiClient.LearningComponentLogs.Paginated
            .GetAsync(config =>
            {
                config.QueryParameters.PageSize = pageSize;
                config.QueryParameters.PageIndex = pageIndex;
            });

        if (response == null || response.Audits == null)
        {
            return PaginatedList<LearningComponentAudit>.Empty(pageSize, pageIndex);
        }

        var audits = response.Audits.Select(dto => dto.ToEntity()).ToList();

        return new PaginatedList<LearningComponentAudit>(
            audits,
            (int)response.TotalCount!,
            (int)response.PageSize!,
            (int)response.PageNumber!
        );
    }
}

