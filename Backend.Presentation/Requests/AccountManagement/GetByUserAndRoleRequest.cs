namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to retrieve a user-role association by user ID and role ID.
/// </summary>
public class GetByUserAndRoleRequest
{
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// The unique identifier of the role.
    /// </summary>
    public int RoleId { get; set; }
}