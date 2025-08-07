namespace UCR.ECCI.PI.ThemePark.Backend.Application.Requests.AccountManagement;

/// <summary>
/// Request object for retrieving permissions associated with a specific role.
/// </summary>
public class GetPermissionsByRoleIdRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the role.
    /// </summary>
    public int RoleId { get; set; }
}
