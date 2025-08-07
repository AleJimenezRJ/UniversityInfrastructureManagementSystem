namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

/// <summary>
/// Interface for User Repository.
/// This interface defines the contract for user-related data access operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Deletes a user from the system by their id number.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating
    /// whether the deletion was successful or not.
    /// </returns>
    Task<bool> DeleteUserAsync(int id);

    /// <summary>
    /// Creates a user in the database.
    /// </summary>
    /// <param name="user">The user object containing the details of the user to be created.</param>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating success or failure.</returns>
    Task<bool> CreateUserAsync(User user);


    /// Retrieves all Users from the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Users"/> objects
    /// if any exist, or an empty list if no Users are found in the database.
    Task<List<User>> GetAllUsersAsync();

    /// <summary>
    /// Modifies an existing user in the database.
    /// </summary>
    /// <param name="id"></param> The id of the user to be modified.
    /// <param name="user"></param> The user object containing the updated details of the user.
    /// <returns></returns>
    Task<bool> ModifyUserAsync(int id, User user);
}
