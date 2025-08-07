using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to create a new user.
/// </summary>
/// <param name="User"> The details of the user to be created.</param>
public record class PostCreateUserRequest(CreateUserDto User);
