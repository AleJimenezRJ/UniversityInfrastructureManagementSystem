namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to delete a UserWithPerson.
/// </summary>
public class DeleteUserWithPersonRequest
{
    public int UserId { get; set; }
    public int PersonId { get; set; }
}
