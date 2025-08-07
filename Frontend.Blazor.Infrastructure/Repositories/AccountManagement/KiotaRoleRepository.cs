using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// KiotaRoleRepository is an implementation of the IRoleRepository interface that uses the Kiota API client to manage roles.
/// </summary>
internal class KiotaRoleRepository : IRoleRepository
{
    /// <summary>
    /// The Kiota API client used to interact with the backend API for role management.
    /// </summary>
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaRoleRepository"/> class with the specified API client.
    /// </summary>
    /// <param name="apiClient"> The Kiota API client used to make requests to the backend API.</param>
    public KiotaRoleRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Removes a role asynchronously.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public Task<bool> DeleteRoleAsync(int roleId) => throw new NotImplementedException();


    /// <summary>
    /// Retrieves all roles from the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Role"/> objects
    /// if any exist, or an empty list if no roles are found in the database.
    public async Task<List<Role>> GetAllRolesAsync()
    {
        var response = await _apiClient.Role.List.GetAsync();

        if (response == null || response.Roles is null || !response.Roles.Any())
            return new List<Role>();

        return response.Roles.Select(dto => dto.ToEntity()).ToList();
    }

}