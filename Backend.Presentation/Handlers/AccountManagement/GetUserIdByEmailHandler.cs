using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handler for retrieving a user's ID by their email.
/// </summary>
public static class GetUserIdByEmailHandler
{
    /// <summary>
    /// Handles the HTTP GET request to retrieve a user's ID based on their email address.
    /// </summary>
    /// <param name="email"> The email address of the user whose ID is to be retrieved.</param>
    /// <param name="userWithPersonService"> The service for user-person operations.</param>
    /// <returns> A result containing the user's ID if found, or a NotFound response with an error message if not found.</returns>
    public static async Task<Results<
        Ok<int>,
        NotFound<ErrorResponse>>> HandleAsync(
        [FromQuery] string email,
        [FromServices] IUserWithPersonService userWithPersonService)
    {
        var userId = await userWithPersonService.GetUserIdByEmailAsync(email);

        if (userId == 0)
        {
            return TypedResults.NotFound(new ErrorResponse(new List<string>
            {
                $"No user found with email '{email}'."
            }));
        }

        return TypedResults.Ok(userId.Value); 
    }
}
