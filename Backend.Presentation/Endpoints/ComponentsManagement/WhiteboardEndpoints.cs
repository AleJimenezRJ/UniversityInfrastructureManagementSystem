using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.ComponentsManagement;

/// <summary>
/// Provides extension methods to map endpoints related to the whiteboard module.
/// </summary>
public static class WhiteboardEndpoints
{
    /// <summary>
    /// Maps the endpoints for the whiteboard module.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapWhiteboardEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/learning-component/whiteboard", GetWhiteboardHandler.HandleAsync)
            .WithName("GetWhiteboard")
            .WithTags("Learning Components Management")
            .RequireAuthorization("List Components")
            .WithOpenApi();

        builder.MapPost("/learning-spaces/{learningSpaceId}/learning-component/whiteboard", PostWhiteboardHandler.HandleAsync)
            .WithName("PostWhiteboard")
            .WithTags("Learning Components Management")
            .RequireAuthorization("Create Components")
            .WithOpenApi();

        builder.MapPut("/learning-spaces/{learningSpaceId}/learning-component/whiteboard/{learningComponentId}", PutWhiteboardHandler.HandleAsync)
            .WithName("PutWhiteboard")
            .WithTags("Learning Components Management")
            .RequireAuthorization("Edit Components")
            .WithOpenApi();

        return builder;
    }
}
