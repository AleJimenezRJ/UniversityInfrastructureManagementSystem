using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

/// <summary>
/// Provides mapping logic between <see cref="Student"/> domain entities and DTO representations.
/// </summary>
internal static class StudentDtoMapper
{

    /// <summary>
    /// Converts a <see cref="CreateStudentDto"/> to a <see cref="Student"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="CreateStudentDto"/> to convert.</param>
    /// <returns>A <see cref="Student"/> entity constructed from the DTO values.</returns>
    internal static Student ToEntity(CreateStudentDto dto)
    {
        var person = new Person(
            Email.Create(dto.Email),
            dto.FirstName,
            dto.LastName,
            Phone.Create(dto.Phone),
            BirthDate.Create(dto.BirthDate),
            IdentityNumber.Create(dto.IdentityNumber)
        );

        var student = new Student(
            dto.StudentId,
            Email.Create(dto.InstitutionalEmail),
            personId: 0
        )
        {
            Person = person
        };

        return student;
    }


    /// <summary>
    /// Maps a list of <see cref="Student"/> domain entities to a list of <see cref="StudentDto"/>.
    /// </summary>
    /// <param name="students">The domain students.</param>
    /// <returns>A list of student DTOs.</returns>
    public static List<StudentDto> ToDtoList(List<Student> students)
    {
        return students.Select(s => new StudentDto(
            PersonId: s.PersonId,
            StudentId: s.StudentId,
            Email: s.Person.Email.Value,
            FirstName: s.Person.FirstName,
            LastName: s.Person.LastName,
            Phone: s.Person.Phone.Value,
            BirthDate: s.Person.BirthDate.Value,
            IdentityNumber: s.Person.IdentityNumber.Value,
            InstitutionalEmail: s.InstitutionalEmail.Value
        )).ToList();
    }
}