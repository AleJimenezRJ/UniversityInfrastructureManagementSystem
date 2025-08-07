using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

/// <summary>
/// Provides mapping methods for converting <see cref="Permission"/> entities to <see cref="PermissionDto"/> objects.
/// </summary>
internal static class PermissionDtoMapper
{
    /// <summary>
    /// Maps a <see cref="Permission"/> entity to a <see cref="PermissionDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Permission"/> entity to map.</param>
    /// <returns>A <see cref="PermissionDto"/> representing the mapped entity.</returns>
    internal static PermissionDto ToDto(Permission entity)
    {
        return new PermissionDto(entity.Id, entity.Description);
    }

    /// <summary>
    /// Maps a collection of <see cref="Permission"/> entities to a list of <see cref="PermissionDto"/> objects.
    /// </summary>
    /// <param name="permissions">The collection of <see cref="Permission"/> entities to map.</param>
    /// <returns>A list of <see cref="PermissionDto"/> objects.</returns>
    internal static List<PermissionDto> ToDtoList(IEnumerable<Permission> permissions)
    {
        return [.. permissions.Select(ToDto)];
    }
}
