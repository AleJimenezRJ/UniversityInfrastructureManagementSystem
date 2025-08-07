using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Provides a handler for retrieving a paginated list of user audit records.
/// </summary>
public static class GetUserAuditPaginatedListHandler
{
    /// <summary>
    /// Handles the request to retrieve paginated user audit logs.
    /// </summary>
    /// <param name="auditService">The service used to retrieve audit data.</param>
    /// <param name="pageSize">The number of records per page.</param>
    /// <param name="pageNumber">The current page index.</param>
    /// <returns>
    /// An <see cref="IResult"/> containing:
    /// - An <see cref="Ok{T}"/> with <see cref="GetUserAuditPaginatedListResponse"/> if successful.
    /// - A <see cref="BadRequest{T}"/> if validation fails.
    /// </returns>
    public static async Task<Results<Ok<GetUserAuditPaginatedListResponse>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IUserAuditService auditService,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 0)
    {
        var errors = new List<ValidationError>();

        if (pageSize < 1 || pageSize > 150)
            errors.Add(new ValidationError("PageSize", "Page size must be between 1 and 150."));

        if (pageNumber < 0)
            errors.Add(new ValidationError("PageIndex", "Page index must be a non-negative number."));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        var paginatedList = await auditService.GetPaginatedUserAuditAsync(pageSize, pageNumber);

        var dtoList = UserAuditDtoMapper.ToDtoList(paginatedList);

        var response = new GetUserAuditPaginatedListResponse(
            dtoList,
            paginatedList.PageSize,
            paginatedList.PageIndex,
            paginatedList.TotalCount,
            paginatedList.TotalPages
        );

        return TypedResults.Ok(response);
    }
}
