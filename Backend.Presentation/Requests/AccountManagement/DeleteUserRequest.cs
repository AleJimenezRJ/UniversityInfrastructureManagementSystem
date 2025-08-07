namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to delete a user by their unique Id.
/// </summary>
public class DeleteUserRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the user to be deleted.
    /// </summary>
    public int Id { get; set; }
}
