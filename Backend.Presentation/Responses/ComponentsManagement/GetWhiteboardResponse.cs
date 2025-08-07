using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

/// <summary>
/// Response wrapper for a whiteboard component.
/// </summary>
/// <param name="Whiteboards">The DTO containing whiteboard-specific data.</param>
public record class GetWhiteboardResponse(List<WhiteboardDto> Whiteboards);
