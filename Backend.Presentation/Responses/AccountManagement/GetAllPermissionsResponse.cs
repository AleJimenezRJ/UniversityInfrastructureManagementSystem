using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Response class for retrieving all permissions.
/// </summary>
public class GetAllPermissionsResponse
{
    /// <summary>
    /// Gets or sets the list of permissions retrieved from the system.
    /// </summary>
    public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
}
