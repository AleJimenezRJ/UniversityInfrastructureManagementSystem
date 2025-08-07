using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.UniversityManagement;

/// <summary>
/// Provides mapping methods to convert University-related DTOs into domain entities and vice versa.
/// </summary>
internal static class UniversityDtoMappers
{
    /// <summary>
    /// Maps a <see cref="University"/> entity to a <see cref="UniversityDto"/> used for data transfer or external operations.
    /// </summary>
    /// <param name="university">The <see cref="University"/> entity to convert.</param>
    /// <returns>A <see cref="UniversityDto"/> populated with data from the university entity.</returns>
    internal static UniversityDto ToDto(University university)
    {
        return new UniversityDto
        {
            Name = university.Name!.Name,
            Country = university.Country!.Location
        };
    }

    /// <summary>
    /// Maps a <see cref="UniversityDto"/> to a <see cref="University"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="UniversityDto"/> containing university data withot the country.</param>
    /// <returns>A <see cref="University"/> entity created from the DTO's data.</returns>
    internal static University ToEntity(UniversityDto dto)
    {
        return new University(EntityName.Create(dto.Name), EntityLocation.Create(dto.Country));
    }

    /// <summary>
    /// Maps a <see cref="UniversityDto"/> to a <see cref="University"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="UniversityDto"/> containing university data.</param>
    /// <returns>A <see cref="University"/> entity created from the DTO's data.</returns>
    internal static University ToEntityList(UniversityDto dto)
    {
        return new University(
            name: EntityName.Create(dto.Name),
            country: EntityLocation.Create(dto.Country)
        );
    }

    /// <summary>
    /// Maps a collection of <see cref="UniversityDto"/> objects to a collection of <see cref="University"/> entities.
    /// </summary>
    /// <param name="dtos">The collection of <see cref="UniversityDto"/> instances to convert.</param>
    /// <returns>An <see cref="IEnumerable{University}"/> containing all mapped university entities.</returns>
    internal static IEnumerable<University> ToEntities(IEnumerable<UniversityDto> dtos)
    {
        return dtos.Select(ToEntityList);
    }
}
