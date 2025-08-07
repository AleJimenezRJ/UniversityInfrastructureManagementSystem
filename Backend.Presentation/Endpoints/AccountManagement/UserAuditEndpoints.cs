using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.AccountManagement;

/// <summary>
/// Endpoints for user audit logs.
/// </summary>
internal static class UserAuditEndpoints
{
    /// <summary>
    /// Maps the user audit endpoints to the specified route builder.
    /// </summary>
    /// <param name="builder"> Builder to map the endpoints to.</param>
    /// <returns>
    /// Returns the updated <see cref="IEndpointRouteBuilder"/> with the user audit endpoints mapped.
    /// </returns>
    public static IEndpointRouteBuilder MapUserAuditEndpoints(this IEndpointRouteBuilder builder)
    {

        builder.MapGet("/auditlogs", GetAllUserAuditHandler.HandleAsync)
            .WithName("GetAllUserAudit")
            .WithTags("AuditLogs")
            .Produces<List<UserAuditDto>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .RequireAuthorization("View Users")
            .WithOpenApi();

        builder.MapGet("/auditlogs/paginated", GetUserAuditPaginatedListHandler.HandleAsync)
            .WithName("GetUserAuditPaginated")
            .WithTags("AuditLogs")
            .Produces<GetUserAuditPaginatedListResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .RequireAuthorization("View Users")
            .WithOpenApi();

        return builder;
    }
}