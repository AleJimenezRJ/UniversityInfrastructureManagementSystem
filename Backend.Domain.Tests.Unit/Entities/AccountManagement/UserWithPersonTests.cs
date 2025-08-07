using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.AccountManagement;

/// <summary>
/// Unit tests for the <see cref="UserWithPerson"/> class.
/// </summary>
/// The PBI that this test class is related to is: #214
/// Manage UserWithPerson Entity
/// Technical tasks to complete:
/// – Implement backend logic to manage combined user and person data
/// – Add validations for null and existing user entries
/// – Write unit and integration tests
/// Participants: Andres Murillo & Tatiana Paramo
public class UserWithPersonTests
{
    /// <summary>
    /// Test data for UserWithPerson constructor tests.
    /// </summary>
    private readonly UserName _userName = UserName.Create("jdoe");

    /// <summary>
    /// Test data for the first name used in UserWithPerson constructor tests.
    /// </summary>
    private readonly string _firstName = "Joe";

    /// <summary>
    /// Test data for the last name used in UserWithPerson constructor tests.
    /// </summary>
    private readonly string _lastName = "Doe";

    /// <summary>
    /// Test data for the email used in UserWithPerson constructor tests.
    /// </summary>
    private readonly Email _email = Email.Create("joeFonseca@ucr.ac.cr");

    /// <summary>
    /// Test data for the phone used in UserWithPerson constructor tests.
    /// </summary>
    private readonly Phone _phone = Phone.Create("1234-5678");

    /// <summary>
    /// Test data for the identity number used in UserWithPerson constructor tests.
    /// </summary>
    private readonly IdentityNumber _identityNumber = IdentityNumber.Create("1-2345-6789");

    /// <summary>
    /// Test data for the birth date used in UserWithPerson constructor tests.
    /// </summary>
    private readonly BirthDate _birthDate = BirthDate.Create(new DateOnly(2002, 1, 1));

    /// <summary>
    /// Test data for the roles used in UserWithPerson constructor tests.
    /// </summary>
    private readonly List<string> _roles = new() { "Admin", "Editor" };

    /// <summary>
    /// Test data for the user ID used in UserWithPerson constructor tests.
    /// </summary>
    private readonly int _userId = 1;

    /// <summary>
    /// Test data for the person ID used in UserWithPerson constructor tests.
    /// </summary>
    private readonly int _personId = 10;

    /// <summary>
    /// Tests that the constructor correctly assigns all properties.
    /// </summary>
    [Fact]
    public void Constructor_AssignsUserNameCorrectly()
    {
        // Arrange
        var userWithPerson = new UserWithPerson(_userName, _firstName, _lastName, _email, _phone, _identityNumber, _birthDate, _userId, _personId);
        //  Assert
        userWithPerson.UserName.Should().Be(_userName, because: "constructor should assign username");
    }

    /// <summary>
    /// Tests that the constructor correctly assigns the first name.
    /// </summary>
    [Fact]
    public void Constructor_AssignsFirstNameCorrectly()
    {
        // Arrange
        var userWithPerson = new UserWithPerson(_userName, _firstName, _lastName, _email, _phone, _identityNumber, _birthDate, _userId, _personId);
        //  Assert
        userWithPerson.FirstName.Should().Be(_firstName, because: "constructor should assign firstName");
    }

    /// <summary>
    /// Tests that the constructor correctly assigns the last name.
    /// </summary>
    [Fact]
    public void Constructor_AssignsLastNameCorrectly()
    {
        // Arrange
        var userWithPerson = new UserWithPerson(_userName, _firstName, _lastName, _email, _phone, _identityNumber, _birthDate, _userId, _personId);
        //  Assert
        userWithPerson.LastName.Should().Be(_lastName, because: "constructor should assign lastName");
    }

    /// <summary>
    /// Tests that the constructor correctly assigns the email.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEmailCorrectly()
    {
        // Arrange
        var userWithPerson = new UserWithPerson(_userName, _firstName, _lastName, _email, _phone, _identityNumber, _birthDate, _userId, _personId);
        //  Assert
        userWithPerson.Email.Should().Be(_email, because: "constructor should assign email");
    }

    /// <summary>
    /// Tests that the constructor correctly assigns the phone.
    /// </summary>
    [Fact]
    public void Constructor_AssignsPhoneCorrectly()
    {
        // Arrange
        var userWithPerson = new UserWithPerson(_userName, _firstName, _lastName, _email, _phone, _identityNumber, _birthDate, _userId, _personId);
        //  Assert
        userWithPerson.Phone.Should().Be(_phone, because: "constructor should assign phone");
    }

    /// <summary>
    /// Tests that the constructor correctly assigns the identity number.
    /// </summary>
    [Fact]
    public void Constructor_AssignsIdentityNumberCorrectly()
    {
        // Arrange
        var userWithPerson = new UserWithPerson(
            _userName, _firstName, _lastName, _email, _phone,
            _identityNumber, _birthDate, _roles, _userId, _personId);

        // Assert
        userWithPerson.IdentityNumber.Should().Be(_identityNumber, because: "constructor should assign identity number");
    }

    /// <summary>
    /// Tests that the constructor correctly assigns the birth date.
    /// </summary>
    [Fact]
    public void Constructor_AssignsBirthDateCorrectly()
    {
        // Arrange
        var userWithPerson = new UserWithPerson(
            _userName, _firstName, _lastName, _email, _phone,
            _identityNumber, _birthDate, _roles, _userId, _personId);

        // Assert
        userWithPerson.BirthDate.Should().Be(_birthDate, because: "constructor should assign birth date");
    }

