namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;


/// <summary>
/// Data Transfer Object representing an audit record for a learning component.
/// </summary>
/// <param name="LearningComponentAuditId">Unique identifier for the audit record.</param>
/// <param name="ComponentId">Identifier of the associated learning component.</param>
/// <param name="Width">Width of the component at the time of audit.</param>
/// <param name="Height">Height of the component at the time of audit.</param>
/// <param name="Depth">Depth of the component at the time of audit.</param>
/// <param name="X">X coordinate of the component at the time of audit.</param>
/// <param name="Y">Y coordinate of the component at the time of audit.</param>
/// <param name="Z">Z coordinate of the component at the time of audit.</param>
/// <param name="Orientation">Orientation of the component at the time of audit.</param>
/// <param name="IsDeleted">Indicates if the component was marked as deleted in this audit record.</param>
/// <param name="ComponentType">Type of the component.</param>
/// <param name="MarkerColor">Color of the marker associated with the component.</param>
/// <param name="ProjectedContent">Projected content associated with the component.</param>
/// <param name="ProjectedHeight">Height of the projected content.</param>
/// <param name="ProjectedWidth">Width of the projected content.</param>
/// <param name="Action">Action performed (e.g., Created, Updated, Deleted).</param>
/// <param name="ModifiedAt">Timestamp when the audit record was created.</param>
public record class LearningComponentAuditDto(
    int LearningComponentAuditId,
    int ComponentId,
    decimal? Width,
    decimal? Height,
    decimal? Depth,
    decimal? X,
    decimal? Y,
    decimal? Z,
    string? Orientation,
    bool IsDeleted,
    string ComponentType,
    string? MarkerColor,
    string? ProjectedContent,
    decimal? ProjectedHeight,
    decimal? ProjectedWidth,
    string Action,
    DateTime ModifiedAt
);
