namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to delete a role by their unique Id.
/// </summary>
public class DeleteRoleRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the role to be deleted.
    /// </summary>
    public int Id { get; set; }
}
