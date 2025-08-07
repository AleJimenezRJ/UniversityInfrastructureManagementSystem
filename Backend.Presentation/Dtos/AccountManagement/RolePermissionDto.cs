namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

/// <summary>
/// Data Transfer Object (DTO) representing a role-permission association.
/// </summary>
/// <param name="RoleId">The unique identifier of the role.</param>
/// <param name="PermId">The unique identifier of the permission.</param>
public record class RolePermissionDto(int RoleId, int PermId);
