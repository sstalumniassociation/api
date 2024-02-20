using Api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.OwnerOrAdmin;

/// <inheritdoc cref="OwnerOrAdminRequirement"/>
public class OwnerOrAdminHandler(IServiceProvider serviceProvider)
    : AuthorizationHandler<OwnerOrAdminRequirement, string>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnerOrAdminRequirement requirement,
        string? userId)
    {
        // Manually get service to prevent circular dependencies
        await using var scope = serviceProvider.CreateAsyncScope();
        var authorizationService = scope.ServiceProvider.GetRequiredService<IAuthorizationService>();

        // If a user is admin, grant permission to access user data
        var userIsAdmin = await authorizationService.AuthorizeAsync(context.User, Policies.Admin);
        if (userIsAdmin.Succeeded)
        {
            context.Succeed(requirement);
            return;
        }

        // If a user is reading their own data, grant permission
        if (requirement.Name == OwnerOrAdminOperations.Read.Name &&
            userId == context.User.Claims.GetNameIdentifier())
        {
            context.Succeed(requirement);
        }
    }
}