using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.Spaces;

/// <summary>
/// Provides extension methods for mapping between LearningSpaceLogDto and LearningSpaceLog entities.
/// </summary>
internal static class LearningSpaceLogDtoMapper
{
    /// <summary>
    /// Converts a LearningSpaceLogDto to a LearningSpaceLog entity.
    /// </summary>
    /// <param name="dto">The LearningSpaceLogDto to convert.</param>
    /// <returns>A LearningSpaceLog entity.</returns>
    public static LearningSpaceLog ToEntity(this LearningSpaceLogDto dto)
    {
        return new LearningSpaceLog
        {
            LearningSpaceLogInternalId = dto.LearningSpaceLogInternalId ?? 0,
            Name = dto.Name ?? string.Empty,
            Type = dto.Type ?? string.Empty,
            MaxCapacity = dto.MaxCapacity ?? 0,
            Width = (decimal)(dto.Width ?? 0),
            Height = (decimal)(dto.Height ?? 0),
            Length = (decimal)(dto.Length ?? 0),
            ColorFloor = dto.ColorFloor ?? string.Empty,
            ColorWalls = dto.ColorWalls ?? string.Empty,
            ColorCeiling = dto.ColorCeiling ?? string.Empty,
            ModifiedAt = dto.ModifiedAt?.DateTime ?? DateTime.MinValue,
            Action = dto.Action ?? string.Empty
        };
    }

    /// <summary>
    /// Converts a list of LearningSpaceLogDto to a list of LearningSpaceLog entities.
    /// </summary>
    /// <param name="dtos">The list of LearningSpaceLogDto to convert.</param>
    /// <returns>A list of LearningSpaceLog entities.</returns>
    public static List<LearningSpaceLog> ToEntity(this IEnumerable<LearningSpaceLogDto> dtos)
    {
        return dtos.Select(dto => dto.ToEntity()).ToList();
    }
}
