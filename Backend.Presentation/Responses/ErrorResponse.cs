
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;

public record ErrorResponse(
    List<string> ErrorMessages,
    int SubStatusCode = 0);
