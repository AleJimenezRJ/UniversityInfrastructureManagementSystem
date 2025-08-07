using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Defines the contract for services that manage the association between roles and permissions.
/// Provides methods to retrieve permissions assigned to a specific role.
/// </summary>
public interface IRolePermissionService
{
    /// <summary>
    /// Retrieves the list of permissions associated with the specified role identifier.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Permission"/> objects
    /// associated with the role, or <c>null</c> if the role does not exist or has no permissions.
    /// </returns>
    Task<List<Permission>?> ViewPermissionsByRoleIdAsync(int roleId);

    /// <summary>
    /// Assigns a permission to a role.
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="permId"></param>
    /// <returns></returns>
    Task<bool> AssignPermissionToRoleAsync(int roleId, int permId);

    /// <summary>
    /// Removes a permission from a role.
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="permId"></param>
    /// <returns></returns>
    Task<bool> RemovePermissionFromRoleAsync(int roleId, int permId);
}
