using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to modify an existing user.
/// </summary>
/// <param name="User">The user data to be modified.</param>
public record class PutModifyUserRequest(ModifyUserDto User);
