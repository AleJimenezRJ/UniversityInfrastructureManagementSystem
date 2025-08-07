using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to assign a role to a user.
/// </summary>
/// <param name="UserRole">The user-role association to be created.</param>
public record class PostUserRoleRequest(UserRoleDto UserRole);
