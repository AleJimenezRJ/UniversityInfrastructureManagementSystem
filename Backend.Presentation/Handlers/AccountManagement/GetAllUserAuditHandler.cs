using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handles the retrieval of all user audit records.
/// </summary>
public static class GetAllUserAuditHandler
{
    /// <summary>
    /// Handles the HTTP GET request to retrieve all user audit records.
    /// </summary>
    /// <param name="userAuditService"></param>
    /// <returns>
    /// Returns a result containing either a list of user audits or a message indicating that no audits are registered.
    /// </returns>
    public static async Task<Results<Ok<List<UserAuditDto>>, Ok<string>>> HandleAsync(
    [FromServices] IUserAuditService userAuditService)
    {
        var userAudits = await userAuditService.ListUserAuditAsync();

        if (userAudits == null || userAudits.Count == 0)
        {
            return TypedResults.Ok("There are no registered user audits.");
        }

        var dtoList = UserAuditDtoMapper.ToDtoList(userAudits);
        return TypedResults.Ok(dtoList);
    }

}
