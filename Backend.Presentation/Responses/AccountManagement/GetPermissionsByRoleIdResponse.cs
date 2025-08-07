using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response containing a list of permissions associated with a specific role.
/// </summary>
public class GetPermissionsByRoleIdResponse
{
    /// <summary>
    /// Gets the list of permissions for the specified role.
    /// </summary>
    public List<PermissionDto> Permissions { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPermissionsByRoleIdResponse"/> class.
    /// </summary>
    /// <param name="permissions">The list of permissions associated with the role.</param>
    public GetPermissionsByRoleIdResponse(List<PermissionDto> permissions)
    {
        Permissions = permissions;
    }
}
