using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response returned when retrieving all registered roles.
/// </summary>
public class GetAllRolesResponse
{
    /// <summary>
    /// Gets or sets the list of roles retrieved from the system.
    /// </summary>
    public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
}