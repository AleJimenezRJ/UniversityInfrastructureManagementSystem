using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;

/// <summary>
/// Provides mapping functionality between <see cref="BuildingLog"/> entities and <see cref="BuildingLogDto"/> data
/// transfer objects.
/// </summary>
/// <remarks>This class contains methods to convert collections of <see cref="BuildingLog"/> objects into their
/// corresponding <see cref="BuildingLogDto"/> representations. The mapping ensures that all relevant properties are
/// transferred between the entity and DTO layers.</remarks>
internal static class BuildingLogDtoMappers
{
    /// <summary>
    /// Converts a list of <see cref="BuildingLog"/> objects to a list of <see cref="BuildingLogDto"/> objects.
    /// </summary>
    /// <param name="buildingLogs">The list of <see cref="BuildingLog"/> objects to convert. Cannot be null.</param>
    /// <returns>A list of <see cref="BuildingLogDto"/> objects representing the converted data. Returns an empty list if
    /// <paramref name="buildingLogs"/> is empty.</returns>
    public static List<BuildingLogDto> ToDto(List<BuildingLog> buildingLogs)
    {
        return buildingLogs.Select(bl => new BuildingLogDto(
            BuildingLogInternalId: bl.BuildingsLogInternalId,
            Name: bl.Name,
            X : bl.X,
            Y : bl.Y,
            Z : bl.Z,
            Width: bl.Width,
            Length: bl.Length,
            Height: bl.Height,
            Color: bl.Color,
            AreaName: bl.AreaName,
            ModifiedAt: bl.ModifiedAt,
            Action: bl.Action
        )).ToList();
    }
}