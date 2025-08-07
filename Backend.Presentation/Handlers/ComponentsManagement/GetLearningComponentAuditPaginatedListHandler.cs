using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;


/// <summary>
/// Handler for retrieving a paginated list of learning component audit records.
/// Validates the input parameters for pagination, invokes the service to fetch the paginated data,
/// maps the domain entities to DTOs, and returns the result in a response object.
/// Returns a bad request with validation errors if the input parameters are invalid.
/// </summary>
public static class GetLearningComponentAuditPaginatedListHandler
{
    /// <summary>
    /// Handles the request to get a paginated list of learning component audit records.
    /// </summary>
    /// <param name="learningComponentAuditService">The service used to retrieve audit records.</param>
    /// <param name="pageSize">The number of records per page (default is 10, must be between 1 and 150).</param>
    /// <param name="pageNumber">The zero-based page number to retrieve (default is 0, must be non-negative).</param>
    /// <returns>
    /// An <see cref="Ok{T}"/> result containing a <see cref="GetLearningComponentAuditPaginatedListResponse"/> if successful,
    /// or a <see cref="BadRequest{T}"/> result containing a list of <see cref="ValidationError"/> if validation fails.
    /// </returns>
    public static async Task<Results<Ok<GetLearningComponentAuditPaginatedListResponse>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] ILearningComponentAuditServices learningComponentAuditService,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageIndex = 0)
    {
        var errors = new List<ValidationError>();

        if (pageSize < 1 || pageSize > 150)
            errors.Add(new ValidationError("PageSize", "Page size must be between 1 and 150."));

        if (pageIndex < 0)
            errors.Add(new ValidationError("PageIndex", "Page index must be a non-negative number."));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        var paginatedList = await learningComponentAuditService.GetPaginatedLearningComponentAuditAsync(pageSize, pageIndex);

        var dtoList = LearningComponentAuditDtoMapper.ToDtoList(paginatedList);

        var response = new GetLearningComponentAuditPaginatedListResponse(
            dtoList,
            paginatedList.PageSize,
            paginatedList.PageIndex,
            paginatedList.TotalCount,
            paginatedList.TotalPages
        );

        return TypedResults.Ok(response);
    }
}
