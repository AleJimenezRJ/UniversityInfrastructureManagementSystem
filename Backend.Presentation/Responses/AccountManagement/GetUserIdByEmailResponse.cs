using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response returned when retrieving a user's ID by their email.
/// </summary>
public class GetUserIdByEmailResponse
{
    public UserIdDto? User { get; set; }
}
