using Microsoft.AspNetCore.Authorization;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Authorization;

/// <summary>
/// Represents a requirement for authorization based on specific permissions.
/// </summary>
public class PermissionsRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Gets the required permission for this authorization requirement.
    /// </summary>
    public string RequiredPermission { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionsRequirement"/> class with the specified required permission.
    /// </summary>
    /// <param name="requiredPermission"> The permission that is required for authorization.</param>
    public PermissionsRequirement(string requiredPermission)
    {
        RequiredPermission = requiredPermission;
    }
}
