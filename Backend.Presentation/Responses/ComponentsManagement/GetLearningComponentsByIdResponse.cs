using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

/// <summary>
/// Response wrapper for components retrieved by space identifiers.
/// </summary>
/// <param name="LearningComponents">The list of DTO of the learning component belonging to a specific space.</param>
public record GetLearningComponentsByIdResponse(
    List<LearningComponentDto> LearningComponents,
    int PageSize,
    int PageIndex,
    int TotalCount,
    int TotalPages
);
