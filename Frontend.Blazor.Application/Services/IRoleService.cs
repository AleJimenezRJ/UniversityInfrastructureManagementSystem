using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Defines the contract for role-related operations in the application.
/// This interface is intended to be implemented by services that handle role management functionality.
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Deletes a role asynchronously by their unique identifier.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains <c>true</c> if the role was successfully deleted; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> DeleteRoleAsync(int roleId);

    /// <summary>
    /// Retrieves a list of roles
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task contains a list of <see cref="Role"/> objects if found,
    /// or an empty list if no role exists in the data base.
    /// </returns>
    Task<List<Role>> GetAllRolesAsync();
}
