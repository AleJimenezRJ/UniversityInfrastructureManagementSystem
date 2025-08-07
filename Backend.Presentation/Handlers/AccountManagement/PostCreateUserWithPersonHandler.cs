using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;


/// <summary>
/// Handles the creation of a new user with person.
/// </summary>
public static class PostCreateUserWithPersonHandler
{
    /// <summary>
    /// Handles the HTTP POST request to create a new user with person.
    /// </summary>
    /// <param name="userWithPersonService"> The service used to manage user-person-related operations.</param>
    /// <param name="request"> The request containing the details of the user to be created.</param>
    /// <returns></returns>
    public static async Task<Results<
    Ok<PostCreateUserWithPersonResponse>,
    NotFound<string>,
    Conflict<string>,
    BadRequest<List<ValidationError>>>> HandleAsync(
    [FromServices] IUserWithPersonService userWithPersonService,
    [FromBody] PostCreateUserWithPersonRequest request)
    {
        UserWithPerson? userWithPersonEntity = null;
        List<ValidationError> errorMessages = [];

        try
        {
            userWithPersonEntity = UserWithPersonDtoMapper.ToEntity(request.UserWithPerson);
        }
        catch (ValidationException exception)
        {
            errorMessages.Add(new ValidationError("UserWithPerson", exception.Message));
        }

        if (errorMessages.Count > 0 || userWithPersonEntity == null)
        {
            return TypedResults.BadRequest(errorMessages);
        }

        try
        {
            var additionSucceeded = await userWithPersonService.CreateUserWithPersonAsync(userWithPersonEntity);

            if (!additionSucceeded)
            {
                return TypedResults.Conflict("The username or person identity already exists.");
            }

            return TypedResults.Ok(new PostCreateUserWithPersonResponse(request.UserWithPerson, "Successful creation."));
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}

