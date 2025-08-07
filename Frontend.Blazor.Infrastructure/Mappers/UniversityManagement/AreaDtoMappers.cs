using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.UniversityManagement;

/// <summary>
/// Provides mapping methods between Area-related DTOs and domain entities.
/// </summary>
internal static class AreaDtoMappers
{
    /// <summary>
    /// Maps an <see cref="Area"/> domain entity to an <see cref="AddAreaDto"/> used for POST requests.
    /// </summary>
    /// <param name="area">The <see cref="Area"/> entity to convert.</param>
    /// <returns>An <see cref="AddAreaDto"/> representation of the area.</returns>
    internal static AddAreaDto ToDto(Area area)
    {
        return new AddAreaDto
        {
            Name = area.Name!.Name,
            Campus = new AddAreaCampusDto
            {
                Name = area.CampusName!.Name
            }
        };
    }

    /// <summary>
    /// Maps a <see cref="ListAreaDto"/> and an existing <see cref="Campus"/> to an <see cref="Area"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="ListAreaDto"/> containing area data.</param>
    /// <param name="campus">The <see cref="Campus"/> entity associated with the area.</param>
    /// <returns>An <see cref="Area"/> domain entity created from the DTO and campus.</returns>
    internal static Area ToEntity(ListAreaDto dto, Campus campus)
    {
        return new Area(
            name: EntityName.Create(dto.Name),
            campus: campus
        );
    }

    /// <summary>
    /// Maps an <see cref="AddAreaDto"/> and an existing <see cref="Campus"/> to an <see cref="Area"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="AddAreaDto"/> containing data for area creation.</param>
    /// <param name="campus">The campus entity that the area will belong to.</param>
    /// <returns>An <see cref="Area"/> domain entity based on the DTO and campus.</returns>
    internal static Area ToEntity(AddAreaDto dto, Campus campus)
    {
        return new Area(
            name: EntityName.Create(dto.Name),
            campus: campus
        );
    }

    /// <summary>
    /// Maps a <see cref="ListAreaDto"/> to a fully constructed <see cref="Area"/> domain entity,
    /// including nested <see cref="Campus"/> and <see cref="University"/> information.
    /// </summary>
    /// <param name="dto">The <see cref="ListAreaDto"/> with area, campus, and university data.</param>
    /// <returns>A complete <see cref="Area"/> entity with nested domain hierarchy.</returns>
    internal static Area ToEntity(ListAreaDto dto)
    {
        var university = UniversityDtoMappers.ToEntity(dto.Campus!.University!);
        var campus = new Campus(
            name: EntityName.Create(dto.Campus.Name),
            location: EntityLocation.Create(dto.Campus.Location),
            university: university
        );
        return new Area(
            name: EntityName.Create(dto.Name),
            campus: campus
        );
    }

    /// <summary>
    /// Maps a collection of <see cref="ListAreaDto"/> objects to a collection of <see cref="Area"/> domain entities.
    /// </summary>
    /// <param name="dtos">The collection of <see cref="ListAreaDto"/> instances.</param>
    /// <returns>An <see cref="IEnumerable{Area}"/> containing the mapped area entities.</returns>
    internal static IEnumerable<Area> ToEntities(IEnumerable<ListAreaDto> dtos)
    {
        return dtos.Select(ToEntity);
    }
}
