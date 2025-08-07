namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

/// <summary>
/// Represents a Data Transfer Object (DTO) for a role.
/// </summary>
/// <param name="Id"> The unique id of the role.</param>
/// <param name="Name">The name of the role.</param>
public record class RoleDto(int Id, string Name);