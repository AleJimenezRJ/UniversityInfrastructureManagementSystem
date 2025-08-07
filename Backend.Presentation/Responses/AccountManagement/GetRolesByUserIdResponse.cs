using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response containing the list of roles associated with a specific user.
/// </summary>
public class GetRolesByUserIdResponse
{
    /// <summary>
    /// Gets the list of roles assigned to the user.
    /// </summary>
    public List<RoleDto>? Roles { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRolesByUserIdResponse"/> class.
    /// </summary>
    /// <param name="roles">The list of roles assigned to the user.</param>
    public GetRolesByUserIdResponse(List<RoleDto>? roles)
    {
        Roles = roles;
    }
}
