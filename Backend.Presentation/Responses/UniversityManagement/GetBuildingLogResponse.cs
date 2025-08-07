using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

/// <summary>
/// Represents the response containing a collection of building logs.
/// </summary>
/// <remarks>This response is typically used to return a list of building log entries from an API or service. Each
/// log entry is represented by a <see cref="BuildingLogDto"/>.</remarks>
/// <param name="Logs">The collection of building log entries.</param>
public record class GetBuildingLogResponse(List<BuildingLogDto> Logs);