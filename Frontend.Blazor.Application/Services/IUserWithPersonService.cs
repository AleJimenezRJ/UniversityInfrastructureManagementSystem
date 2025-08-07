using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
/// <summary>
/// Defines the contract for user-person-related operations in the application.
/// This interface is intended to be implemented by services that handle user management functionality.
/// </summary>

public interface IUserWithPersonService
{
    /// <summary>
    /// Retrieves all users with associated personal information and their assigned roles.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of 
    /// <see cref="UserWithPerson"/> objects enriched with role information.
    /// </returns>
    Task<List<UserWithPerson>> GetAllUserWithPersonAsync();

    /// <summary>
    /// Creates a UserWithPerson in the database.
    /// </summary>
    /// <param name="userWithPerson">The user object containing the details of the userWithPerson to be created.</param>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating success or failure.</returns>
    Task<bool> CreateUserWithPersonAsync(UserWithPerson userWithPerson);

    /// <summary>
    /// Deletes a user with person from the system by their user id and person id.
    /// </summary>
    /// <param name="personId">The unique identifier for the person associated with the user.</param>
    /// <param name="userId">The unique identifier for the user associated with the user.</param>
    /// <returns> A task that represents the asynchronous operation.
    /// The task result contains a boolean indicating whether the deletion was successful or not.</returns>
    Task<bool> DeleteUserWithPersonAsync(int userId, int personId);

    /// <summary>
    /// Updates a user with associated personal information in the database.
    /// </summary>
    /// <param name="user"> The user object containing the updated details of the user with person information.</param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    Task<bool> UpdateUserWithPersonAsync(UserWithPerson user);

    /// <summary>
    /// Retrieves a paginated list of users with associated personal information and their assigned roles.
    /// </summary>
    /// <param name="pageSize">The number of users per page.</param>
    /// <param name="lastUserId">The ID of the last user from the previous page, used to retrieve the next set of results.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a paginated list of <see cref="UserWithPerson"/> objects.
    /// </returns>
    Task<PaginatedList<UserWithPerson>> GetPaginatedUsersAsync(int pageSize, int pageNumber, string searchText);

    /// <summary>
    /// Validates whether the specified <see cref="UserWithPerson"/> instance is unique in the system.
    /// This typically checks for uniqueness based on key properties such as username, email, phone, or identity number.
    /// </summary>
    /// <param name="userWithPerson">The user-person object to validate for uniqueness.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating
    /// whether the user-person combination is unique (true) or already exists (false).
    /// </returns>
    Task<bool> ValidateUserUniquenessAsync(UserWithPerson userWithPerson);

    /// <summary>
    /// Retrieves a user with personal information and their assigned roles by email.
    /// </summary>
    /// <param name="email"> The email address of the user to retrieve.</param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    Task<int?> GetUserIdByEmailAsync(string email);
}
