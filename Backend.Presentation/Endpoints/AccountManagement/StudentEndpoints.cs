using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.AccountManagement;

/// <summary>
/// Defines the endpoints related to student management.
/// </summary>
public static class StudentEndpoints
{
    /// <summary>
    /// Maps the endpoints related to Students into the application.
    /// </summary>
    /// <param name="builder">The route builder to add endpoints to.</param>
    /// <returns>The route builder with the mapped endpoints.</returns>
    public static IEndpointRouteBuilder MapStudentEndpoints(this IEndpointRouteBuilder builder)
    {

        builder.MapPost("/student/create", PostCreateStudentHandler.HandleAsync)
            .WithName("PostCreateStudent")
            .WithTags("Student Management")
            .RequireAuthorization("Create Users")
            .WithOpenApi();

       builder.MapGet("/student/list", GetAllStudentHandler.HandleAsync) 
            .WithName("GetAllStudents")
            .WithTags("Student Management")
            .RequireAuthorization("View Users")
            .WithOpenApi();

        return builder;
    }
}
