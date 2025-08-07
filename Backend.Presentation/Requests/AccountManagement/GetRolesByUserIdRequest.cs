namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Request object for retrieving the roles associated with a specific user.
/// </summary>
public class GetRolesByUserIdRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the user whose roles are to be retrieved.
    /// </summary>
    public int UserId { get; set; }
}
