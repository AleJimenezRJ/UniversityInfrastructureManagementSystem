using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

/// <summary>
/// Provides mapping logic between <see cref="UserRole"/> domain entities and <see cref="UserRoleDto"/> representations.
/// </summary>
internal static class UserRoleDtoMapper
{
    /// <summary>
    /// Converts a <see cref="UserRole"/> entity to a <see cref="UserRoleDto"/>.
    /// </summary>
    /// <param name="entity">The user-role association entity.</param>
    /// <returns>A <see cref="UserRoleDto"/> with basic mapping.</returns>
    internal static UserRoleDto ToDto(UserRole entity)
    {
        return new UserRoleDto(entity.UserId, entity.RoleId);
    }

    /// <summary>
    /// Converts a <see cref="UserRoleDto"/> to a <see cref="UserRole"/> entity.
    /// </summary>
    /// <param name="dto">The DTO to convert.</param>
    /// <returns>A domain entity <see cref="UserRole"/>.</returns>
    internal static UserRole ToEntity(UserRoleDto dto)
    {
        return new UserRole(dto.UserId, dto.RoleId);
    }

    /// <summary>
    /// Converts a collection of <see cref="UserRole"/> entities to a list of <see cref="UserRoleDto"/>.
    /// </summary>
    /// <param name="userRoles">Collection of user-role entities.</param>
    /// <returns>List of user-role DTOs.</returns>
    internal static List<UserRoleDto> ToDtoList(IEnumerable<UserRole> userRoles)
    {
        return [.. userRoles.Select(ToDto)];
    }
}
