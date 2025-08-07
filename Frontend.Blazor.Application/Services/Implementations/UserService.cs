using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Provides implementation for user-related operations defined in <see cref="IUserService"/>.
/// </summary>
internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userRepository">The repository used to access user data.</param>
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public Task<bool> DeleteUserAsync(int id)
    {
        return _userRepository.DeleteUserAsync(id);
    }

    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    /// <param name="user"> The user object containing the details of the user to be created.</param>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating success or failure.</returns>
    public Task<bool> CreateUserAsync(User user)
    {
        return _userRepository.CreateUserAsync(user);
    }

    /// <summary>
    /// Retrieves a list of all users in the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="User"/> objects
    /// if found, or an empty list if no user exists in the database.
    /// </returns>
    public Task<List<User>> GetAllUsersAsync()
    {
        return _userRepository.GetAllUsersAsync();
    }

    /// <summary>
    /// Modifies an existing user in the system.
    /// </summary>
    /// <param name="id">The unique identifier of the user to modify.</param>
    /// <param name="user">The user object containing the updated details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating
    /// whether the modification was successful or not.
    /// </returns>
    public Task<bool> ModifyUserAsync(int id, User user)
    {
        return _userRepository.ModifyUserAsync(id, user);
    }
}

