using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.AccountManagement;

/// <summary>
/// Provides mapping functionality between <see cref="UserWithPersonDto"/> data transfer objects (DTOs)
/// and the <see cref="UserWithPerson"/> domain entity. This class is responsible for converting
/// data received from external sources (such as APIs) into domain entities used within the application,
/// and vice versa.
/// </summary>
internal static class UserWithPersonDtoMapper
{
    /// <summary>
    /// Maps a <see cref="UserWithPersonDto"/> instance to a <see cref="UserWithPerson"/> domain entity.
    /// This method is typically used to convert data received from the backend or API into a domain entity
    /// that can be used within the Blazor application.
    /// </summary>
    /// <param name="dto">The <see cref="UserWithPersonDto"/> instance containing user and person data.</param>
    /// <returns>
    /// A <see cref="UserWithPerson"/> domain entity populated with the data from the DTO.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if required properties in the DTO are null.
    /// </exception>
    internal static UserWithPerson ToEntity(this UserWithPersonDto dto)
    {
        // Map each property from the DTO to the corresponding value object or primitive in the domain entity.
        // The value objects (UserName, Email, Phone, IdentityNumber, BirthDate) are created using their respective
        // static Create methods, which may throw validation exceptions if the input is invalid.
        // The Id and PersonId are expected to be non-nullable in the DTO for this mapping.
        return new UserWithPerson
        (
            UserName.Create(dto.UserName),
            dto.FirstName!,
            dto.LastName!,
            Email.Create(dto.Email),
            Phone.Create(dto.Phone),
            IdentityNumber.Create(dto.IdentityNumber),
            BirthDate.Create(dto.BirthDate),
            dto.Id!.Value,
            dto.PersonId!.Value
        )
        {
            Roles = dto.Roles?.ToList() ?? new List<string>()
        };
    }

    /// <summary>
    /// Maps a <see cref="CreateUserWithPersonDto"/> instance to a <see cref="UserWithPerson"/> domain entity.
    /// This method is typically used when creating a new user and person record from client input.
    /// The resulting domain entity can then be used for further processing or persistence.
    /// </summary>
    /// <param name="dto">The <see cref="CreateUserWithPersonDto"/> containing the data for the new user and person.</param>
    /// <returns>
    /// A <see cref="UserWithPerson"/> domain entity initialized with the provided data.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if required properties in the DTO are null.
    /// </exception>
    internal static UserWithPerson CreateUserWithPersonEntity(CreateUserWithPersonDto dto)
    {
        // Map each property from the creation DTO to the corresponding value object or primitive in the domain entity.
        // The roles list is optional; if not provided, an empty list is used.
        return new UserWithPerson(
            UserName.Create(dto.UserName),
            dto.FirstName,
            dto.LastName,
            Email.Create(dto.Email),
            Phone.Create(dto.Phone),
            IdentityNumber.Create(dto.IdentityNumber),
            BirthDate.Create(dto.BirthDate),
            roles: dto.Roles ?? new List<string>()
        );
    }

    /// <summary>
    /// Maps a <see cref="UserWithPersonPaginatedDto"/> instance to a <see cref="UserWithPerson"/> domain entity.
    /// This method is typically used to convert paginated user and person data received from the backend or API
    /// into a domain entity that can be used within the Blazor application.
    /// </summary>
    /// <param name="dto">The <see cref="UserWithPersonPaginatedDto"/> instance containing user and person data.</param>
    /// <returns>
    /// A <see cref="UserWithPerson"/> domain entity populated with the data from the paginated DTO.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if required properties in the DTO are null.
    /// </exception>
    internal static UserWithPerson ToEntity(this UserWithPersonPaginatedDto dto)
    {
        return new UserWithPerson
        (
            UserName.Create(dto.UserName),
            dto.FirstName!,
            dto.LastName!,
            Email.Create(dto.Email),
            Phone.Create(dto.Phone),
            IdentityNumber.Create(dto.IdentityNumber),
            BirthDate.Create(dto.BirthDate),
            dto.UserId!.Value,
            dto.PersonId!.Value
        )
        {
            Roles = dto.Roles?.ToList() ?? new List<string>()
        };
    }
}
