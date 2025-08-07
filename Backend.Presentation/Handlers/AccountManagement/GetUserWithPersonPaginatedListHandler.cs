using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Provides a handler for retrieving a paginated list of users with person information.
/// </summary>
public static class GetUserWithPersonPaginatedListHandler
{
    /// <summary>
    /// Handles the request to retrieve users with pagination and optional search.
    /// </summary>
    /// <param name="userService">The service used to interact with user data.</param>
    /// <param name="pageSize">The page size (default is 10).</param>
    /// <param name="pageNumber">The index of the page (default is 0).</param>
    /// <param name="searchText">Optional search text to filter users by name, email, username, etc.</param>
    /// <returns>
    /// An <see cref="IResult"/> containing:
    /// - An <see cref="Ok{T}"/> result with a <see cref="GetUserWithPersonPaginatedListResponse"/> if users are found.
    /// - A <see cref="BadRequest{T}"/> result with validation errors.
    /// </returns>
    public static async Task<Results<Ok<GetUserWithPersonPaginatedListResponse>, BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IUserWithPersonService userService,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 0,
        [FromQuery] string? searchText = "")
    {
        var errors = new List<ValidationError>();

        if (pageSize < 1 || pageSize > 20)
            errors.Add(new ValidationError("PageSize", "Page size must be between 1 and 130."));

        if (pageNumber < 0)
            errors.Add(new ValidationError("PageIndex", "Page index must be a non-negative number."));

        if (errors.Count > 0)
            return TypedResults.BadRequest(errors);

        var paginatedList = await userService.GetPaginatedUsersAsync(pageSize, pageNumber, searchText ?? string.Empty);

        IReadOnlyCollection<UserWithPersonPaginatedDto> dtoList = paginatedList
            .Select(UserWithPersonDtoMapper.ToPaginatedDto)
            .ToList();

        var response = new GetUserWithPersonPaginatedListResponse(
            dtoList,
            paginatedList.PageSize,
            paginatedList.PageIndex,
            paginatedList.TotalCount,
            paginatedList.TotalPages);

        return TypedResults.Ok(response);
    }
}
