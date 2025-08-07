using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Response returned after modifying a user.
/// </summary>
/// <param name="User">The modified user data.</param>
/// <param name="Message">A message indicating the result of the operation.</param>
public record class PutModifyUserResponse(ModifyUserDto User, string Message);
