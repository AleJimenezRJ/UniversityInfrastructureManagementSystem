using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response after creating a new user with associated person information.
/// </summary>
/// <param name="UserWithPerson">The user with person data that was created.</param>
/// <param name="Message">A message indicating the result of the operation.</param>
public record class PostCreateUserWithPersonResponse(CreateUserWithPersonDto UserWithPerson, string Message);