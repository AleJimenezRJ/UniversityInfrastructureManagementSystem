using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;

/// <summary>
/// Request wrapper for retrieving a learning component.
/// </summary>
/// <param name="LearningComponent">The DTO representing the learning component to retrieve.</param>
public record class GetLearningComponentRequest(LearningComponentDto LearningComponent);
