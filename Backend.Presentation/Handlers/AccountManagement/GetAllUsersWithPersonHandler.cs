using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handler for retrieving all users with their associated person data.
/// </summary>
public static class GetAllUsersWithPersonHandler
{
    /// <summary>
    /// Handles the request to retrieve all users and their associated person details.
    /// </summary>
    /// <param name="userService">The user service.</param>
    /// <param name="personService">The person service.</param>
    /// <param name="userRoleService">The user-role service.</param>
    /// <returns>A list of users with person data or an appropriate message.</returns>
    public static async Task<Results<
        Ok<List<UserWithPersonDto>>,
        NotFound<string>,
        Conflict<string>>> HandleAsync(

            [FromServices] IUserService userService,
        [FromServices] IPersonService personService,
        [FromServices] IUserRoleService userRoleService)
    {
        try
        {
            var users = await userService.GetAllUsersAsync();
            var people = await personService.GetAllPeopleAsync();

            if (users.Count == 0 || people.Count == 0)
            {
                return TypedResults.NotFound("There are no registered users.");
            }

            var dtoList = new List<UserWithPersonDto>();

            foreach (var user in users)
            {
                var person = people.FirstOrDefault(p => p.Id == user.PersonId);
                if (person is null)
                    continue;

                var roles = await userRoleService.GetRolesByUserIdAsync(user.Id);
                var roleNames = roles?.Select(r => r.Name).ToList() ?? new List<string>();

                dtoList.Add(new UserWithPersonDto(
                    user.Id,
                    user.UserName.Value,
                    person.Id,
                    person.Email.Value,
                    person.FirstName,
                    person.LastName,
                    person.Phone.Value,
                    person.BirthDate.Value,
                    person.IdentityNumber.Value,
                    roleNames
                ));
            }

            return TypedResults.Ok(dtoList);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
