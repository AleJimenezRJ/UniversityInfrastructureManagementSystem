using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response for creating a new user.
/// </summary>
/// <param name="User"> The details of the created user.</param>
/// <param name="Message"> A message indicating the result of the operation.</param>
public record class PostCreateUserResponse(CreateUserDto User, string Message);