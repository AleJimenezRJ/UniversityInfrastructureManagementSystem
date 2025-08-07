using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.Spaces;

/// <summary>
/// Represents a request to create a new learning space.
/// </summary>
/// <param name="LearningSpace">
/// The data transfer object (DTO) containing the details of the learning space to be created.
/// </param>
public record class PostLearningSpaceRequest(LearningSpaceDto LearningSpace);
