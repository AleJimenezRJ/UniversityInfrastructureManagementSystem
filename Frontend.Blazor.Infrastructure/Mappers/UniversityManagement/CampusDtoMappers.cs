using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.UniversityManagement;

/// <summary>
/// Provides mapping methods to convert Campus-related DTOs into domain entities and vice versa.
/// </summary>
internal static class CampusDtoMappers
{
    /// <summary>
    /// Maps a <see cref="Campus"/> entity to an <see cref="AddCampusDto"/> used for creating a campus via POST requests.
    /// </summary>
    /// <param name="campus">The <see cref="Campus"/> entity to convert.</param>
    /// <returns>An <see cref="AddCampusDto"/> populated with data from the campus entity.</returns>
    internal static AddCampusDto ToDto(Campus campus)
    {
        return new AddCampusDto
        {
            Name = campus.Name!.Name,
            Location = campus.Location!.Location,
            University = new AddCampusUniversityDto
            {
                Name = campus.UniversityName!.Name
            }
        };
    }

    /// <summary>
    /// Maps a <see cref="ListCampusDto"/> to a <see cref="Campus"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="ListCampusDto"/> containing campus data.</param>
    /// <returns>A <see cref="Campus"/> entity created from the DTO, including the associated university.</returns>
    internal static Campus ToEntity(ListCampusDto dto)
    {
        return new Campus(
            name: EntityName.Create(dto.Name),
            location: EntityLocation.Create(dto.Location),
            university: UniversityDtoMappers.ToEntity(dto.University!)
        );
    }

    /// <summary>
    /// Maps a collection of <see cref="ListCampusDto"/> objects to a collection of <see cref="Campus"/> entities.
    /// </summary>
    /// <param name="dtos">The collection of <see cref="ListCampusDto"/> instances.</param>
    /// <returns>An <see cref="IEnumerable{Campus}"/> containing mapped campus entities.</returns>
    internal static IEnumerable<Campus> ToEntities(IEnumerable<ListCampusDto> dtos)
    {
        return dtos.Select(ToEntity);
    }
}
