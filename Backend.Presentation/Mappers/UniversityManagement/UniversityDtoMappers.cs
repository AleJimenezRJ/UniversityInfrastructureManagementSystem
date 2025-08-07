using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;

/// <summary>
/// Provides extension methods to map between <see cref="University"/> domain entities and university-related DTOs.
/// </summary>
internal static class UniversityDtoMappers
{
    /// <summary>
    /// Maps a <see cref="University"/> domain entity to a <see cref="UniversityDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="University"/> entity to map from.</param>
    /// <returns>A <see cref="UniversityDto"/> representing the mapped entity.</returns>
    internal static UniversityDto ToDto(University entity)
    {
        return new UniversityDto(
            entity.Name!.Name,
            entity.Country!.Location
        );
    }

    /// <summary>
    /// Maps a <see cref="University"/> domain entity to an <see cref="AddCampusUniversityDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="University"/> entity to map from.</param>
    /// <returns>An <see cref="AddCampusUniversityDto"/> representing the mapped entity.</returns>
    internal static AddCampusUniversityDto ToSecondDto(University entity)
    {
        return new AddCampusUniversityDto(
            entity.Name!.Name
        );
    }

    /// <summary>
    /// Maps an <see cref="AddCampusUniversityDto"/> to a <see cref="University"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="AddCampusUniversityDto"/> to map from.</param>
    /// <returns>A <see cref="University"/> entity created from the DTO.</returns>
    internal static University ToEntity(AddCampusUniversityDto dto)
    {
        return new University(
            EntityName.Create(dto.Name)
        );
    }

    /// <summary>
    /// Maps a <see cref="UniversityDto"/> to a <see cref="University"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="UniversityDto"/> to map from.</param>
    /// <returns>A <see cref="University"/> entity created from the DTO.</returns>
    internal static University ToEntity(UniversityDto dto)
    {
        return new University(
            EntityName.Create(dto.Name),
            EntityLocation.Create(dto.Country)
        );
    }
}
