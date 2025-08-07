// Members: Isabella Rodríguez Sánchez y Gael Alpízar Alfaro
// User Story: SPT-SR-007-002 Add Institutional Email on User Creation #100
// Technical task: Write unit tests for Staff entity
// Justification: These tests are necessary to ensure 85% coverage on unit testing.

using FluentAssertions;
using Xunit;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="Staff"/> entity.
/// Ensures correct property assignment and construction logic for staff members,
/// including validation of institutional email, person ID, and staff type.
/// </summary>
public class StaffTests
{
    private readonly Email _validEmail;
    private readonly Staff _staff;
    private readonly Person _person;

    /// <summary>
    /// Initializes test data for <see cref="StaffTests"/>.
    /// Sets up a valid <see cref="Email"/>, <see cref="Person"/>, and <see cref="Staff"/> instance.
    /// </summary>
    public StaffTests()
    {
        _validEmail = Email.Create("gael.alpizar@ucr.ac.cr");

        _person = new Person(
            email: Email.Create("person@ucr.ac.cr"),
            firstName: "Gael",
            lastName: "Alpizar",
            phone: Phone.Create("8889-3999"),
            birthDate: BirthDate.Create(new DateOnly(2004, 9, 6)),
            identityNumber: IdentityNumber.Create("1-2345-6789"),
            id: 1
        );

        _staff = new Staff(_validEmail, 1, "Professor")
        {
            Person = _person
        };
    }

    /// <summary>
    /// Verifies that the <see cref="Staff"/> constructor correctly sets all properties when provided with valid data.
    /// </summary>
    [Fact]
    public void Constructor_WithValidData_ShouldSetProperties()
    {
        // Arrange

        // Act

        // Assert
        _staff.InstitutionalEmail.Should().Be(_validEmail);
        _staff.PersonId.Should().Be(1);
        _staff.Type.Should().Be("Professor");
        _staff.Person.Should().Be(_person);
    }

    /// <summary>
    /// Ensures that the <see cref="Staff"/> constructor allows creation with a negative person ID.
    /// </summary>
    [Fact]
    public void Constructor_WithNegativePersonId_ShouldAllowCreation()
    {
        // Arrange
        var negativePersonId = -5;

        // Act
        var staff = new Staff(_validEmail, negativePersonId, "Admin");

        // Assert
        staff.PersonId.Should().Be(negativePersonId);
    }

    /// <summary>
    /// Tests that assigning a new <see cref="Person"/> to the <see cref="Staff.Person"/> property
    /// correctly updates the reference.
    /// </summary>
    [Fact]
    public void Property_SetPerson_ShouldAssignCorrectly()
    {
        // Arrange
        var anotherPerson = new Person(
            Email.Create("other@ucr.ac.cr"),
            "Ana",
            "Mora",
            Phone.Create("8777-6666"),
            BirthDate.Create(new DateOnly(1995, 3, 10)),
            IdentityNumber.Create("9-8765-4321"),
            id: 2
        );

        // Act
        _staff.Person = anotherPerson;

        // Assert
        _staff.Person.Should().Be(anotherPerson);
    }

    /// <summary>
    /// Tests that the protected parameterless constructor of <see cref="Staff"/> creates
    /// an instance with default property values.
    /// </summary>
    [Fact]
    public void ProtectedConstructor_ShouldCreateInstanceWithDefaults()
    {
        // Arrange

        // Act
        var emptyStaff = (Staff)Activator.CreateInstance(typeof(Staff), nonPublic: true)!;

        // Assert
        emptyStaff.Should().NotBeNull();
        emptyStaff.PersonId.Should().Be(0);
        emptyStaff.Type.Should().BeNull();
        emptyStaff.InstitutionalEmail.Should().BeNull();
    }
}
