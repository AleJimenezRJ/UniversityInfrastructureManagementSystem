using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

/// <summary>
/// Represents a response containing a list of floors.
/// </summary>
/// <param name="Floor">A list of floors transferred in the response.</param>
public record class GetFloorListResponse(List<FloorDto> Floors);
