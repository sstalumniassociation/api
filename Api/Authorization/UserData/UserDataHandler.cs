using System.Security.Claims;
using Api.Authorization.Admin;
using Api.Entities;
using Api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.UserData;

/// <inheritdoc cref="UserDataRequirement"/>
public class UserDataHandler(IServiceProvider serviceProvider)
    : AuthorizationHandler<UserDataRequirement, string>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserDataRequirement requirement,
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
        if (requirement.Name == UserDataOperations.Read.Name &&
            userId == context.User.Claims.GetNameIdentifier())
        {
            context.Succeed(requirement);
        }
    }
}