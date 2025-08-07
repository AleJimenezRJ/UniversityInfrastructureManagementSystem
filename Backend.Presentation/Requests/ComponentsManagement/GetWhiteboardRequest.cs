using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;

/// <summary>
/// Represents the response for a request to get whiteboard information.
/// </summary>
/// <param name="Whiteboard"></param>
public record class GetWhiteboardRequest(WhiteboardDto Whiteboard);