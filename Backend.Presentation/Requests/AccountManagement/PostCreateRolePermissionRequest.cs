using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to assign a permission to a role.
/// </summary>
/// <param name="RolePermission">The role-permission association to be created.</param>
public record class PostCreateRolePermissionRequest(RolePermissionDto RolePermission);