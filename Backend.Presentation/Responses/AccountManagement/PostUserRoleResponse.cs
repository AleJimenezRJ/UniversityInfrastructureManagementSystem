using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response for assigning a role to a user.
/// </summary>
/// <param name="UserRole">The user-role association that was created.</param>
/// <param name="Message">A message indicating the result of the operation.</param>
public record class PostUserRoleResponse(UserRoleDto UserRole, string Message);
