namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

/// <summary>
/// Interface for Role repository.
/// This interface defines the contract for the Role repository.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Removes a role asynchronously.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<bool> DeleteRoleAsync(int roleId);

    /// <summary>
    /// Retrieves all roles from the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Role"/> objects
    /// if any exist, or an empty list if no roles are found in the database.
    Task<List<Role>> GetAllRolesAsync();
}