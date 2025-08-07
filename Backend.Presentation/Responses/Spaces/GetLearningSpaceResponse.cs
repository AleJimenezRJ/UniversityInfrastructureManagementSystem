using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

/// <summary>
/// Represents a response containing a single learning space.
/// </summary>
/// <param name="LearningSpace">The learning space data transferred in the response.</param>
public record class GetLearningSpaceResponse(LearningSpaceDto LearningSpace);
