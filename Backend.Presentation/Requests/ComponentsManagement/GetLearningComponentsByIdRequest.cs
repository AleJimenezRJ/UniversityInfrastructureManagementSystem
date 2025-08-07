using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;

/// <summary>
/// Request wrapper for retrieving components in a specific learning space by ID.
/// </summary>
/// <param name="LearningComponent">The DTO containing the identifiers of the building, floor, and learning space.</param>
public record class GetLearningComponentsByIdRequest(LearningComponentDto LearningComponent);
