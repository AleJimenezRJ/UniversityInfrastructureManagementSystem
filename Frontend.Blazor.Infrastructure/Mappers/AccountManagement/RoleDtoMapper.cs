using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;


namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.AccountManagement;

/// <summary>
/// Provides mapping functionality between <see cref="RoleDto"/> data transfer objects (DTOs)
/// </summary>
internal static  class RoleDtoMapper
{
    /// <summary>
    /// Maps a <see cref="RoleDto"/> instance to a <see cref="Role"/> domain entity.
    /// </summary>
    /// <param name="dto"> The <see cref="RoleDto"/> instance containing role data.</param>
    /// <returns> </returns>
    public static Role ToEntity(this RoleDto dto)
    {
        return new Role(dto.Name)
        {
            Id = dto.Id!.Value
        };
    }

}
