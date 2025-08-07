using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

/// <summary>
/// Represents the response for creating a new learning space.
/// </summary>
/// <param name="LearningSpace">
/// The data transfer object (DTO) containing details of the newly created learning space.
/// </param>
/// <param name="Message">
/// A message indicating the result of the creation operation (e.g., success).
/// </param>
public record class PostLearningSpaceResponse(LearningSpaceDto LearningSpace, string Message);
