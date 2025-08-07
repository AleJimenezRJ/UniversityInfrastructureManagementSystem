using Microsoft.Kiota.Abstractions;
using System.Linq;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// KiotaUserAuditRepository is an implementation of IUserAuditRepository that uses Kiota to interact with the API.
/// </summary>
internal class KiotaUserAuditRepository : IUserAuditRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Constructor for the KiotaUserAuditRepository class.
    /// </summary>
    /// <param name="apiClient"></param>
    public KiotaUserAuditRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Retrieves a list of user audits asynchronously.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation, containing a list of UserAudit entities.
    /// </returns>
    public async Task<List<UserAudit>> ListUserAuditAsync()
    {
        var response = await _apiClient.Auditlogs.GetAsync();

        return response!.Select(dto => dto.ToEntity()).ToList();
    }

    /// <summary>
    /// Retrieves a paginated list of user audit logs.
    /// </summary>
    /// <param name="pageSize">The number of audit logs to include in each page. Must be greater than zero.</param>
    /// <param name="pageNumber">The page number to retrieve. Must be greater than or equal to one.</param>
    /// <returns>A <see cref="PaginatedList{T}"/> containing user audit logs for the specified page. If no audit logs are
    /// available, returns an empty paginated list.</returns>
    public async Task<PaginatedList<UserAudit>> GetPaginatedUserAuditAsync(int pageSize, int pageNumber)
    {
        var response = await _apiClient.Auditlogs.Paginated.GetAsync(options =>
        {
            options.QueryParameters.PageSize = pageSize;
            options.QueryParameters.PageNumber = pageNumber;
        });

        if (response == null || response.Audits == null)
        {
            return PaginatedList<UserAudit>.Empty(pageSize, pageNumber);
        }

        var audits = response.Audits.Select(dto => dto.ToEntity()).ToList();

        return new PaginatedList<UserAudit>(
            audits,
            response.TotalCount ?? 0,
            pageSize,
            response.PageNumber ?? 0
        );
    }

}

