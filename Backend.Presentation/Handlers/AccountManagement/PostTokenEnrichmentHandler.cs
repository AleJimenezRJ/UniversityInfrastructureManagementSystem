using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handles the enrichment of user tokens with additional information such as roles and permissions.
/// </summary>
public static class PostTokenEnrichmentHandler
{
    /// <summary>
    /// Handles the enrichment of user tokens with roles and permissions based on the provided email address.
    /// </summary>
    /// <param name="request"> The request containing the email address of the user whose token needs to be enriched.</param>
    /// <param name="userRepo"> The repository for accessing user information.</param>
    /// <param name="userRoleRepo"> The repository for accessing user roles.</param>
    /// <param name="permissionRepo"> The repository for accessing role permissions.</param>
    /// <returns></returns>
    public static async Task<IResult> HandleAsync(
        [FromBody] PostTokenEnrichmentRequest? request,
        [FromServices] IUserWithPersonRepository userRepo,
        [FromServices] IUserRoleRepository userRoleRepo,
        [FromServices] IRolePermissionRepository permissionRepo)
    {

        if (string.IsNullOrWhiteSpace(request!.Email))
        {
            return Results.Json(new PostTokenEnrichmentResponse(Action: "ValidationError"));
        }

        var user = await userRepo.GetUserIdByEmailAsync(request.Email);

        if (user is null)
        {
            return Results.Json(new PostTokenEnrichmentResponse());
        }

        var roles = await userRoleRepo.GetRolesByUserIdAsync((int)user);

        var roleNames = roles!.Select(r => r.Name).Distinct().ToList();

        if (!roleNames.Any())
        {

            return Results.Json(new PostTokenEnrichmentResponse());
        }

        var allPermissions = new HashSet<string>();
        foreach (var role in roles!)
        {
            var perms = await permissionRepo.ViewPermissionsByRoleIdAsync(role.Id);
            foreach (var p in perms!)
                allPermissions.Add(p.Description);
        }

        var response = new PostTokenEnrichmentResponse(
        Extension_Roles: string.Join(",", roleNames),
        Extension_Permissions: string.Join(",", allPermissions)
        );

        return Results.Json(response);
    }
}