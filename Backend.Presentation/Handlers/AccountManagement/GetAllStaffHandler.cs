using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handler for retrieving all registered staff.
/// </summary>
public static class GetAllStaffHandler
{
    /// <summary>
    /// Handles the HTTP request to retrieve all registered staff members.
    /// </summary>
    /// <param name="staffService">The service used to access staff data.</param>
    /// <returns>
    /// A <see cref="Results{T1, T2}"/> containing either an <see cref="Ok{T}"/> result with the list of staff members
    /// or an <see cref="Ok{T}"/> result with a message indicating no staff are registered.
    /// </returns>
    public static async Task<Results<
        Ok<GetAllStaffResponse>,
        NotFound<string>,
        Conflict<string>>> HandleAsync(
            [FromServices] IStaffService staffService)
    {
        try
        {
            var staffList = await staffService.ListStaffAsync();

            if (staffList == null || staffList.Count == 0)
                return TypedResults.NotFound("There are no registered staff members.");

            var dtoList = StaffDtoMapper.ToDtoList(staffList);

            var response = new GetAllStaffResponse
            {
                Staff = dtoList
            };

            return TypedResults.Ok(response);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
