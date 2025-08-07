namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to delete a role-permission association.
/// </summary>
public class DeleteRolePermissionRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the role to be deleted.
    /// </summary>
    public int RoleId { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the permission to be deleted.
    /// </summary>
    public int PermId { get; set; }
}