using System.Linq;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Repository for managing role permissions using Kiota API client.
/// </summary>
public class KiotaRolePermissionRepository : IRolePermissionRepository
{
    /// <summary>
    /// The Kiota API client used to interact with the backend API for role permissions.
    /// </summary>
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaRolePermissionRepository"/> class.
    /// </summary>
    /// <param name="apiClient"> The Kiota API client used to make requests to the backend API.</param>
    public KiotaRolePermissionRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Retrieves a list of permissions associated with a specific role by its ID.
    /// </summary>
    /// <param name="roleId"> The ID of the role for which permissions are to be retrieved.</param>
    /// <returns> A task that represents the asynchronous operation, containing a list of permissions associated with the role.</returns>
    public async Task<List<Permission>?> ViewPermissionsByRoleIdAsync(int roleId)
    {
        var result = await _apiClient.Roles[roleId].Permissions.GetAsync();

        if (result?.Permissions == null)
            return new List<Permission>();

        return result.Permissions
            .Where(p => p.Id.HasValue && !string.IsNullOrWhiteSpace(p.Description))
            .Select(p => new Permission(p.Description!, p.Id!.Value))
            .ToList();
    }

    /// <summary>
    /// Assigns a permission to a role by their IDs asynchronously.
    /// </summary>
    /// <param name="roleId"> The ID of the role to which the permission will be assigned.</param>
    /// <param name="permId"> The ID of the permission to be assigned to the role.</param>
    /// <returns> A task that represents the asynchronous operation, returning true if the permission was successfully assigned; otherwise, false.</returns>
    /// <exception cref="NotImplementedException"> Thrown when the method is not implemented.</exception>
    public Task<bool> AssignPermissionToRoleAsync(int roleId, int permId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes a permission from a role by their IDs asynchronously.
    /// </summary>
    /// <param name="roleId"> The ID of the role from which the permission will be removed.</param>
    /// <param name="permId"> The ID of the permission to be removed from the role.</param>
    /// <returns> A task that represents the asynchronous operation, returning true if the permission was successfully removed; otherwise, false.</returns>
    /// <exception cref="NotImplementedException"> Thrown when the method is not implemented.</exception>
    public Task<bool> RemovePermissionFromRoleAsync(int roleId, int permId)
    {
        throw new NotImplementedException();
    }
}
