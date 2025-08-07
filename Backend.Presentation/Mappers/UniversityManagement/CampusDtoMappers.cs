using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;

/// <summary>
/// Provides extension methods to map between <see cref="Campus"/> domain entities and campus-related DTOs.
/// </summary>
internal static class CampusDtoMappers
{
    /// <summary>
    /// Maps a <see cref="Campus"/> domain entity to a <see cref="ListCampusDto"/>.
    /// </summary>
    /// <param name="entity">The <see cref="Campus"/> entity to map from.</param>
    /// <returns>A <see cref="ListCampusDto"/> representing the mapped entity.</returns>
    internal static ListCampusDto ToDto(Campus entity)
    {
        return new ListCampusDto(
            entity.Name!.Name,
            entity.Location!.Location,
            UniversityDtoMappers.ToDto(entity.University) // returns UniversityDto
        );
    }

    /// <summary>
    /// Maps an <see cref="AddCampusDto"/> to a <see cref="Campus"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="AddCampusDto"/> to map from.</param>
    /// <param name="university">The <see cref="University"/> domain entity associated with this campus.</param>
    /// <returns>A <see cref="Campus"/> entity created from the DTO.</returns>
    internal static Campus ToEntity(AddCampusDto dto, University university)
    {
        return new Campus(
            EntityName.Create(dto.Name),
            EntityLocation.Create(dto.Location),
            university
        );
    }

    /// <summary>
    /// Maps a <see cref="ListCampusDto"/> to a <see cref="Campus"/> domain entity.
    /// </summary>
    /// <param name="dto">The <see cref="ListCampusDto"/> to map from.</param>
    /// <param name="university">The <see cref="University"/> domain entity associated with this campus.</param>
    /// <returns>A <see cref="Campus"/> entity created from the DTO.</returns>
    internal static Campus ToEntity(ListCampusDto dto, University university)
    {
        return new Campus(
            EntityName.Create(dto.Name),
            EntityLocation.Create(dto.Location),
            university
        );
    }
}
