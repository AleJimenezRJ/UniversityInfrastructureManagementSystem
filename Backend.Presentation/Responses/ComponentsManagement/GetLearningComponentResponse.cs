using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

/// <summary>
/// Response wrapper for a single learning component.
/// </summary>
/// <param name="LearningComponents">The list of DTO representing the learning component returned by the API.</param>
public record class GetLearningComponentResponse(List<LearningComponentDto> LearningComponents);
