using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

/// <summary>
/// Represents a response containing a single whiteboard.
/// </summary>
/// <param name="Whiteboard"></param>
public record class PostWhiteboardResponse(WhiteboardNoIdDto Whiteboard);

