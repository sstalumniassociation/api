using System.Security.Claims;
using Api.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Api.Authorization;

public class ClaimsTransformation(AppDbContext dbContext) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var sub = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (sub is null)
        {
            return principal;
        }

        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.FirebaseId == sub.Value);
        if (user is null)
        {
            return principal;
        }

        var identity = principal.Identity as ClaimsIdentity;
        if (identity is null)
        {
            return principal;
        }

        var existingRoleClaim = identity.FindFirst(ClaimTypes.Role);
        if (existingRoleClaim is not null)
        {
            identity.RemoveClaim(existingRoleClaim);
        }

        identity.AddClaim(new Claim(ClaimTypes.Role, user.MemberType.ToString()));
        
        var existingNameIdentifierClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
        if (existingNameIdentifierClaim is not null)
        {
            identity.RemoveClaim(existingNameIdentifierClaim);
        }
        
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

        return principal;
    }
}