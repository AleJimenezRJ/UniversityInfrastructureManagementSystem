using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

/// <summary>
/// Represents a response containing a list of learning spaces.
/// </summary>
/// <param name="LearningSpaces">A list of learning spaces transferred in the response.</param>
public record class GetLearningSpaceListResponse(List<LearningSpaceListDto> LearningSpaces);
