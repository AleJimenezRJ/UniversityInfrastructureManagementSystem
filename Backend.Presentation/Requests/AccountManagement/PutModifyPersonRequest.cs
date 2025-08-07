using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to modify an existing person's information.
/// </summary>
/// <param name="Person">The data transfer object containing the updated details of the person.</param>
public record class PutModifyPersonRequest(PersonDto Person);
