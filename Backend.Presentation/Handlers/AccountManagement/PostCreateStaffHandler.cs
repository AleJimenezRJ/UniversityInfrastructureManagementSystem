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
/// Handles the creation of a new staff.
/// </summary>
public static class PostCreateStaffHandler
{

    /// <summary>
    /// Handles the HTTP POST request to create a new Staff.
    /// </summary>
    /// <param name="StaffService"> The service used to manage Staff related operations.</param>
    /// <param name="request"> The request containing the details of the Staff to be created.</param>
    /// <returns></returns>
    public static async Task<Results<
        Ok<PostCreateStaffResponse>,
        NotFound<string>,
        Conflict<string>,
        BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IStaffService StaffService,
        [FromBody] PostCreateStaffRequest request)
    {
        Staff? staffEntity = null;
        List<ValidationError> errorMessages = [];

        try
        {
            staffEntity = StaffDtoMapper.ToEntity(request.Staff);
        }
        catch (ValidationException exception)
        {
            errorMessages.Add(new ValidationError("Staff", exception.Message));
        }

        if (errorMessages.Count > 0 || staffEntity == null)
        {
            return TypedResults.BadRequest(errorMessages);
        }

        try
        {
            var additionSucceeded = await StaffService.CreateStaffAsync(staffEntity);

            if (!additionSucceeded)
            {
                return TypedResults.Conflict("A staff with this identity or institutional email already exists.");
            }

            return TypedResults.Ok(new PostCreateStaffResponse(request.Staff, "Successful creation."));
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}


