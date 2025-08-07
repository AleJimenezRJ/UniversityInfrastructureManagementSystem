namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Represents the context for managing permissions within the application.
/// </summary>
public class PermissionContext : IPermissionContext
{
    /// <summary>
    /// A list of permissions assigned to the user or context.
    /// </summary>
    private List<string> _permissions = new();

    /// <summary>
    /// Sets the permissions for the context, ensuring they are trimmed, distinct, and case-insensitive.
    /// </summary>
    /// <param name="permissions"> An enumerable collection of permission strings to set.</param>
    public void SetPermissions(IEnumerable<string> permissions)
    {
        _permissions = permissions.Select(p => p.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    /// <summary>
    /// Gets the list of permissions currently assigned to the context.
    /// </summary>
    /// <returns> An IReadOnlyList of permission strings.</returns>
    public IReadOnlyList<string> GetPermissions() => _permissions;

    /// <summary>
    /// Checks if the context has a specific permission.
    /// </summary>
    /// <param name="permission"> The permission string to check for.</param>
    /// <returns> True if the permission exists in the context, otherwise false.</returns>
    public bool HasPermission(string permission) =>
        _permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if the context has any of the specified permissions.
    /// </summary>
    /// <param name="permissions"> An array of permission strings to check against the context.</param>
    /// <returns> True if any of the specified permissions exist in the context, otherwise false.</returns>
    public bool HasAny(params string[] permissions) =>
        _permissions.Any(p => permissions.Contains(p, StringComparer.OrdinalIgnoreCase));
}
