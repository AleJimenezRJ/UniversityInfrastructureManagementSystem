using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="StudentDtoMapper"/> class, verifying the correct mapping between <see
/// cref="CreateStudentDto"/> and <see cref="Student"/> entities.
/// </summary>
public class StudentDtoMapperTests
{
    /// <summary>
    /// Represents a sample data transfer object for creating a student.
    /// </summary>
    private static readonly CreateStudentDto SampleCreateDto = new(
        StudentId: "S123456",
        Email: "student.user@example.com",
        FirstName: "Student",
        LastName: "User",
        Phone: "7000-1234",
        BirthDate: new DateOnly(2000, 1, 1),
        IdentityNumber: "1-1111-1111",
        InstitutionalEmail: "s.user@institution.edu"
    );

    /// <summary>
    /// Represents a sample student with predefined details for testing or demonstration purposes.
    /// </summary>
    private static readonly Student SampleStudent = new Student(
        studentId: "S123456",
        institutionalEmail: Email.Create("s.user@institution.edu"),
        personId: 101
    )
    {
        Person = new Person(
            Email.Create("student.user@example.com"),
            "Student",
            "User",
            Phone.Create("7000-1234"),
            BirthDate.Create(new DateOnly(2000, 1, 1)),
            IdentityNumber.Create("1-1111-1111")
        )
    };

    /// <summary>
    /// Tests that the <see cref="StudentDtoMapper.ToEntity"/> method correctly maps the student ID from the DTO to the
    /// entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapStudentIdCorrectly()
    {
        var entity = StudentDtoMapper.ToEntity(SampleCreateDto);
        entity.StudentId.Should().Be("S123456");
    }

    /// <summary>
    /// Tests that the <see cref="StudentDtoMapper.ToEntity"/> method correctly maps the institutional email.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapInstitutionalEmailCorrectly()
    {
        var entity = StudentDtoMapper.ToEntity(SampleCreateDto);
        entity.InstitutionalEmail.Value.Should().Be("s.user@institution.edu");
    }

    /// <summary>
    /// Tests that the <see cref="StudentDtoMapper.ToEntity"/> method correctly maps the email address from a
    /// <c>StudentCreateDto</c> to a <c>Person</c> entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonEmailCorrectly()
    {
        var entity = StudentDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.Email.Value.Should().Be("student.user@example.com");
    }

    /// <summary>
    /// Tests that the <see cref="StudentDtoMapper.ToEntity"/> method correctly maps the first name of a person.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonFirstNameCorrectly()
    {
        var entity = StudentDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.FirstName.Should().Be("Student");
    }

    /// <summary>
    /// Tests that the <see cref="StudentDtoMapper.ToEntity"/> method correctly maps the last name of a person.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonLastNameCorrectly()
    {
        var entity = StudentDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.LastName.Should().Be("User");
    }

    /// <summary>
    /// Tests that the <see cref="StudentDtoMapper.ToEntity"/> method correctly maps the phone number from a DTO to an
    /// entity.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonPhoneCorrectly()
    {
        var entity = StudentDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.Phone.Value.Should().Be("7000-1234");
    }

    /// <summary>
    /// Tests that the <see cref="StudentDtoMapper.ToEntity"/> method correctly maps the birth date of a person.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonBirthDateCorrectly()
    {
        var entity = StudentDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.BirthDate.Value.Should().Be(new DateOnly(2000, 1, 1));
    }

    /// <summary>
    /// Tests that the <see cref="StudentDtoMapper.ToEntity"/> method correctly maps the identity number of a person.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldMapPersonIdentityNumberCorrectly()
    {
        var entity = StudentDtoMapper.ToEntity(SampleCreateDto);
        entity.Person.IdentityNumber.Value.Should().Be("1-1111-1111");
    }

    /// <summary>
    /// Tests that the <see cref="StudentDtoMapper.ToDtoList"/> method returns a list with the correct number of DTOs.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnCorrectCount()
    {
        var dtos = StudentDtoMapper.ToDtoList(new List<Student> { SampleStudent });
        dtos.Should().HaveCount(1);
    }

    /// <summary>
    /// Verifies that the <see cref="StudentDtoMapper.ToDtoList"/> method correctly maps the StudentId of the first
    /// student in the list.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstStudentIdCorrectly()
    {
        var dtos = StudentDtoMapper.ToDtoList(new List<Student> { SampleStudent });
        dtos[0].StudentId.Should().Be("S123456");
    }

    /// <summary>
    /// Tests that the <see cref="StudentDtoMapper.ToDtoList"/> method correctly maps the first institutional email of a
    /// student.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstInstitutionalEmailCorrectly()
    {
        var dtos = StudentDtoMapper.ToDtoList(new List<Student> { SampleStudent });
        dtos[0].InstitutionalEmail.Should().Be("s.user@institution.edu");
    }

    /// <summary>
    /// Tests that the <see cref="StudentDtoMapper.ToDtoList"/> method returns an empty list when the input list is
    /// empty.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnEmptyList_WhenInputIsEmpty()
    {
        var dtos = StudentDtoMapper.ToDtoList(new List<Student>());
        dtos.Should().BeEmpty();
    }
}
