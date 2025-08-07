using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;

/// <summary>
/// Represents a request to modify an existing whiteboard's information.
/// </summary>
/// <param name="Whiteboard">The data transfer object containing the updated details of the whiteboard.</param>
public record class PutWhiteboardRequest(WhiteboardNoIdDto Whiteboard);
