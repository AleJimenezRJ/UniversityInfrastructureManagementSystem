using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.ComponentsManagement;


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
        return learningComponentAudits.Select(s => new LearningComponentAuditDto
        {
            LearningComponentAuditId = s.LearningComponentAuditId,
            ComponentId = s.ComponentId,
            Width = (double?)s.Width,
            Height = (double?)s.Height,
            Depth = (double?)s.Depth,
            X = (double?)s.X,
            Y = (double?)s.Y,
            Z = (double?)s.Z,
            Orientation = s.Orientation!,
            IsDeleted = s.IsDeleted,
            ComponentType = s.ComponentType,
            MarkerColor = s.MarkerColor!,
            ProjectedContent = s.ProjectedContent!,
            ProjectedHeight = (double?)s.ProjectedHeight,
            ProjectedWidth = (double?)s.ProjectedWidth,
            Action = s.Action,
            ModifiedAt = s.ModifiedAt
        }).ToList();
    }

    /// <summary>
    /// Converts a <see cref="LearningComponentAudit"/> Data Transfer Object (DTO) to a <see cref="LearningComponentAudit"/> entity.
    /// This extension method facilitates the mapping of properties from the DTO to a new entity instance.
    /// </summary>
    /// <param name="dto">The <see cref="LearningComponentAudit"/> DTO instance to convert.</param>
    /// <returns>A new <see cref="LearningComponentAudit"/> entity populated with data from the provided DTO.</returns>
    public static LearningComponentAudit ToEntity(this LearningComponentAuditDto dto)
    {
        return new LearningComponentAudit
        {
            LearningComponentAuditId = (int) dto.LearningComponentAuditId!,
            ComponentId = (int) dto.ComponentId!,
            Width = (decimal?) dto.Width,
            Height = (decimal?) dto.Height,
            Depth = (decimal?) dto.Depth,
            X = (decimal?) dto.X,
            Y = (decimal?) dto.Y,
            Z = (decimal?) dto.Z,
            Orientation = dto.Orientation!,
            IsDeleted = (bool) dto.IsDeleted!,
            ComponentType = dto.ComponentType!,
            MarkerColor = dto.MarkerColor!,
            ProjectedContent = dto.ProjectedContent!,
            ProjectedHeight = (decimal?) dto.ProjectedHeight,
            ProjectedWidth = (decimal?) dto.ProjectedWidth,
            Action = dto.Action!,
            ModifiedAt = dto.ModifiedAt?.UtcDateTime ?? default
        };
    }   
}
