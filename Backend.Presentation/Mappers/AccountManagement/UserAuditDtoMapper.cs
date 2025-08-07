using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

/// <summary>
/// Maps <see cref="UserAudit"/> entities to <see cref="UserAuditDto"/> objects.
/// </summary>
internal static class UserAuditDtoMapper
{
    /// <summary>
    /// Converts a <see cref="UserAudit"/> entity to a <see cref="UserAuditDto"/>.
    /// </summary>
    /// <param name="userAudits"></param>
    /// <returns>
    /// Returns a <see cref="UserAuditDto"/> object containing the mapped properties from the <see cref="UserAudit"/> entity.
    /// </returns>
    public static List<UserAuditDto> ToDtoList(List<UserAudit> userAudits)
    {
        return userAudits.Select(s => new UserAuditDto(
            AuditId: s.AuditId,
            UserName: s.UserName,
            FirstName: s.FirstName,
            LastName: s.LastName,
            Email: s.Email,
            Phone: s.Phone,
            IdentityNumber: s.IdentityNumber,
            BirthDate: s.BirthDate,
            ModifiedAt: s.ModifiedAt,
            Action: s.Action
        )).ToList();
    }

    /// <summary>
    /// Converts a <see cref="UserAudit"/> entity to a <see cref="UserAuditDto"/>.
    /// </summary>
    /// <param name="userAudit"> The <see cref="UserAudit"/> entity to convert.</param>
    /// <returns>
    /// Returns a <see cref="UserAuditDto"/> object containing the mapped properties from the <see cref="UserAudit"/> entity.
    /// </returns>
    public static UserAuditDto ToDto(UserAudit userAudit)
    {
        return new UserAuditDto(
            AuditId: userAudit.AuditId,
            UserName: userAudit.UserName,
            FirstName: userAudit.FirstName,
            LastName: userAudit.LastName,
            Email: userAudit.Email,
            Phone: userAudit.Phone,
            IdentityNumber: userAudit.IdentityNumber,
            BirthDate: userAudit.BirthDate,
            ModifiedAt: userAudit.ModifiedAt,
            Action: userAudit.Action
            );
    }
}