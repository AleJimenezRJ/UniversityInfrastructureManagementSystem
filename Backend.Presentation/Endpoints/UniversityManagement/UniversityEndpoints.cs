using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.UniversityManagement;

/// <summary>
/// Provides extension methods to map endpoints related to the university module.
/// </summary>
public static class UniversityEndpoints
{
    public static IEndpointRouteBuilder MapUniversityEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/add-university", PostUniversityHandler.HandleAsync)
            .WithName("AddUniversity")
            .WithTags("University")
            .RequireAuthorization("Create Universities")
            .WithOpenApi();

        builder.MapGet("/list-university", GetUniversityHandler.HandleAsync)
             .WithName("GetUniversity")
             .WithTags("University")
             .RequireAuthorization("List Universities")
             .WithOpenApi();

        builder.MapGet("/list-university/{universityName}", GetUniversityByNameHandler.HandleAsync)
            .WithName("GetUniversityByName")
            .WithTags("University")
            .RequireAuthorization("View Specific University")
            .WithOpenApi();

        builder.MapDelete("/university/{universityName}", DeleteUniversityHandler.HandleAsync)
            .WithName("DeleteUniversity")
            .WithTags("University")
            .RequireAuthorization("Delete Universities")
            .WithOpenApi();

        return builder;
    }
}
