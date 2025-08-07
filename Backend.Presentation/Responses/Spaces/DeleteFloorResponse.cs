namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

/// <summary>
/// Represents a response containing a message indicating the result of a delete floor operation.
/// </summary>
/// <param name="Message">Message that contains the result of trying to delete a floor</param>
public record class DeleteFloorResponse(string Message);
