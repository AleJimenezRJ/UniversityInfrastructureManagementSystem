using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response returned when retrieving all registered user with person.
/// </summary>
public class GetAllUsersWithPersonResponse
{
    /// <summary>
    /// Gets or sets the list of user with person retrieved from the system.
    /// </summary>
    public List<UserWithPersonDto> Users { get; set; } = new();
}
