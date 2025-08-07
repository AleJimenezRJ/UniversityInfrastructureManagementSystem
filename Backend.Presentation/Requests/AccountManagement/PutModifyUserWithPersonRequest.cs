using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Request model for modifying an existing user with associated person information.
/// </summary>
public record class PutModifyUserWithPersonRequest
{
    public string IdentityNumber { get; set; } = null!;
    public CreateUserWithPersonDto UserWithPerson { get; init; } = null!;
}
