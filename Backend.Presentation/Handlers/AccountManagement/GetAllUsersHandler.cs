using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handler for retrieving all registered users.
/// </summary>
public static class GetAllUsersHandler
{
    /// <summary>
    /// Handles the request to retrieve all users.
    /// </summary>
    /// <param name="UserService">The service used to retrieve person data.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains:
    /// An <see cref="Ok{T}"/> result with a <see cref="GetAllUsersResponse"/>, containing a list (possibly empty) of users.
    /// A <see cref="NotFound{T}"/> result if no users are registered.
    /// A <see cref="Conflict{T}"/> result if there is a domain exception while retrieving users.
    /// </returns>
    public static async Task<Results<Ok<GetAllUsersResponse>, NotFound<string>, Conflict<string>>> HandleAsync(
        [FromServices] IUserService userService)
    {
        try
        {
            var users = await userService.GetAllUsersAsync();
            if (users.Count == 0)
            {
                return TypedResults.NotFound("There are no registered users.");
            }

            var dtoList = UserDtoMapper.ToDtoList(users);
            var response = new GetAllUsersResponse
            {
                Users = dtoList
            };
            return TypedResults.Ok(response);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
