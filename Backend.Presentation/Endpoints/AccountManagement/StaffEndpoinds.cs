using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.AccountManagement;

/// <summary>
/// Defines the endpoints related to staff management.
/// </summary>
public static class StaffEndpoints
{
    /// <summary>
    /// Maps the endpoints related to Staffs into the application.
    /// </summary>
    /// <param name="builder">The route builder to add endpoints to.</param>
    /// <returns>The route builder with the mapped endpoints.</returns>
    public static IEndpointRouteBuilder MapStaffEndpoints(this IEndpointRouteBuilder builder)
    {

        builder.MapPost("/staff/create", PostCreateStaffHandler.HandleAsync)
            .WithName("PostCreateStaff")
            .WithTags("Staff Management")
            .RequireAuthorization("Create Users")
            .WithOpenApi();

        builder.MapGet("/staff/list", GetAllStaffHandler.HandleAsync)
            .WithName("GetAllStaff")
            .WithTags("Staff Management")
            .RequireAuthorization("View Users")
            .WithOpenApi();

        return builder;
    }
}
