using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.UniversityManagement;

/// <summary>
/// Provides extension methods to map endpoints related to the campus module.
/// </summary>
public static class CampusEndpoints
{
    public static IEndpointRouteBuilder MapCampusEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/add-campus", PostCampusHandler.HandleAsync)
            .WithName("AddCampus")
            .WithTags("Campus")
            .RequireAuthorization("Create Campus")
            .WithOpenApi();

        builder.MapGet("/list-campus", GetCampusHandler.HandleAsync)
            .WithName("GetCampus")
            .WithTags("Campus")
            .RequireAuthorization("List Campus")
            .WithOpenApi();

        builder.MapGet("/list-campus/{campusName}", GetCampusByNameHandler.HandleAsync)
            .WithName("GetCampusByName")
            .WithTags("Campus")
            .RequireAuthorization("View Specific Campus")
            .WithOpenApi();

        builder.MapDelete("/delete-campus/{campusName}", DeleteCampusHandler.HandleAsync)
            .WithName("DeleteCampus")
            .WithTags("Campus")
            .RequireAuthorization("Delete Campus")
            .WithOpenApi();

        return builder;
    }
}