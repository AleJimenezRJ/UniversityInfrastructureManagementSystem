using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

/// <summary>
/// Provides mapping logic between <see cref="Person"/> domain entities and DTO representations.
/// </summary>
internal static class PersonDtoMapper
{
    /// <summary>
    /// Converts a <see cref="Person"/> entity to a <see cref="PersonDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Person"/> domain entity to convert.</param>
    /// <returns>A <see cref="PersonDto"/> containing the mapped data from the entity.</returns>
    internal static PersonDto ToDto(Person entity)
    {
        // Check that no attribute can be null
        if (entity.Email == null || entity.Phone == null || entity.IdentityNumber == null || entity.BirthDate == null)
        {
            throw new ValidationException("The person entity cannot have null attributes.");
        }

        return new PersonDto(
          entity.Id,
          entity.Email.Value,
          entity.FirstName,
          entity.LastName,
          entity.Phone.Value,
          entity.BirthDate.Value,
          entity.IdentityNumber.Value
        );
    }

    /// <summary>
    /// Converts a <see cref="PersonDto"/> to a <see cref="Person"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="PersonDto"/> to convert.</param>
    /// <returns>A <see cref="Person"/> entity constructed from the DTO values.</returns>
    internal static Person ToEntity(PersonDto dto)
    {
        return new Person(
          Email.Create(dto.Email),
          dto.FirstName,
          dto.LastName,
          Phone.Create(dto.Phone),
          BirthDate.Create(dto.BirthDate),
          IdentityNumber.Create(dto.IdentityNumber),
          dto.Id
        );
    }

    /// <summary>
    /// Converts a <see cref="CreatePersonDto"/> to a <see cref="Person"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="CreatePersonDto"/> to convert.</param>
    /// <returns>A <see cref="Person"/> entity constructed from the DTO values.</returns>
    internal static Person ToEntity(CreatePersonDto dto)
    {
        return new Person(
          Email.Create(dto.Email),
          dto.FirstName,
          dto.LastName,
          Phone.Create(dto.Phone),
          BirthDate.Create(dto.BirthDate),
          IdentityNumber.Create(dto.IdentityNumber)
        );
    }

    /// <summary>
    /// Converts a collection of <see cref="Person"/> entities to a list of <see cref="PersonDto"/>.
    /// </summary>
    /// <param name="people">The collection of <see cref="Person"/> entities to convert.</param>
    /// <returns>A list of <see cref="PersonDto"/> objects mapped from the input entities.</returns>
    internal static List<PersonDto> ToDtoList(IEnumerable<Person> people)
    {
        // Creates an empty list of PersonDto
        return [.. people.Select(ToDto)];
    }
}
