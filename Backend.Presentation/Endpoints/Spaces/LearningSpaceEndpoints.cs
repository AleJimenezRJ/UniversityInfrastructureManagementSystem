using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.Spaces;

/// <summary>
/// Provides extension methods to map endpoints related to learning spaces.
/// </summary>
public static class LearningSpaceEndpoints
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
    public static IEndpointRouteBuilder MapLearningSpaceEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/floors/{floorId}/learning-spaces/", PostLearningSpaceHandler.HandleAsync)
            .WithName("PostLearningSpace")
            .WithTags("Learning Spaces")
            .RequireAuthorization("Create Learning Space")
            .WithOpenApi();

        builder.MapGet("/learning-spaces/{learningSpaceId}", GetLearningSpaceHandler.HandleAsync)
            .WithName("GetLearningSpace")
            .WithTags("Learning Spaces")
            .RequireAuthorization("View Learning Space")
            .WithOpenApi();

        builder.MapPut("/learning-spaces/{learningSpaceId}", PutLearningSpaceHandler.HandleAsync)
            .WithName("UpdateLearningSpace")
            .WithTags("Learning Spaces")
            .RequireAuthorization("Edit Learning Space")
            .WithOpenApi();

        builder.MapGet("/floors/{floorId}/learning-spaces/", GetLearningSpaceListHandler.HandleAsync)
            .WithName("GetLearningSpaceList")
            .WithTags("Learning Spaces")
            .RequireAuthorization("List Learning Space")
            .WithOpenApi();

        builder.MapGet("/floors/{floorId}/learning-spaces/paginated", GetLearningSpacePaginatedListHandler.HandleAsync)
            .WithName("GetLearningSpacePaginatedList")
            .WithTags("Learning Spaces")
            .RequireAuthorization("List Learning Space")
            .WithOpenApi();

        builder.MapDelete("/learning-spaces/{learningSpaceId}", DeleteLearningSpaceHandler.HandleAsync)
            .WithName("DeleteLearningSpaceHandler")
            .WithTags("Learning Spaces")
            .RequireAuthorization("Delete Learning Space")
            .WithOpenApi();

        return builder;
    }
}