    /// <summary>
    /// Tests that the constructor correctly assigns the user ID.
    /// </summary>
    [Fact]
    public void Constructor_AssignsUserIdCorrectly()
    {
        // Arrange
        var userWithPerson = new UserWithPerson(
            _userName, _firstName, _lastName, _email, _phone,
            _identityNumber, _birthDate, _roles, _userId, _personId);

        // Assert
        userWithPerson.UserId.Should().Be(_userId, because: "constructor should assign user ID");
    }

    /// <summary>
    /// Tests that the constructor correctly assigns the person ID.
    /// </summary>
    [Fact]
    public void Constructor_AssignsPersonIdCorrectly()
    {
        // Arrange
        var userWithPerson = new UserWithPerson(
            _userName, _firstName, _lastName, _email, _phone,
            _identityNumber, _birthDate, _roles, _userId, _personId);

        // Assert
        userWithPerson.PersonId.Should().Be(_personId, because: "constructor should assign person ID");
    }

    /// <summary>
    /// Tests that the constructor correctly assigns the roles.
    /// </summary>
    [Fact]
    public void Constructor_AssignsRolesCorrectly()
    {
        // Arrange
        var userWithPerson = new UserWithPerson(
            _userName, _firstName, _lastName, _email, _phone,
            _identityNumber, _birthDate, _roles, _userId, _personId);

        // Assert
        userWithPerson.Roles.Should().BeEquivalentTo(_roles, because: "constructor should assign roles");
    }

    /// <summary>
    /// Tests that the default constructor assigns an empty user ID.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEmptyUserId()
    {
        // Arrange
        // reference: https://learn.microsoft.com/en-us/dotnet/api/system.activator.createinstance?view=net-8.0#system-activator-createinstance(system-type-system-boolean
        var user = (UserWithPerson)Activator.CreateInstance(
            typeof(UserWithPerson),
            nonPublic: true)!;
        // Assert
        user.UserId.Should().Be(0, because: "default constructor should assign default value 0");
    }

    /// <summary>
    /// Tests that the default constructor assigns an empty person ID.  
    /// </summary>
    [Fact]
    public void Constructor_AssignsEmptyPersonId()
    {
        // Arrange
        var user = (UserWithPerson)Activator.CreateInstance(
            typeof(UserWithPerson),
            nonPublic: true)!;
        // Assert
        user.PersonId.Should().Be(0, because: "default constructor should assign default value 0");
    }

    /// <summary>
    /// Tests that the default constructor assigns an empty roles list.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEmptyRoles()
    {
        // Arrange
        var user = (UserWithPerson)Activator.CreateInstance(
            typeof(UserWithPerson),
            nonPublic: true)!;
        // Assert
        user.Roles.Should().BeEmpty(because: "default constructor should initialize roles list");
    }

    /// <summary>
    /// Tests that the default constructor assigns an empty user name.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEmptyUserName()
    {
        // Arrange
        var user = (UserWithPerson)Activator.CreateInstance(
            typeof(UserWithPerson),
            nonPublic: true)!;
        // Assert
        user.UserName.Should().BeNull(because: "reference type properties should be null by default");
    }

    /// <summary>
    /// Tests that the default constructor assigns an empty first name.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEmptyFirstName()
    {
        // Arrange
        var user = (UserWithPerson)Activator.CreateInstance(
            typeof(UserWithPerson),
            nonPublic: true)!;
        // Assert
        user.FirstName.Should().BeNull();
    }

    /// <summary>
    /// Tests that the default constructor assigns an empty last name.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEmptyLastName()
    {
        // Arrange
        var user = (UserWithPerson)Activator.CreateInstance(
            typeof(UserWithPerson),
            nonPublic: true)!;
        // Assert
        user.LastName.Should().BeNull(because: "reference type properties should be null by default");
    }

    /// <summary>
    /// Tests that the default constructor assigns an empty email.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEmptyEmail()
    {
        // Arrange
        var user = (UserWithPerson)Activator.CreateInstance(
            typeof(UserWithPerson),
            nonPublic: true)!;
        // Assert
        user.Email.Should().BeNull(because: "reference type properties should be null by default");
    }

    /// <summary>
    /// Tests that the default constructor assigns an empty phone.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEmptyPhone()
    {
        // Arrange
        var user = (UserWithPerson)Activator.CreateInstance(
            typeof(UserWithPerson),
            nonPublic: true)!;
        // Assert
        user.Phone.Should().BeNull(because: "reference type properties should be null by default");
    }

    /// <summary>
    /// Tests that the default constructor assigns an empty identity number.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEmptyIdentityNumber()
    {
        // Arrange
        var user = (UserWithPerson)Activator.CreateInstance(
            typeof(UserWithPerson),
            nonPublic: true)!;
        // Assert
        user.IdentityNumber.Should().BeNull(because: "reference type properties should be null by default");
    }


    /// <summary>
    /// Tests that the default constructor assigns an empty birth date.
    /// </summary>
    [Fact]
    public void Constructor_AssignsEmptyBirthDate()
    {
        // Arrange
        var user = (UserWithPerson)Activator.CreateInstance(
            typeof(UserWithPerson),
            nonPublic: true)!;
        // Assert
        user.BirthDate.Should().BeNull(because: "reference type properties should be null by default");
    }

}