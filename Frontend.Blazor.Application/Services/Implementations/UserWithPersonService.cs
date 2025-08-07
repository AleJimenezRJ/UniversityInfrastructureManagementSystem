using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

internal class UserWithPersonService : IUserWithPersonService
{
    private readonly IUserWithPersonRepository _userWithPersonRepository;
    private readonly IUserRoleService _userRoleService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserWithPersonService"/> class.
    /// </summary>
    /// <param name="userWithPersonRepository">The repository used to access user and person data.</param>
    public UserWithPersonService(
      IUserWithPersonRepository userWithPersonRepository,
      IUserRoleService userRoleService)
    {
        _userWithPersonRepository = userWithPersonRepository;
        _userRoleService = userRoleService;
    }

    /// <summary>
    /// Retrieves a list of all users with people in the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="UserWithPerson"/> objects
    /// if found, or an empty list if none exist in the database.
    /// </returns>
    public Task<List<UserWithPerson>> GetAllUserWithPersonAsync()
    {
        return _userWithPersonRepository.GetAllUserWithPersonAsync();
    }

    /// <summary>
    /// Creates a new userWithPerson in the system.
    /// </summary>
    /// <param name="userWithPerson"> The user object containing the details of the userWithPerson to be created.</param>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating success or failure.</returns>
    public Task<bool> CreateUserWithPersonAsync(UserWithPerson userWithPerson)
    {
        return _userWithPersonRepository.CreateUserWithPersonAsync(userWithPerson);
    }

    /// <summary>
    /// Deletes a user with person from the system by their user id and person id.
    /// </summary>
    /// <param name="personId">The unique identifier for the person associated with the user.</param>
    /// <param name="userId">The unique identifier for the user associated with the user.</param>
    /// <returns> A task that represents the asynchronous operation.
    /// The task result contains a boolean indicating whether the deletion was successful or not.</returns>
    public Task<bool> DeleteUserWithPersonAsync(int userId, int personId)
    {
        return _userWithPersonRepository.DeleteUserWithPersonAsync(userId, personId);
    }

    /// <summary>
    /// Updates a user with associated personal information in the database.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.
    /// </returns>
    /// <exception cref="ArgumentNullException"></exception>
    public Task<bool> UpdateUserWithPersonAsync(UserWithPerson user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null");
        }
        // Validate the user object here if necessary
        return _userWithPersonRepository.UpdateUserWithPersonAsync(user);
    }
    /// <summary>
    /// Retrieves a paginated list of users with associated personal information and their assigned roles.
    /// </summary>
    /// <param name="pageSize">The number of users to return in the current page.</param>
    /// <param name="lastUserId">The ID of the last user from the previous page (used for cursor-based pagination).</param>
    /// <returns>
    /// A <see cref="PaginatedList{UserWithPerson}"/> containing the users for the requested page.
    /// </returns>
    public Task<PaginatedList<UserWithPerson>> GetPaginatedUsersAsync(int pageSize, int pageNumber, string searchText)
    {
        return _userWithPersonRepository.GetPaginatedUsersAsync(pageSize, pageNumber, searchText);
    }

    /// <summary>
    /// Validates whether the specified <see cref="UserWithPerson"/> instance is unique in the system.
    /// This method checks for the existence of a user with the same identifying information (such as username, email, or identity number)
    /// to prevent duplicate user entries.
    /// </summary>
    /// <param name="userWithPerson">The <see cref="UserWithPerson"/> object containing the details to validate for uniqueness.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains a boolean value:
    /// <c>true</c> if the user is unique and does not already exist in the system; otherwise, <c>false</c>.
    /// </returns>
    public Task<bool> ValidateUserUniquenessAsync(UserWithPerson userWithPerson)
    {
        return _userWithPersonRepository.ValidateUserUniquenessAsync(userWithPerson);
    }

    /// <summary>
    /// Gets a user ID by email address.
    /// </summary>
    /// <param name="email"> The email address of the user to retrieve.</param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    public Task<int?> GetUserIdByEmailAsync(string email)
    {
        return _userWithPersonRepository.GetUserIdByEmailAsync(email);
    }
}
