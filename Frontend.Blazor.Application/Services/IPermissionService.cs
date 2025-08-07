using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Defines the contract for permissions-related operations in the application.
/// This interface is intended to be implemented by services that handle permissions management functionality.
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Retrieves a list of permissions
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task contains a list of <see cref="Permission"/> objects if found,
    /// or an empty list if no permission exists in the data base.
    /// </returns>
    Task<List<Permission>> GetAllPermissionsAsync();
}
