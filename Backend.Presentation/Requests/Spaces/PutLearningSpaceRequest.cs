using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.Spaces;

/// <summary>
/// Represents a request to update an existing learning space.
/// </summary>
/// <param name="LearningSpace"></param>

public record class PutLearningSpaceRequest(LearningSpaceDto LearningSpace);