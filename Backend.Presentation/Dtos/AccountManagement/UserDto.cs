namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

/// <summary>
/// Data Transfer Object (DTO) representing a user.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="UserName">The username of the user.</param>
/// <param name="personId">The identifier of the associated person entity.</param>
public record class UserDto(int Id, string UserName, int personId);

/// <summary>
/// Data Transfer Object (DTO) representing a user.
/// </summary>
/// <param name="UserName">The username of the user.</param>
/// <param name="PersonId">The identifier of the associated person entity.</param>
public record class CreateUserDto(string UserName, int PersonId);

/// <summary>
/// Data Transfer Object (DTO) representing a user.
/// </summary>
/// <param name="UserName">The new username of the user.</param>
public record class ModifyUserDto(string UserName);