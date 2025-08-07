using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
/// <summary>
/// Interface for the Permission repository.
/// This interface defines the contract for the Permission repository.
/// </summary>
public interface IPermissionRepository
{
    /// <summary>
    /// Retrieves all permissions from the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Permission"/> objects
    /// if any exist, or an empty list if no roles are found in the database.
    Task<List<Permission>> GetAllPermissionsAsync();
}