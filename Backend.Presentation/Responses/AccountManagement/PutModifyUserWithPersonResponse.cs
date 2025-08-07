using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Response returned after modifying a user with associated person details.
/// </summary>
/// <param name="userWithPersonDto"></param>
public record class PutModifyUserWithPersonResponse(CreateUserWithPersonDto userWithPersonDto);
