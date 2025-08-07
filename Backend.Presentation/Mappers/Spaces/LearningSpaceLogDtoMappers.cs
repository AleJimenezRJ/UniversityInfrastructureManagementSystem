using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;

/// <summary>
/// Provides mapping functionality between <see cref="LearningSpaceLog"/> entities and
/// <see cref="LearningSpaceLogDto"/> data transfer objects.
/// </summary>
/// <remarks>
/// This class contains methods to convert collections of <see cref="LearningSpaceLog"/> objects into their
/// corresponding <see cref="LearningSpaceLogDto"/> representations. The mapping ensures that all relevant
/// properties are transferred between the entity and DTO layers.
/// </remarks>
internal static class LearningSpaceLogDtoMappers
{
    /// <summary>
    /// Converts a list of <see cref="LearningSpaceLog"/> objects to a list of <see cref="LearningSpaceLogDto"/> objects.
    /// </summary>
    /// <param name="logs">The list of <see cref="LearningSpaceLog"/> objects to convert. Cannot be null.</param>
    /// <returns>A list of <see cref="LearningSpaceLogDto"/> objects representing the converted data.
    /// Returns an empty list if <paramref name="logs"/> is empty.</returns>
    public static List<LearningSpaceLogDto> ToDto(List<LearningSpaceLog> logs)
    {
        return logs.Select(ls => new LearningSpaceLogDto(
            LearningSpaceLogInternalId: ls.LearningSpaceLogInternalId,
            Name: ls.Name,
            MaxCapacity: ls.MaxCapacity,
            Type: ls.Type,
            Width: ls.Width,
            Height: ls.Height,
            Length: ls.Length,
            ColorFloor: ls.ColorFloor,
            ColorWalls: ls.ColorWalls,
            ColorCeiling: ls.ColorCeiling,
            ModifiedAt: ls.ModifiedAt,
            Action: ls.Action
        )).ToList();
    }
}
