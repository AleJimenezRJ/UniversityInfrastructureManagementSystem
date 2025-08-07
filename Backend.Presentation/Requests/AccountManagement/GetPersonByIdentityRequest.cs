namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;


/// <summary>
/// Represents a request to retrieve a person by identity number.
/// </summary>
public class GetPersonByIdentityRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the person whose roles are to be retrieved.
    /// </summary>
    public string IdentityNumber { get; set; } = string.Empty;
}
