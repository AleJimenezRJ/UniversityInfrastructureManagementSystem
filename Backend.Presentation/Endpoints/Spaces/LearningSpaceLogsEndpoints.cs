using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.Spaces;

/// <summary>
/// Provides extension methods for mapping learning space log-related endpoints to an <see cref="IEndpointRouteBuilder"/>.
/// </summary>
/// <remarks>
/// This class contains methods to register API endpoints for managing learning space logs,
/// such as retrieving log information. The endpoints are configured with route patterns, authorization policies,
/// and OpenAPI documentation tags.
/// </remarks>
public static class LearningSpaceLogsEndpoints
{
    /// <summary>
    /// Maps the endpoints related to learning space logs to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <remarks>
    /// This method registers the following endpoint:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       A GET endpoint at <c>/list-learning-space-logs</c> that retrieves log information
    ///       for learning spaces. This endpoint requires the "ApiAccess" authorization policy
    ///       and is tagged with "LearningSpaceLogs".
    ///     </description>
    ///   </item>
    /// </list>
    /// </remarks>
    /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to which the learning space log endpoints will be added.</param>
    /// <returns>The <see cref="IEndpointRouteBuilder"/> with the learning space log endpoints mapped.</returns>
    public static IEndpointRouteBuilder MapLearningSpaceLogsEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/list-learning-space-logs", GetLearningSpaceLogHandler.HandleAsync)
            .WithName("GetLearningSpaceLogs")
            .WithTags("LearningSpaceLogs")
            .RequireAuthorization("View Audit")
            .WithOpenApi();

        return builder;
    }
}
