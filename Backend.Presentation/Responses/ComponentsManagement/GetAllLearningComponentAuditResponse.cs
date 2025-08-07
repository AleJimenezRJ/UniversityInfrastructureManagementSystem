using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

/// <summary>
/// Represents the response containing a list of learning component audit records.
/// </summary>
public record class GetAllLearningComponentAuditResponse(List<LearningComponentAuditDto> Logs);