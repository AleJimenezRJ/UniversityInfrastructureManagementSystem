using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response containing a list of user audits.
/// </summary>
public class GetAllUserAuditResponse
{
    /// <summary>
    /// Gets or sets the list of user audits.
    /// </summary>
    public List<UserAuditDto> UserAudits { get; set; } = new List<UserAuditDto>();
}