using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;


/// <summary>
/// Represents the response for a PUT request to modify a person's details.
/// </summary>
/// <param name="PersonDto">The data transfer object containing the modified person's details.</param>
public record class PutModifyPersonResponse(PersonDto PersonDto);
