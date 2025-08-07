using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;

/// <summary>
/// Provides implementation for user-role-related operations defined in <see cref="IUserRoleService"/>.
/// </summary>
internal class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository _userRoleRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRoleService"/> class.
    /// </summary>
    /// <param name="userRoleRepository"></param>
    public UserRoleService(IUserRoleRepository userRoleRepository)
    {
        _userRoleRepository = userRoleRepository;
    }

    /// <summary>
    /// Assigns a role to a user asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<bool> AssignRoleAsync(int userId, int roleId)
    {
        var userRole = await _userRoleRepository.GetByUserAndRoleAsync(userId, roleId);
        if (userRole != null)
        {
            return false;
        }
        var newUserRole = new UserRole(userId, roleId);

        await _userRoleRepository.AddAsync(newUserRole);
        return true;
    }

    /// <summary>
    /// Removes a role from a user asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<bool> RemoveRoleAsync(int userId, int roleId)
    {
        var existing = await _userRoleRepository.GetByUserAndRoleAsync(userId, roleId);
        if (existing == null)
            return false;

        return await _userRoleRepository.RemoveAsync(existing);
    }

    /// <summary>
    /// Retrieves the roles assigned to a user asynchronously.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<Role>?> GetRolesByUserIdAsync(int userId)
    {
        var roles = await _userRoleRepository.GetRolesByUserIdAsync(userId);
        return roles;
    }

    /// <summary>
    /// Retrieves a user-role association by its unique identifier.
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<UserRole?> GetByUserAndRoleAsync(int roleId, int userId)
    {
        return await _userRoleRepository.GetByUserAndRoleAsync(roleId, userId);
    }

}
