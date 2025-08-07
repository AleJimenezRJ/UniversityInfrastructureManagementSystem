using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;

/// <summary>
/// Defines the contract for managing user-role associations within the system.
/// </summary>
public interface IUserRoleRepository
{
    /// <summary>
    /// Retrieves a user-role association by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user-role association.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the <see cref="UserRole"/> 
    /// object if found, or <c>null</c> if no such association exists.
    /// </returns>
    Task<UserRole?> GetUserRoleAsync(int id);

    /// <summary>
    /// Persists a new user-role association in the data store.
    /// </summary>
    /// <param name="userRole">The user-role association to be added.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<bool> AddAsync(UserRole userRole);

    /// <summary>
    /// Removes an existing user-role association from the data store.
    /// </summary>
    /// <param name="userRole">The user-role association to be removed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<bool> RemoveAsync(UserRole userRole);

    /// <summary>
    /// Retrieves a user-role association by user and role identifiers.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="roleId">The identifier of the role.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the <see cref="UserRole"/> 
    /// object if found, or <c>null</c> if no association exists for the given user and role.
    /// </returns>
    Task<UserRole?> GetByUserAndRoleAsync(int userId, int roleId);

    /// <summary>
    /// Retrieves a list of roles associated with a specific user.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of <see cref="Role"/> 
    /// objects associated with the user.
    /// </returns>
    Task<List<Role>?> GetRolesByUserIdAsync(int userId);

    /// <summary>
    /// Assigns a role to a user asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<bool> AssignRoleAsync(int userId, int roleId);
}
