using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handles the HTTP PUT request to modify an existing user in the system.
/// Validates the input, checks for user existence, updates the user entity, and persists the changes.
/// Returns appropriate HTTP responses based on the outcome of the operation.
/// </summary>
public static class PutModifyUserHandler
{
    /// <summary>
    /// Modifies an existing user with the specified ID using the provided user data.
    /// </summary>
    /// <param name="id">The unique identifier of the user to modify.</param>
    /// <param name="userService">The user service used to perform user operations.</param>
    /// <param name="request">The request containing the new user data.</param>
    /// <returns>
    /// An <see cref="Ok{T}"/> result with the updated user information if successful,
    /// a <see cref="Conflict"/> result if the modification could not be completed due to a conflict,
    /// or a <see cref="BadRequest{T}"/> result with error details if the request is invalid.
    /// </returns>
    public static async Task<Results<Ok<PutModifyUserResponse>, NotFound<string>, Conflict<string>, BadRequest<List<ValidationError>>>> HandleAsync(
     [FromRoute] int id,
     [FromServices] IUserService userService,
     [FromBody] PutModifyUserRequest request)
    {
        var errors = new List<ValidationError>();

        if (id <= 0)
            errors.Add(new ValidationError("UserId", "Invalid user ID."));

        if (request?.User == null || string.IsNullOrWhiteSpace(request.User.UserName) || request.User.UserName == "string")
            errors.Add(new ValidationError("UserName", "Username is required."));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        var users = await userService.GetAllUsersAsync();
        var existingUser = users.FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
            return TypedResults.NotFound("User not found.");

        UserDtoMapper.UpdateEntity(existingUser, request?.User!);

        try
        {
            await userService.ModifyUserAsync(id, existingUser);
            return TypedResults.Ok(new PutModifyUserResponse(request?.User!, "User updated successfully."));
        }
        catch (DuplicatedEntityException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
