using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to create a new person.
/// </summary>
/// <param name="Person"> The details of the person to be created.</param>
public record class PostCreatePersonRequest(CreatePersonDto Person);
