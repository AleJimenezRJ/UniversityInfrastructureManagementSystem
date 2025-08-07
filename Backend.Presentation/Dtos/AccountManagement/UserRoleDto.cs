namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

/// <summary>
/// Data Transfer Object (DTO) representing a user-role association.
/// </summary>
/// <param name="UserId">The unique identifier of the user.</param>
/// <param name="RoleId">The unique identifier of the role assigned to the user.</param>
public record class UserRoleDto(int UserId, int RoleId);
