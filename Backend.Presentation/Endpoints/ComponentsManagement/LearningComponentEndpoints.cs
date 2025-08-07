using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.ComponentsManagement;

/// <summary>
/// Provides endpoint mappings related to learning components.
/// </summary>
public static class LearningComponentEndpoints
{
    /// <summary>
    /// Maps the endpoints for learning space operations.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IEndpointRouteBuilder"/> used to define the routes.
    /// </param>
    /// <returns>
    /// The same <see cref="IEndpointRouteBuilder"/> instance to allow method chaining.
    /// </returns>
    public static IEndpointRouteBuilder MapLearningComponentEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/learning-component", GetLearningComponentHandler.HandleAsync)
            .WithName("GetLearningComponent")
            .WithTags("Learning Components Management")
            .RequireAuthorization("List Components")
            .WithOpenApi();
        
        builder.MapGet("/learning-component/{learningComponentId}", GetSingleLearningComponentByIdHandler.HandleAsync)
            .WithName("GetSingleLearningComponentById")
            .WithTags("Learning Components Management")
            .RequireAuthorization("View Specific Component")
            .WithOpenApi();

        builder.MapGet("/learning-spaces/{learningSpaceId}/learning-component", GetLearningComponentsByIdHandler.HandleAsync)
            .WithName("GetLearningComponentsById")
            .WithTags("Learning Components Management")
            .RequireAuthorization("List Components")
            .WithOpenApi();
        
        builder.MapDelete("/learning-component/{learningComponentId}", DeleteLearningComponentHandler.HandleAsync)
            .WithName("DeleteLearningComponent")
            .WithTags("Learning Components Management")
            .RequireAuthorization("Delete Components")
            .WithOpenApi();

        return builder;
    }
}