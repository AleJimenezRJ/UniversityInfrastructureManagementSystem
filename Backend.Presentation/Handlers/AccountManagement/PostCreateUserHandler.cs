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
/// Handles the creation of a new user.
/// </summary>
public static class PostCreateUserHandler
{

    /// <summary>
    /// Handles the HTTP POST request to create a new user.
    /// </summary>
    /// <param name="userService"> The service used to manage user-related operations.</param>
    /// <param name="request"> The request containing the details of the user to be created.</param>
    /// <returns>
    /// A result that can be:
    /// An <see cref="Ok{T}"/> with a success message if the user was created successfully,
    /// A <see cref="NotFound{T}"/> if the user could not be found,
    /// A <see cref="Conflict{T}"/> if there was a conflict during user creation,
    /// A <see cref="BadRequest{T}"/> with validation errors if the request data is invalid.
    /// </returns>
    public static async Task<Results<Ok<PostCreateUserResponse>, NotFound<string>, Conflict<string>, BadRequest<List<ValidationError>>>> HandleAsync(
      [FromServices] IUserService userService,
      [FromBody] PostCreateUserRequest request)
    {
        var errors = new List<ValidationError>();
        User? userEntity = null;
        try
        {
            userEntity = UserDtoMapper.ToEntity(request.User);
        }
        catch (ValidationException exception)
        {
            errors.Add(new ValidationError("User", exception.Message));
        }

        if (errors.Count > 0 || userEntity == null)
        {
            return TypedResults.BadRequest(errors);
        }
        try
        {
            await userService.CreateUserAsync(userEntity);
            return TypedResults.Ok(new PostCreateUserResponse(request.User, "User created successfully."));
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (DuplicatedEntityException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}

