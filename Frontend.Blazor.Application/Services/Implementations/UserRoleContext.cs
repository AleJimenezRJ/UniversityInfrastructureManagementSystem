using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// In-memory scoped service that stores the current user's roles for the frontend.
/// </summary>
internal class UserRoleContext : IUserRoleContext
{
    /// <summary>
    /// List of roles assigned to the current user.
    /// </summary>
    private List<string> _roles = new();

    /// <summary>
    /// Sets the roles for the current user.
    /// </summary>
    /// <param name="roles"> The roles to assign to the current user.</param>
    public void SetRoles(IEnumerable<string> roles)
    {
        _roles = roles.ToList();
    }

    /// <summary>
    /// Gets the roles assigned to the current user.
    /// </summary>
    /// <returns> A read-only list of roles.</returns>
    public IReadOnlyList<string> GetRoles() => _roles;

    /// <summary>
    /// Checks if the current user has a specific role.
    /// </summary>
    /// <param name="role"> The role to check for.</param>
    /// <returns> True if the user has the role, otherwise false.</returns>
    public bool HasRole(string role) =>
        _roles.Contains(role, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if the current user has any of the specified roles.
    /// </summary>
    /// <param name="roles"> The roles to check for.</param>
    /// <returns> True if the user has any of the specified roles, otherwise false.</returns>
    public bool HasAny(params string[] roles) =>
        _roles.Any(r => roles.Contains(r, StringComparer.OrdinalIgnoreCase));
}
