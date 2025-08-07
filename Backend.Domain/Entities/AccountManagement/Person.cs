using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

/// <summary>
/// Represents a person in the system.
/// </summary>
public class Person
{
    public int Id { get; private set; } = 0;
    public Email Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IdentityNumber IdentityNumber { get; set; }
    public Phone Phone { get; set; }
    public BirthDate BirthDate { get; set; }

    /// <summary>
    /// Constructor for the Person entity
    /// </summary>
    /// <param name="id"></param>
    /// <param name="email"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="phone"></param>
    /// <param name="birthDate"></param>
    /// <param name="identityNumber"></param>
    public Person(Email email, string firstName, string lastName, Phone phone, BirthDate birthDate, IdentityNumber identityNumber, int id = 0)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        BirthDate = birthDate;
        IdentityNumber = identityNumber;
    }

    /// <summary>
    /// Private parameterless constructor to the entity
    /// </summary>
    protected Person() 
    {
        Id = default;
        Email = null!;
        FirstName = null!;
        LastName = null!;
        Phone = null!;
        BirthDate = null!;
        IdentityNumber = null!;
    }
}