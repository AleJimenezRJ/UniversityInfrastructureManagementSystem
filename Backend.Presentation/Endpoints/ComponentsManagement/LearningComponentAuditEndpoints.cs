using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.ComponentsManagement;

/// <summary>
/// Provides extension methods to map endpoints related to learning component audit logs.
/// </summary>
internal static class LearningComponentAuditEndpoints
{
    /// <summary>
    /// Maps endpoints for retrieving learning component audit logs and paginated audit logs.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    /// <returns>The updated endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapLearningComponentAuditEndpoints(this IEndpointRouteBuilder builder)
    {
        // Maps an endpoint to retrieve all learning component audit logs.
        builder.MapGet("/learning-component-logs", GetAllLearningComponentAuditHandler.HandleAsync)
            .WithName("GetAllLearningComponentAudit")
            .WithTags("LearningComponentAuditLogs")
            .RequireAuthorization("View Audit")
            .Produces<List<LearningComponentAuditDto>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .WithOpenApi();

        // Maps an endpoint to retrieve paginated learning component audit logs.
        builder.MapGet("/learning-component-logs/paginated", GetLearningComponentAuditPaginatedListHandler.HandleAsync)
            .WithName("GetLearningComponentAuditPaginatedList")
            .WithTags("LearningComponentAuditLogs")
            .RequireAuthorization("View Audit")
            .Produces<GetLearningComponentAuditPaginatedListResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .WithOpenApi();

        return builder;
    }
}
