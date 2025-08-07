using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to create a new user with associated person information.
/// </summary>
/// <param name="UserWithPerson">The combined user and person data to be created.</param>
public record class PostCreateUserWithPersonRequest(CreateUserWithPersonDto UserWithPerson);