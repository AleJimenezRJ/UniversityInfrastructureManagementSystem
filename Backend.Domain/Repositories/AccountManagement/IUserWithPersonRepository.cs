using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;

/// <summary>
/// Interface for User with person Repository.
/// This interface defines the contract for user-person-related data access operations.
/// </summary>
public interface IUserWithPersonRepository
{
    /// <summary>
    /// Retrieves a list of <see cref="UserWithPerson"/>.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of <see cref="UserWithPerson"/>.
    /// </returns>
    Task<List<UserWithPerson>> GetAllUserWithPersonAsync();

    /// <summary>
    /// Creates a <see cref="UserWithPerson"/> record in the database
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
    /// <param name="user"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.
    /// </returns>
    Task<bool> UpdateUserWithPersonAsync(UserWithPerson user);

    /// <summary>
    /// Retrieves a paginated list of <see cref="UserWithPerson"/> using keyset pagination.
    /// </summary>
    /// <param name="pageSize">The maximum number of users to return.</param>
    /// <param name="lastUserId">The ID of the last user from the previous page, used to fetch the next set of results.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="PaginatedList{UserWithPerson}"/> with the requested users and pagination metadata.
    /// </returns>
    Task<PaginatedList<UserWithPerson>> GetPaginatedUsersAsync(int pageSize, int pageNumber, string searchText);


    /// <summary>
    /// Validates the uniqueness of a <see cref="UserWithPerson"/> entity.
    /// Checks if the provided user or person information (such as username, email, or identity number)
    /// already exists in the system to prevent duplicate records.
    /// </summary>
    /// <param name="userWithPerson">The <see cref="UserWithPerson"/> instance to validate for uniqueness.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean value:
    /// <c>true</c> if the user and person information is unique; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> ValidateUserUniquenessAsync(UserWithPerson userWithPerson);

    /// <summary>
    /// Retrieves a user with personal information and their roles by email.
    /// </summary>
    /// <param name="email"> The email address of the user to retrieve.</param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    Task<int?> GetUserIdByEmailAsync(string email);
}
