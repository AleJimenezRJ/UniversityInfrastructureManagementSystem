using System.ComponentModel.DataAnnotations.Schema;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

/// <summary>
/// Represents a combination of a user account and the associated personal information.
/// This class is used to transfer joined data between User and Person entities.
/// </summary>
[NotMapped]
public class UserWithPerson
{
    // User-related properties

    /// <summary>
    /// Gets or sets the unique identifier for the user account.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the username for the user account.
    /// </summary>
    public UserName UserName { get; set; }


    // Person-related properties

    /// <summary>
    /// Gets or sets the unique identifier for the associated person.
    /// </summary>
    public int PersonId { get; set; }

    /// <summary>
    /// Gets or sets the first name of the person.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the person.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the email address of the person.
    /// </summary>
    public Email Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the person.
    /// </summary>
    public Phone Phone { get; set; }

    /// <summary>
    /// Gets or sets the national identity number of the person.
    /// </summary>
    public IdentityNumber IdentityNumber { get; set; }

    /// <summary>
    /// Gets or sets the birth date of the person.
    /// </summary>
    public BirthDate BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the list of role names assigned to the user.
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="UserWithPerson"/> class with the specified values.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="firstName">The first name of the person.</param>
    /// <param name="lastName">The last name of the person.</param>
    /// <param name="email">The email address of the person.</param>
    /// <param name="phone">The phone number of the person.</param>
    /// <param name="identityNumber">The identity number of the person.</param>
    /// <param name="birthDate">The birth date of the person.</param>
    /// <param name="userId">The unique identifier for the user (optional, default is 0).</param>
    /// <param name="personId">The unique identifier for the person (optional, default is 0).</param>
    public UserWithPerson(
       UserName userName,
       string firstName,
       string lastName,
       Email email,
       Phone phone,
       IdentityNumber identityNumber,
       BirthDate birthDate,
       int userId,
       int personId)
    {
        UserId = userId;
        UserName = userName;
        PersonId = personId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        IdentityNumber = identityNumber;
        BirthDate = birthDate;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserWithPerson"/> class with the specified values.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <param name="firstName">The first name of the person.</param>
    /// <param name="lastName">The last name of the person.</param>
    /// <param name="email">The email address of the person.</param>
    /// <param name="phone">The phone number of the person.</param>
    /// <param name="identityNumber">The identity number of the person.</param>
    /// <param name="birthDate">The birth date of the person.</param>
    /// <param name="userId">The unique identifier for the user (optional, default is 0).</param>
    /// <param name="personId">The unique identifier for the person (optional, default is 0).</param>
    public UserWithPerson(
       UserName userName,
       string firstName,
       string lastName,
       Email email,
       Phone phone,
       IdentityNumber identityNumber,
       BirthDate birthDate,
       List<string> roles,
       int userId = 0,
       int personId= 0)
    {
        UserId = userId;
        UserName = userName;
        PersonId = personId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        IdentityNumber = identityNumber;
        BirthDate = birthDate;
        Roles = roles;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserWithPerson"/> class.
    /// Protected parameterless constructor for ORM and serialization purposes.
    /// </summary>
    protected UserWithPerson()
    {
        UserId = default;
        UserName = null!;
        PersonId = default;
        FirstName = null!;
        LastName = null!;
        Email = null!;
        Phone = null!;
        IdentityNumber = null!;
        BirthDate = null!;
        Roles = new();
    }

}
