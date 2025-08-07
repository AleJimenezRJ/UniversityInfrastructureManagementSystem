using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;

/// <summary>
/// Service implementation for managing role-permission associations.
/// Provides methods to retrieve permissions assigned to a specific role.
/// </summary>
internal class RolePermissionService : IRolePermissionService
{
    private readonly IRolePermissionRepository _rolePermissionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolePermissionService"/> class.
    /// </summary>
    /// <param name="rolePermissionRepository">The repository for accessing role-permission data.</param>
    public RolePermissionService(IRolePermissionRepository rolePermissionRepository)
    {
        _rolePermissionRepository = rolePermissionRepository;
    }

    /// <summary>
    /// Retrieves the list of permissions associated with the specified role identifier.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Permission"/> objects
    /// assigned to the role, or <c>null</c> if the role does not exist or has no permissions.
    /// </returns>
    public async Task<List<Permission>?> ViewPermissionsByRoleIdAsync(int roleId)
    {
        return await _rolePermissionRepository.ViewPermissionsByRoleIdAsync(roleId);
    }

    /// <summary>
    /// Assigns a permission to a role.
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="permId"></param>
    /// <returns></returns>
    public async Task<bool> AssignPermissionToRoleAsync(int roleId, int permId)
    {
        return await _rolePermissionRepository.AssignPermissionToRoleAsync(roleId, permId);
    }

    /// <summary>
    /// Removes a permission from a role.
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="permId"></param>
    /// <returns></returns>
    public async Task<bool> RemovePermissionFromRoleAsync(int roleId, int permId)
    {
        return await _rolePermissionRepository.RemovePermissionFromRoleAsync(roleId, permId);
    }
}
