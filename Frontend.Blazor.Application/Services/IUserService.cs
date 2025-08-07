using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Defines the contract for user-related operations in the application.
/// This interface is intended to be implemented by services that handle user management functionality.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Deletes a user asynchronously by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains <c>true</c> if the user was successfully deleted; otherwise, <c>false</c>.
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
    /// Modifies an existing user asynchronously by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to modify.</param>
    /// <param name="user">The user object containing the updated details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>true</c> if the user was successfully modified; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> ModifyUserAsync(int id, User user);
}
