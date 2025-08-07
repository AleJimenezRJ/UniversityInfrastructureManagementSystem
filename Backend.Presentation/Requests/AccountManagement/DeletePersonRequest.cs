namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to delete a person by their identity number.
/// </summary>
public class DeletePersonRequest
{
    /// <summary>
    /// The identity number of the person to be deleted.
    /// </summary>
    public string IdentityNumber { get; set; } = string.Empty;
}
