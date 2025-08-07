using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Provides implementation for role-related operations defined in <see cref="IRoleService"/>.
/// </summary>
internal class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleService"/> class.
    /// </summary>
    /// <param name="roleRepository">The repository used to access role data.</param>
    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    /// <summary>
    /// Removes a role asynchronously.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public Task<bool> DeleteRoleAsync(int id)
    {
        return _roleRepository.DeleteRoleAsync(id);
    }

    /// <summary>
    /// Retrieves a list of all roles in the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Role"/> objects
    /// if found, or an empty list if no role exists in the database.
    /// </returns>
    public Task<List<Role>> GetAllRolesAsync()
    {
        return _roleRepository.GetAllRolesAsync();
    }

}
