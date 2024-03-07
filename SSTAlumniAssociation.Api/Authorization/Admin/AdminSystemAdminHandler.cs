using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SSTAlumniAssociation.Api.Entities;

namespace SSTAlumniAssociation.Api.Authorization.Admin;

/// <inheritdoc cref="AdminRequirement"/>
public class AdminSystemAdminHandler : AuthorizationHandler<AdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
    {
        var role = context.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role);

        if (role is not null && role.Value == nameof(SystemAdmin))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}