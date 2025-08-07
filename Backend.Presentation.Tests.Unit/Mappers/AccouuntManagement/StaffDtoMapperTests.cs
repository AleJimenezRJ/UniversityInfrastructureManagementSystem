using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="StaffDtoMapper"/> class, verifying the correct mapping between data transfer
/// objects and entity objects for staff members.
/// </summary>
public class StaffDtoMapperTests
{
    /// <summary>
    /// Represents a sample data transfer object for creating a staff member.
    /// </summary>
    private static readonly CreateStaffDto SampleCreateDto = new(
        Type: "Administrative",
        Email: "staff.user@example.com",
        FirstName: "Staff",
        LastName: "User",
        Phone: "7000-5678",
        BirthDate: new DateOnly(1985, 3, 15),
        IdentityNumber: "1-2222-3333",
        InstitutionalEmail: "s.user@institution.edu"
    );

    /// <summary>
    /// Represents a sample staff member with predefined details for testing or demonstration purposes.
    /// </summary>
    private static readonly Staff SampleStaff = new Staff(
        institutionalEmail: Email.Create("s.user@institution.edu"),
        personId: 201,
        type: "Administrative"
    )
    {
        Person = new Person(
            Email.Create("staff.user@example.com"),
            "Staff",
            "User",
            Phone.Create("7000-5678"),
            BirthDate.Create(new DateOnly(1985, 3, 15)),
            IdentityNumber.Create("1-2222-3333")
        )
    };

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToEntity"/> method correctly maps the type of a staff DTO to an entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapTypeCorrectly()
    {
        var entity = StaffDtoMapper.ToEntity(SampleCreateDto);
        entity.Type.Should().Be("Administrative");
    }

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToEntity"/> method correctly maps the institutional email.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapInstitutionalEmailCorrectly()
    {
        var entity = StaffDtoMapper.ToEntity(SampleCreateDto);
        entity.InstitutionalEmail.Value.Should().Be("s.user@institution.edu");
    }

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToEntity"/> method correctly maps the email address from a DTO to an
    /// entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonEmailCorrectly()
    {
        var entity = StaffDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.Email.Value.Should().Be("staff.user@example.com");
    }

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToEntity"/> method correctly maps the first name of a person.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonFirstNameCorrectly()
    {
        var entity = StaffDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.FirstName.Should().Be("Staff");
    }

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToEntity"/> method correctly maps the last name of a person.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonLastNameCorrectly()
    {
        var entity = StaffDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.LastName.Should().Be("User");
    }

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToEntity"/> method correctly maps the phone number from a DTO to an
    /// entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonPhoneCorrectly()
    {
        var entity = StaffDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.Phone.Value.Should().Be("7000-5678");
    }

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToEntity"/> method correctly maps the birth date of a person.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonBirthDateCorrectly()
    {
        var entity = StaffDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.BirthDate.Value.Should().Be(new DateOnly(1985, 3, 15));
    }

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToEntity"/> method correctly maps the identity number of a person.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonIdentityNumberCorrectly()
    {
        var entity = StaffDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.IdentityNumber.Value.Should().Be("1-2222-3333");
    }

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToDtoList"/> method returns a list with the correct number of DTOs.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnCorrectCount()
    {
        var dtos = StaffDtoMapper.ToDtoList(new List<Staff> { SampleStaff });
        dtos.Should().HaveCount(1);
    }

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToDtoList"/> method correctly maps the <c>PersonId</c> of the first
    /// <c>Staff</c> object.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstPersonIdCorrectly()
    {
        var dtos = StaffDtoMapper.ToDtoList(new List<Staff> { SampleStaff });
        dtos[0].PersonId.Should().Be(201);
    }

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToDtoList"/> method correctly maps the type of the first <see
    /// cref="Staff"/> object to "Administrative".
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstTypeCorrectly()
    {
        var dtos = StaffDtoMapper.ToDtoList(new List<Staff> { SampleStaff });
        dtos[0].Type.Should().Be("Administrative");
    }

    /// <summary>
    /// Tests that the <see cref="StaffDtoMapper.ToDtoList"/> method correctly maps the first institutional email.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstInstitutionalEmailCorrectly()
    {
        var dtos = StaffDtoMapper.ToDtoList(new List<Staff> { SampleStaff });
        dtos[0].InstitutionalEmail.Should().Be("s.user@institution.edu");
    }

    /// <summary>
    /// Tests that <see cref="StaffDtoMapper.ToDtoList"/> returns an empty list when the input list is empty.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnEmptyList_WhenInputIsEmpty()
    {
        var dtos = StaffDtoMapper.ToDtoList(new List<Staff>());
        dtos.Should().BeEmpty();
    }
}
