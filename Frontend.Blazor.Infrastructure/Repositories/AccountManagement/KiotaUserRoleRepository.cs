using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Repository implementation that interacts with Kiota to manage user-role data.
/// </summary>
internal class KiotaUserRoleRepository : IUserRoleService
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaUserRoleRepository"/> class.
    /// </summary>
    /// <param name="apiClient">The Kiota API client used to interact with the backend services.</param>
    public KiotaUserRoleRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Retrieves the list of roles assigned to a specific user by their user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of <see cref="Role"/> objects assigned to the user,
    /// or an empty list if no roles are found.
    /// </returns>
    public async Task<List<Role>?> GetRolesByUserIdAsync(int userId)
    {
        var result = await _apiClient.Users[userId].Roles.GetAsync();

        if (result?.Roles == null || !result.Roles.Any())
            return new List<Role>();

        return result.Roles
          .Where(r => r.Id.HasValue && !string.IsNullOrWhiteSpace(r.Name))
          .Select(r => new Role(r.Id!.Value, r.Name!))
          .ToList();
    }

    /// <summary>
    /// Assigns a specific role to a user.
    /// </summary>
    /// <param name="userId">The identifier of the user to whom the role will be assigned.</param>
    /// <param name="roleId">The identifier of the role to be assigned.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <c>true</c> if the assignment was successful, or <c>false</c> otherwise.
    /// </returns>
    public Task<bool> AssignRoleAsync(int userId, int roleId)
      => throw new NotImplementedException();

    /// <summary>
    /// Removes a specific role from a user.
    /// </summary>
    /// <param name="userId">The identifier of the user from whom the role will be removed.</param>
    /// <param name="roleId">The identifier of the role to be removed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains <c>true</c> if the removal was successful, or <c>false</c> otherwise.
    /// </returns>
    public Task<bool> RemoveRoleAsync(int userId, int roleId)
      => throw new NotImplementedException();

    /// <summary>
    /// Retrieves a user-role association by its unique identifiers.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="UserRole"/> object if found, or <c>null</c> otherwise.
    /// </returns>
    public Task<UserRole?> GetByUserAndRoleAsync(int roleId, int userId)
      => throw new NotImplementedException();
}
