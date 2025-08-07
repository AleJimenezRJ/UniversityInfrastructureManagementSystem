using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handles the modification of an existing user with associated person information.
/// </summary>
public static class PutModifyUserWithPersonHandler
{
    /// <summary>
    /// Handles the HTTP PUT request to modify an existing user with person information.
    /// </summary>
    /// <param name="userWithPersonService"> The service for user-person operations.</param>
    /// <param name="request"> The request containing the identity number and user data to be modified.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static async Task<Results<
        Ok<PutModifyUserWithPersonResponse>,
        NotFound<string>,
        BadRequest<ErrorResponse>>> HandleAsync(
        [FromServices] IUserWithPersonService userWithPersonService,
        [FromBody] PutModifyUserWithPersonRequest request)
    {
        var errorMessages = new List<string>();
        IdentityNumber? identityNumber = null;

        // Validate identity number
        try
        {
            identityNumber = IdentityNumber.Create(request.IdentityNumber);
        }
        catch (ValidationException ex)
        {
            errorMessages.Add(ex.Message);
        }

        // Validate request body
        if (request.UserWithPerson is null)
        {
            errorMessages.Add("UserWithPerson data is required.");
        }

        if (errorMessages.Count > 0 || identityNumber is null)
        {
            return TypedResults.BadRequest(new ErrorResponse(errorMessages));
        }

        // Look for existing user
        var allUsers = await userWithPersonService.GetAllUserWithPersonAsync();
        var existingUser = allUsers.FirstOrDefault(u => u.IdentityNumber.Value == identityNumber.Value);

        if (existingUser is null)
        {
            return TypedResults.NotFound($"User with identity number {identityNumber.Value} not found.");
        }

        // Update entity
        UserWithPersonDtoMapper.UpdateEntity(existingUser, request.UserWithPerson!);

        try
        {
            var updated = await userWithPersonService.UpdateUserWithPersonAsync(existingUser);

            if (!updated)
            {
                return TypedResults.BadRequest(new ErrorResponse(["Could not update the user."]));
            }

            return TypedResults.Ok(new PutModifyUserWithPersonResponse(request.UserWithPerson));
        }
        catch (DuplicatedEntityException ex)
        {
            return TypedResults.BadRequest(new ErrorResponse([ex.Message]));
        }
        catch (NotFoundException)
        {
            return TypedResults.NotFound($"User with identity number {identityNumber.Value} not found.");
        }
        catch (DomainException ex)
        {
            return TypedResults.BadRequest(new ErrorResponse([ex.Message]));
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(new ErrorResponse(["An unexpected error occurred.", ex.Message]));
        }
    }
}
