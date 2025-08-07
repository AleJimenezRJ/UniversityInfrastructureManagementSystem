using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;

/// <summary>
/// Provides implementation for permissions-related operations defined in <see cref="IPermissionService"/>.
/// </summary>
internal class PermissionService : IPermissionService
{
    /// <summary>
    /// Constructor for the <see cref="PermissionService"/> class.
    /// </summary>
    private readonly IPermissionRepository _permissionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionService"/> class.
    /// </summary>
    /// <param name="_permissionRepository">The repository used to access permission data.</param>
    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    /// <summary>
    /// Retrieves a list of permissions
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task contains a list of <see cref="Permission"/> objects if found,
    /// or an empty list if no permission exists in the data base.
    /// </returns>
    public async Task<List<Permission>> GetAllPermissionsAsync()
    {
        return await _permissionRepository.GetAllPermissionsAsync();
    }
}
