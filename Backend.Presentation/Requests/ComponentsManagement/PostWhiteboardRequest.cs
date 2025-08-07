using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;

/// <summary>
/// Represents a request to create a new whiteboard.
/// </summary>
/// <param name="Whiteboard"></param>
public record class PostWhiteboardRequest(WhiteboardNoIdDto Whiteboard);
