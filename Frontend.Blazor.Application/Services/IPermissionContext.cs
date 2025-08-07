namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Interface for managing permissions in the application context.
/// </summary>
public interface IPermissionContext
{
    /// <summary>
    /// Sets the permissions for the current context.
    /// </summary>
    /// <param name="permissions"> The permissions to set.</param>
    void SetPermissions(IEnumerable<string> permissions);

    /// <summary>
    /// Retrieves the permissions for the current context.
    /// </summary>
    /// <returns> A read-only list of permissions.</returns>
    IReadOnlyList<string> GetPermissions();

    /// <summary>
    /// Checks if the current context has a specific permission.
    /// </summary>
    /// <param name="permission"> The permission to check.</param>
    /// <returns> True if the permission exists, otherwise false.</returns>
    bool HasPermission(string permission);

    /// <summary>
    /// Checks if the current context has any of the specified permissions.
    /// </summary>
    /// <param name="permissions"> The permissions to check.</param>
    /// <returns> True if any of the specified permissions exist, otherwise false.</returns>
    bool HasAny(params string[] permissions);
}
