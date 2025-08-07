using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;

/// <summary>
/// Provides extension methods to map between <see cref="Area"/> domain entities and area-related DTOs.
/// </summary>
internal static class AreaDtoMappers
{
    /// <summary>
    /// Maps an <see cref="Area"/> domain entity to a <see cref="ListAreaDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Area"/> entity to map from.</param>
    /// <returns>A <see cref="ListAreaDto"/> representing the mapped entity.</returns>
    internal static ListAreaDto ToDto(Area entity)
    {
        return new ListAreaDto(
            entity.Name!.Name,
            CampusDtoMappers.ToDto(entity.Campus)
        );
    }

    /// <summary>
    /// Maps an <see cref="Area"/> domain entity to an <see cref="AddBuildingAreaDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Area"/> entity to map from.</param>
    /// <returns>A <see cref="AddBuildingAreaDto"/> representing the mapped entity.</returns>
    internal static AddBuildingAreaDto ToSecondDto(Area entity)
    {
        return new AddBuildingAreaDto(
            entity.Name!.Name
        );
    }

    /// <summary>
    /// Maps an <see cref="Area"/> domain entity to an <see cref="AddAreaDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Area"/> entity to map from.</param>
    /// <returns>An <see cref="AddAreaDto"/> representing the mapped entity.</returns>
    internal static AddAreaDto ToAddDto(Area entity)
    {
        return new AddAreaDto(
            entity.Name!.Name,
            new AddAreaCampusDto(entity.Campus.Name!.Name)
        );
    }

    /// <summary>
    /// Maps a <see cref="ListAreaDto"/> to an <see cref="Area"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="ListAreaDto"/> to map from.</param>
    /// <param name="campus">The <see cref="Campus"/> domain entity associated with this area.</param>
    /// <returns>An <see cref="Area"/> entity created from the DTO.</returns>
    internal static Area ToEntity(ListAreaDto dto, Campus campus)
    {
        return new Area(
            name: EntityName.Create(dto.Name),
            campus: campus
        );
    }

    /// <summary>
    /// Maps an <see cref="AddAreaDto"/> to an <see cref="Area"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="AddAreaDto"/> to map from.</param>
    /// <param name="campus">The <see cref="Campus"/> domain entity associated with this area.</param>
    /// <returns>An <see cref="Area"/> entity created from the DTO.</returns>
    internal static Area ToEntity(AddAreaDto dto, Campus campus)
    {
        return new Area(
            name: EntityName.Create(dto.Name),
            campus: campus
        );
    }
}
