using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

/// <summary>
/// Represents the response returned after successfully updating a whiteboard.
/// </summary>
/// <param name="Whiteboard"></param>

public record class PutWhiteboardResponse(WhiteboardNoIdDto Whiteboard);
