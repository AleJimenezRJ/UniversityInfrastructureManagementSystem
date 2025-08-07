using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.Spaces;

/// <summary>
/// Provides mapping methods to convert between <see cref="LearningSpace"/> domain entities and <see cref="LearningSpaceDto"/> data transfer objects.
/// </summary>
public static class LearningSpaceDtoMapper
{
    /// <summary>
    /// Converts a <see cref="LearningSpaceDto"/> object into a <see cref="LearningSpace"/> domain entity.
    /// </summary>
    /// <param name="dto">The data transfer object containing learning space data.</param>
    /// <returns>A <see cref="LearningSpace"/> entity with values populated from the DTO.</returns>
    public static LearningSpace ToEntity(this LearningSpaceDto dto)
    {
        return new LearningSpace(
            EntityName.Create(dto.Name),
            LearningSpaceType.Create(dto.Type),
            Capacity.Create(dto.MaxCapacity),
            Size.Create(dto.Height ?? 0),
            Size.Create(dto.Width ?? 0),
            Size.Create(dto.Length ?? 0),
            Colors.Create(dto.ColorFloor ?? ""),
            Colors.Create(dto.ColorWalls ?? ""),
            Colors.Create(dto.ColorCeiling ?? "")
        );
    }

    /// <summary>
    /// Converts a <see cref="LearningSpace"/> domain entity into a <see cref="LearningSpaceDto"/>.
    /// </summary>
    /// <param name="learningSpaceEntity">The <see cref="LearningSpace"/> domain object to convert</param>
    /// <returns>A <see cref="LearningSpaceDto"/> object with values mapped from the domain entity.</returns>
    public static LearningSpaceDto ToDto(this LearningSpace learningSpaceEntity)
    {
        return new LearningSpaceDto
        {
            Name = learningSpaceEntity.Name?.Name,
            Type = learningSpaceEntity.Type?.Value,
            MaxCapacity = learningSpaceEntity.MaxCapacity?.Value,
            Height = learningSpaceEntity.Height?.Value,
            Width = learningSpaceEntity.Width?.Value,
            Length = learningSpaceEntity.Length?.Value,
            ColorFloor = learningSpaceEntity.ColorFloor?.Value,
            ColorWalls = learningSpaceEntity.ColorWalls?.Value,
            ColorCeiling = learningSpaceEntity.ColorCeiling?.Value
        };
    }
}
