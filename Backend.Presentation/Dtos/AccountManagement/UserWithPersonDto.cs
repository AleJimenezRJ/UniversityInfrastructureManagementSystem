using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

/// <summary>
/// Data Transfer Object (DTO) representing a user with associated person details and their roles.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="UserName">The username of the user.</param>
/// <param name="PersonId">The unique identifier of the associated person.</param>
/// <param name="Email">The email address of the person.</param>
/// <param name="FirstName">The first name of the person.</param>
/// <param name="LastName">The last name of the person.</param>
/// <param name="Phone">The phone number of the person.</param>
/// <param name="BirthDate">The birth date of the person.</param>
/// <param name="IdentityNumber">The national identity number of the person.</param>
/// <param name="Roles">The list of role names assigned to the user.</param>
public record class UserWithPersonDto(
    int Id,
    string UserName,
    int PersonId,
    string Email,
    string FirstName,
    string LastName,
    string Phone,
    DateOnly BirthDate,
    string IdentityNumber,
    List<string> Roles
);

/// <summary>
/// Data Transfer Object (DTO) used for creating a new user with associated person details and their roles.
/// </summary>
/// <param name="UserName">The username of the user.</param>
/// <param name="Email">The email address of the person.</param>
/// <param name="FirstName">The first name of the person.</param>
/// <param name="LastName">The last name of the person.</param>
/// <param name="Phone">The phone number of the person.</param>
/// <param name="BirthDate">The birth date of the person.</param>
/// <param name="IdentityNumber">The national identity number of the person.</param>
/// <param name="Roles">The list of role names assigned to the user.</param>
public record class CreateUserWithPersonDto(
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    string Phone,
    DateOnly BirthDate,
    string IdentityNumber,
    List<string> Roles
);

/// <summary>
/// Data Transfer Object (DTO) representing the unique identifier of a user.
/// </summary>
/// <param name="UserId">The unique identifier of the user.</param>
public record class UserIdDto(
    int UserId
);

/// <summary>
/// DTO specifically used for paginated retrieval of users with associated person details.
/// </summary>
/// <param name="UserId">The unique identifier of the user.</param>
/// <param name="UserName">The username of the user.</param>
/// <param name="PersonId">The unique identifier of the associated person.</param>
/// <param name="Email">The email address of the person.</param>
/// <param name="FirstName">The first name of the person.</param>
/// <param name="LastName">The last name of the person.</param>
/// <param name="Phone">The phone number of the person.</param>
/// <param name="BirthDate">The birth date of the person.</param>
/// <param name="IdentityNumber">The national identity number of the person.</param>
/// <param name="Roles">The list of role names assigned to the user.</param>
public record class UserWithPersonPaginatedDto(
    int UserId,
    string UserName,
    int PersonId,
    string Email,
    string FirstName,
    string LastName,
    string Phone,
    DateOnly BirthDate,
    string IdentityNumber,
    List<string> Roles
);
