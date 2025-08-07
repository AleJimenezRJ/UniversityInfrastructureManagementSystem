using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;
using Microsoft.Kiota.Abstractions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Repository implementation for user and person data access using the Kiota-generated API client.
/// Provides methods to retrieve and create user-person combined entities from the backend API.
/// </summary>
internal class KiotaUsersRepository : IUserWithPersonRepository
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="KiotaUsersRepository"/> class.
    /// </summary>
    /// <param name="apiClient">The Kiota-generated API client used for HTTP requests.</param>
    public KiotaUsersRepository(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Retrieves all users with their associated person information from the backend API.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of <see cref="UserWithPerson"/> entities.
    /// </returns>
    /// <exception cref="EntityNotFoundException">Thrown if no users are found.</exception>
    public async Task<List<UserWithPerson>> GetAllUserWithPersonAsync()
    {
        var response = await _apiClient.Userwithperson.GetAsync();

        if (response == null || !response.Any())
            return new List<UserWithPerson>();

        return response.Select(dto => dto.ToEntity()).ToList();
    }

    /// <summary>
    /// Creates a new user with associated person information in the backend API.
    /// </summary>
    /// <param name="userWithPerson">The <see cref="UserWithPerson"/> entity containing user and person details to create.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result is <c>true</c> if the creation was successful; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> CreateUserWithPersonAsync(UserWithPerson userWithPerson)
    {
        var request = new PostCreateUserWithPersonRequest
        {
            UserWithPerson = new CreateUserWithPersonDto
            {
                UserName = userWithPerson.UserName.Value,
                FirstName = userWithPerson.FirstName,
                LastName = userWithPerson.LastName,
                Email = userWithPerson.Email.Value,
                Phone = userWithPerson.Phone.Value,
                IdentityNumber = userWithPerson.IdentityNumber.Value,
                // Documentation: https://learn.microsoft.com/en-us/openapi/kiota/abstractions
                BirthDate = new Microsoft.Kiota.Abstractions.Date(
                    userWithPerson.BirthDate.Value.Year,
                    userWithPerson.BirthDate.Value.Month,
                    userWithPerson.BirthDate.Value.Day
                ),
                Roles = userWithPerson.Roles
            }
        };

        var response = await _apiClient.Userwithperson.Create.PostAsync(request);
        return response is not null;
    }
    /// <summary>
    /// Deletes a user with associated person information from the backend API by their user ID and person ID.
    /// </summary>
    /// <param name="userId"> The unique identifier for the user to be deleted.</param>
    /// <param name="personId"> The unique identifier for the person associated with the user to be deleted.</param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    public async Task<bool> DeleteUserWithPersonAsync(int userId, int personId)
    {
        var response = await _apiClient.Userwithperson.DeletePath.DeleteAsync(options =>
        {
            options.QueryParameters.UserId = userId;
            options.QueryParameters.PersonId = personId;
        });

        return response != null;
    }

    /// <summary>
    /// Updates an existing user with associated person information in the backend API.
    /// </summary>
    /// <param name="userWithPerson"> The <see cref="UserWithPerson"/> entity containing updated user and person details.</param>
    /// <returns></returns>
    public async Task<bool> UpdateUserWithPersonAsync(UserWithPerson userWithPerson)
    {
        var dto = new CreateUserWithPersonDto
        {
            UserName = userWithPerson.UserName.Value,
            FirstName = userWithPerson.FirstName,
            LastName = userWithPerson.LastName,
            Email = userWithPerson.Email.Value,
            Phone = userWithPerson.Phone.Value,
            IdentityNumber = userWithPerson.IdentityNumber.Value,
            BirthDate = userWithPerson.BirthDate.Value,
            Roles = userWithPerson.Roles
        };

        var request = new PutModifyUserWithPersonRequest
        {
            IdentityNumber = userWithPerson.IdentityNumber.Value,
            UserWithPerson = dto
        };

        var result = await _apiClient.Userwithperson.Modify.PutAsync(request);
        return result is not null;
    }

    /// <summary>
    /// Retrieves a paginated list of users with their associated person information from the backend API.
    /// </summary>
    /// <param name="pageSize">The maximum number of users to return per page.</param>
    /// <param name="pageNumber">The current page number (zero-based index).</param>
    /// <param name="searchText">A search string to filter users by name, email, or other criteria.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a <see cref="PaginatedList{UserWithPerson}"/> 
    /// with the requested users and pagination metadata.
    /// </returns>
    public async Task<PaginatedList<UserWithPerson>> GetPaginatedUsersAsync(int pageSize, int pageNumber, string searchText)
    {
        var response = await _apiClient.Userwithperson.Paginated.GetAsync(options =>
        {
            options.QueryParameters.PageSize = pageSize;
            options.QueryParameters.PageNumber = pageNumber;
            options.QueryParameters.SearchText = searchText;
        });

        if (response == null || response.Users == null)
        {
            return PaginatedList<UserWithPerson>.Empty(pageSize, pageNumber);
        }

        var users = response.Users.Select(dto => dto.ToEntity()).ToList();

        return new PaginatedList<UserWithPerson>(
            users,
            response.TotalCount ?? 0,
            pageSize,
            response.PageNumber ?? 0
        );
    }


    /// <summary>
    /// Validates the uniqueness of a user's username, email, and identity number.
    /// Checks if any other user in the system has the same username, email, or identity number,
    /// excluding the current user (by UserId and PersonId). Throws a <see cref="DuplicatedEntityException"/>
    /// if a duplicate is found for any of these fields.
    /// </summary>
    /// <param name="user">The <see cref="UserWithPerson"/> entity to validate for uniqueness.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result is <c>true</c> if the user is unique.
    /// </returns>
    /// <exception cref="DuplicatedEntityException">
    /// Thrown if the username, email, or identity number is already registered to another user.
    /// </exception>
    public async Task<bool> ValidateUserUniquenessAsync(UserWithPerson user)
    {
        var users = await GetAllUserWithPersonAsync();

        bool userNameExists = users.Any(u =>
            u.UserName.Value == user.UserName.Value && u.UserId != user.UserId);

        if (userNameExists)
            throw new DuplicatedEntityException("El nombre de usuario ya está registrado.");

        bool emailExists = users.Any(u =>
            u.Email.Value == user.Email.Value && u.PersonId != user.PersonId);

        if (emailExists)
            throw new DuplicatedEntityException("El correo electrónico ya está registrado.");

        bool identityExists = users.Any(u =>
            u.IdentityNumber.Value == user.IdentityNumber.Value && u.PersonId != user.PersonId);

        if (identityExists)
            throw new DuplicatedEntityException("El número de identificación ya está registrado.");

        return true;
    }


    /// <summary>
    /// Retrieves the user ID based on the provided email using the backend API.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>The user ID if found; otherwise, <c>null</c>.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the user is not found.</exception>
    public async Task<int?> GetUserIdByEmailAsync(string email)
    {
        try
        {
            var response = await _apiClient
                .User
                .IdByEmail
                .GetAsync(config =>
                {
                    config.QueryParameters.Email = email;
                });

            return response;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}


