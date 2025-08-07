using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Defines the contract for application-level operations related to user-role management.
/// </summary>
public interface IUserRoleService
{
    /// <summary>
    /// Assigns a specific role to a user.
    /// </summary>
    /// <param name="userId">The identifier of the user to whom the role will be assigned.</param>
    /// <param name="roleId">The identifier of the role to be assigned.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <c>true</c> if the assignment 
    /// was successful, or <c>false</c> otherwise.
    /// </returns>
    Task<bool> AssignRoleAsync(int userId, int roleId);

    /// <summary>
    /// Removes a specific role from a user.
    /// </summary>
    /// <param name="userId">The identifier of the user from whom the role will be removed.</param>
    /// <param name="roleId">The identifier of the role to be removed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <c>true</c> if the removal 
    /// was successful, or <c>false</c> otherwise.
    /// </returns>
    Task<bool> RemoveRoleAsync(int userId, int roleId);

    /// <summary>
    /// Retrieves the names of roles assigned to a specific user.
    /// </summary>
    /// <param name="userId">The identifier of the user whose roles will be retrieved.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of role names 
    /// assigned to the user.
    /// </returns>
    Task<List<Role>?> GetRolesByUserIdAsync(int userId);

    /// <summary>
    /// Retrieves a user-role association by its unique identifier.
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<UserRole?> GetByUserAndRoleAsync(int roleId, int userId);
}
