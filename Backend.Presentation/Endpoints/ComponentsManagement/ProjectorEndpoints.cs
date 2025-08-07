using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.ComponentsManagement;

/// <summary>
/// Provides extension methods to map endpoints related to the projector module.
/// </summary>
public static class ProjectorEndpoints
{
    /// <summary>
    /// Maps the endpoints for the projector module.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapProjectorEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/learning-component/projector", GetProjectorHandler.HandleAsync)
            .WithName("GetProjector")
            .WithTags("Learning Components Management")
            .RequireAuthorization("List Components")
            .WithOpenApi();

        builder.MapPost("/learning-spaces/{learningSpaceId}/learning-component/projector", PostProjectorHandler.HandleAsync)
            .WithName("PostProjector")
            .WithTags("Learning Components Management")
            .RequireAuthorization("Create Components")
            .WithOpenApi();

        builder.MapPut("/learning-spaces/{learningSpaceId}/learning-component/projector/{learningComponentId}", PutProjectorHandler.HandleAsync)
            .WithName("PutProjector")
            .WithTags("Learning Components Management")
            .RequireAuthorization("Edit Components")
            .WithOpenApi();

        return builder;
    }
}