using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.UniversityManagement;

/// <summary>
/// Provides extension methods for mapping between <see cref="BuildingLogDto"/> and <see cref="BuildingLog"/> entities.
/// </summary>
/// <remarks>This static class contains methods that facilitate the conversion of data transfer objects (DTOs) to
/// entity objects used within the application. It is intended for internal use to streamline the mapping
/// process.</remarks>
internal static class BuildingLogDtoMappers
{
    /// <summary>
    /// Converts a <see cref="BuildingLogDto"/> to a <see cref="BuildingLog"/> entity.
    /// </summary>
    /// <param name="dto">The data transfer object containing building log information. Cannot be null.</param>
    /// <returns>A <see cref="BuildingLog"/> entity populated with data from the specified <paramref name="dto"/>.</returns>
    public static BuildingLog ToEntity(this BuildingLogDto dto)
    {
        return new BuildingLog
        {
            BuildingsLogInternalId = dto.BuildingLogInternalId,
            Name = dto.Name!,
            X = dto.X,
            Y = dto.Y,
            Z = dto.Z,
            Height = dto.Height,
            Width = dto.Width,
            Length = dto.Length,
            Color = dto.Color!,
            AreaName = dto.AreaName!,
            ModifiedAt = dto.ModifiedAt,
            Action = dto.Action!
        };
    }

}
