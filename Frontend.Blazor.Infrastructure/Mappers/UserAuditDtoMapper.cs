using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers;

/// <summary>
/// Maps UserAuditDto to UserAudit entity.
/// </summary>
internal static class UserAuditDtoMapper
{
    /// <summary>
    /// Converts a UserAuditDto to a UserAudit entity.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>
    /// Returns a UserAudit entity populated with data from the UserAuditDto.
    /// </returns>
    public static UserAudit ToEntity(this UserAuditDto dto)
    {
        return new UserAudit
        {
            AuditId = dto.AuditId ?? 0,
            UserName = dto.UserName ?? string.Empty,
            FirstName = dto.FirstName ?? string.Empty,
            LastName = dto.LastName ?? string.Empty,
            Email = dto.Email ?? string.Empty,
            Phone = dto.Phone ?? string.Empty,
            IdentityNumber = dto.IdentityNumber ?? string.Empty,
            BirthDate = dto.BirthDate?.DateTime ?? DateTime.MinValue,
            ModifiedAt = dto.ModifiedAt?.DateTime ?? DateTime.MinValue,
            Action = dto.Action ?? string.Empty
        };
    }
}
