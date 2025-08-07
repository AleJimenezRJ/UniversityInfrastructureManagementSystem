using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

/// <summary>
/// Represents the response for creating a new floor.
/// </summary>
/// <param name="Message">
/// A message indicating the result of the creation operation (e.g., success).
/// </param>
public record class PostFloorResponse(string Message);
