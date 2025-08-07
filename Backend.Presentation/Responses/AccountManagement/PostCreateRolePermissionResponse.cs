using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response for assigning a permission to a role.
/// </summary>
/// <param name="RolePermission">The role-permission association that was created.</param>
/// <param name="Message">A message indicating the result of the operation.</param>
public record class PostCreateRolePermissionResponse(RolePermissionDto RolePermission, string Message);
