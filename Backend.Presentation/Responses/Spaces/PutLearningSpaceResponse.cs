using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

/// <summary>
/// Represents the response returned after successfully updating a learning space.
/// </summary>
/// <param name="LearningSpaceDto"> The updated learning space data represented as a Data Transfer Object (DTO). </param>
/// <param name="Message"> A message indicating the result of the creation operation (e.g., success). </param>
public record class PutLearningSpaceResponse(LearningSpaceDto LearningSpaceDto, string Message);
