namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;


public record class UserAuditDto(
    int AuditId,
    string UserName,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string IdentityNumber,
    DateTime BirthDate,
    DateTime ModifiedAt,
    string Action
    );