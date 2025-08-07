namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Provides access to the current user's roles in memory during their session.
/// </summary>
public interface IUserRoleContext
{
    /// <summary>
    /// Sets the roles for the current user session.
    /// </summary>
    /// <param name="roles"> The roles to set for the current user.</param>
    void SetRoles(IEnumerable<string> roles);

    /// <summary>
    /// Gets the roles for the current user session.
    /// </summary>
    /// <returns> A read-only list of roles assigned to the current user.</returns>
    IReadOnlyList<string> GetRoles();

    /// <summary>
    /// Checks if the current user has a specific role.
    /// </summary>
    /// <param name="role"> The role to check for.</param>
    /// <returns> True if the user has the specified role; otherwise, false.</returns>
    bool HasRole(string role);

    /// <summary>
    /// Checks if the current user has any of the specified roles.
    /// </summary>
    /// <param name="roles"> The roles to check for.</param>
    /// <returns> True if the user has any of the specified roles; otherwise, false.</returns>
    bool HasAny(params string[] roles);
}
