using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

/// <summary>
/// Provides mapping logic between <see cref="Staff"/> domain entities and DTO representations.
/// </summary>
internal static class StaffDtoMapper
{

    /// <summary>
    /// Converts a <see cref="CreateStaffDto"/> to a <see cref="Staff"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="CreateStaffDto"/> to convert.</param>
    /// <returns>A <see cref="Staff"/> entity constructed from the DTO values.</returns>
    internal static Staff ToEntity(CreateStaffDto dto)
    {
        var person = new Person(
            Email.Create(dto.Email),
            dto.FirstName,
            dto.LastName,
            Phone.Create(dto.Phone),
            BirthDate.Create(dto.BirthDate),
            IdentityNumber.Create(dto.IdentityNumber)
        );
        //Staff(string staffId, Email institutionalEmail, int personId, string type)
        var staff = new Staff(
            Email.Create(dto.InstitutionalEmail),
            personId: 0,
            dto.Type
        )
        {
            Person = person
        };

        return staff;
    }


    /// <summary>
    /// Maps a list of <see cref="Staff"/> domain entities to a list of <see cref="StaffDto"/>.
    /// </summary>
    /// <param name="staffs">The domain staffs.</param>
    /// <returns>A list of staff DTOs.</returns>
    public static List<StaffDto> ToDtoList(List<Staff> staffs)
    {
        return staffs.Select(s => new StaffDto(
            Type: s.Type,
            PersonId: s.PersonId,
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