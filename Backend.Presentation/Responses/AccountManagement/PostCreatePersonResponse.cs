using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response for creating a new person.
/// </summary>
/// <param name="Person"> The details of the created person.</param>
/// <param name="Message"> A message indicating the result of the operation.</param>
public record class PostCreatePersonResponse(CreatePersonDto Person, string Message);