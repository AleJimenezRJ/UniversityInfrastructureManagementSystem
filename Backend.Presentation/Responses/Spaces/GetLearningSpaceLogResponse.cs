using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

/// <summary>
/// Represents the response containing a collection of learning space logs.
/// </summary>
/// <remarks>
/// This response is typically used to return a list of learning space log entries from an API or service.
/// Each log entry is represented by a <see cref="LearningSpaceLogDto"/>.
/// </remarks>
/// <param name="Logs">The collection of learning space log entries.</param>
public record class GetLearningSpaceLogResponse(List<LearningSpaceLogDto> Logs);
