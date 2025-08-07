namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

/// <summary>
/// Data Transfer Object (DTO) that represents a minimal view of a learning space list object,
/// containing only its name and type.
/// </summary>
/// <param name="LearningSpaceId">
/// The identifier of the learning space is located.
/// </param>
/// <param name="Name">
/// The name of the learning space.
/// </param>
/// <param name="Type">
/// The type or category of the learning space (e.g., classroom, lab, auditorium).
/// </param>
public record class LearningSpaceListDto(
    int LearningSpaceId,
    string Name,
    string Type
);
