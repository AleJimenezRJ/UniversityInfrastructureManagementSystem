using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;


/// <summary>
/// Provides mapping methods for converting <see cref="LearningComponentAudit"/> entities
/// to <see cref="LearningComponentAuditDto"/> data transfer objects.
/// </summary>
internal static class LearningComponentAuditDtoMapper
{
    /// <summary>
    /// Maps a list of <see cref="LearningComponentAudit"/> entities to a list of <see cref="LearningComponentAuditDto"/> objects.
    /// </summary>
    /// <param name="learningComponentAudits">The list of audit entities to map.</param>
    /// <returns>A list of <see cref="LearningComponentAuditDto"/> objects representing the mapped audits.</returns>
    public static List<LearningComponentAuditDto> ToDtoList(List<LearningComponentAudit> learningComponentAudits)
    {
        return learningComponentAudits.Select(s => new LearningComponentAuditDto(
         LearningComponentAuditId: s.LearningComponentAuditId,
         ComponentId: s.ComponentId,
         Width: s.Width,
         Height: s.Height,
         Depth: s.Depth,
         X: s.X,
         Y: s.Y,
         Z: s.Z,
         Orientation: s.Orientation!,
         IsDeleted: s.IsDeleted,
         ComponentType: s.ComponentType,
         MarkerColor: s.MarkerColor!,
         ProjectedContent: s.ProjectedContent!,
         ProjectedHeight: s.ProjectedHeight!,
         ProjectedWidth: s.ProjectedWidth!,
         Action: s.Action,
         ModifiedAt: s.ModifiedAt
         )).ToList();
    }
}
