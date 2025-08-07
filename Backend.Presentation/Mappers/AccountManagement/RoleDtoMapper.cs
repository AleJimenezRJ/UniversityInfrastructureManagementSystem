using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Blazor.Presentation.Mappers.AccountManagement;


/// <summary>
/// Provides mapping methods to convert <see cref="Role"/> entities to <see cref="RoleDto"/> objects.
/// </summary>
internal static class RoleDtoMapper
{
    /// <summary>
    /// Maps a <see cref="Role"/> entity to a <see cref="RoleDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Role"/> entity to map.</param>
    /// <returns>A <see cref="RoleDto"/> representing the given entity.</returns>
    internal static RoleDto ToDto(Role entity)
    {
        return new RoleDto(entity.Id, entity.Name);
    }

    /// <summary>
    /// Maps a collection of <see cref="Role"/> entities to a list of <see cref="RoleDto"/> objects.
    /// </summary>
    /// <param name="roles">The collection of <see cref="Role"/> entities to map.</param>
    /// <returns>A list of <see cref="RoleDto"/> objects.</returns>
    internal static List<RoleDto> ToDtoList(IEnumerable<Role> roles)
    {
        return [.. roles.Select(ToDto)];
    }

    public static Role ToEntity(this RoleDto dto)
    {
        return new Role(dto.Name)
        {
            Id = dto.Id
        };
    }
}
