namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

/// <summary>
/// Data Transfer Object representing a permission assigned to a user or role.
/// </summary>
/// <param name="Id">Unique identifier of the permission.</param>
/// <param name="Description">Description of the permission.</param>
public record class PermissionDto(int Id, string Description);
