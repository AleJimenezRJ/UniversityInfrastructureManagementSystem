namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to delete a user-role association.
/// </summary>
public class DeleteUserRoleRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the user to be deleted.
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the role to be deleted.
    /// </summary>
    public int RoleId { get; set; }
}