using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Endpoints.AccountManagement;

/// <summary>
/// Defines the endpoints related to user management.
/// </summary>
internal static class UserEndpoints
{
    /// <summary>
    /// Maps user management endpoints to the application's endpoint route builder.
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the user endpoints will be added.</param>
    /// <returns>The endpoint route builder with user endpoints mapped.</returns>
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        // References: https://learn.microsoft.com/es-es/dotnet/api/microsoft.aspnetcore.http.openapiroutehandlerbuilderextensions.produces?view=aspnetcore-9.0
        builder.MapGet("/userwithperson", GetAllUsersWithPersonHandler.HandleAsync)
            .WithName("getAllUsersWithPerson")
            .WithTags("UserWithPerson")
            .Produces<List<UserWithPersonDto>>(StatusCodes.Status200OK)
            .RequireAuthorization("View Users")
            .WithOpenApi();

        builder.MapGet("/user/list", GetAllUsersHandler.HandleAsync)
            .WithName("GetAllUsers")
            .WithTags("User Management")
            .RequireAuthorization("View Users")
            .WithOpenApi();

        builder.MapPost("/user/create", PostCreateUserHandler.HandleAsync)
            .WithName("PostCreateUser")
            .WithTags("User Management")
            .RequireAuthorization("Create Users")
            .WithOpenApi();

        builder.MapPost("/userwithperson/create", PostCreateUserWithPersonHandler.HandleAsync)
            .WithName("PostCreateUserWithPerson")
            .WithTags("UserWithPerson")
            .RequireAuthorization("ApiAccess")
            .WithOpenApi();

        builder.MapPut("/user/modify/{id}", PutModifyUserHandler.HandleAsync)
            .WithName("PutModifyUser")
            .WithTags("User Management")
            .RequireAuthorization("Edit Users")
            .WithOpenApi();

        // References: https://learn.microsoft.com/en-us/openapi/kiota/errors
        builder.MapPut("/userwithperson/modify", PutModifyUserWithPersonHandler.HandleAsync)
            .WithName("PutModifyUserWithPerson")
            .WithTags("UserWithPerson")
            .Produces<PutModifyUserWithPersonResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .RequireAuthorization("Edit Users")
            .WithOpenApi();

        builder.MapDelete("/user/delete/{id}", DeleteUserHandler.HandleAsync)
            .WithName("DeleteUser")
            .WithTags("User Management")
            .RequireAuthorization("Delete Users")
            .WithOpenApi();

        // References: https://learn.microsoft.com/es-es/aspnet/core/fundamentals/openapi/include-metadata?view=aspnetcore-9.0&tabs=minimal-apis
        builder.MapDelete("/userwithperson/delete", DeleteUserWithPersonHandler.HandleAsync)
            .WithName("DeleteUserWithPerson")
            .WithTags("UserWithPerson")
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("Delete Users")
            .WithOpenApi();
        
        builder.MapGet("/userwithperson/paginated", GetUserWithPersonPaginatedListHandler.HandleAsync)
            .WithName("GetUserWithPersonPaginatedList")
            .WithTags("UserWithPerson")
            .Produces<GetUserWithPersonPaginatedListResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .RequireAuthorization("View Users")
            .WithOpenApi();

        builder.MapGet("/user/id-by-email", GetUserIdByEmailHandler.HandleAsync)
            .WithName("GetUserIdByEmail")
            .WithTags("User Management")
            .Produces<int>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound)
            .RequireAuthorization("View Users")
            .WithOpenApi();

        builder.MapPost("/user/validate-register", PostValidateSignupHandler.HandleAsync)
            .WithName("ValidateUser")
            .WithOpenApi();

        return builder;
    }
}  