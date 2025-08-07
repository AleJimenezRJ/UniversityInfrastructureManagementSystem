using Microsoft.AspNetCore.Authorization;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Authorization;

/// <summary>
/// A custom authorization handler that checks if the user has the required permissions.
/// </summary>
public class PermissionsHandler : AuthorizationHandler<PermissionsRequirement>
{
    /// <summary>
    /// Handles the authorization requirement by checking if the user has the required permissions.
    /// </summary>
    /// <param name="context"> The context for the authorization handler, which contains information about the user and the requirements.</param>
    /// <param name="requirement"> The requirement that needs to be fulfilled for authorization to succeed, in this case, the required permission.</param>
    /// <returns> A task that represents the asynchronous operation.</returns>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
    {
        var permissionClaim = context.User.FindFirst("extension_Permissions");

        if (permissionClaim is not null)
        {
            var permissions = permissionClaim.Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (permissions.Contains(requirement.RequiredPermission))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}