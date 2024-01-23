using System.Security.Claims;
using Api.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Api.Authorization;

public class ClaimsTransformation(AppDbContext dbContext): IClaimsTransformation
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

        var ci = new ClaimsIdentity();
        ci.AddClaim(new Claim(ClaimTypes.Role, user.MemberType.ToString()));
        
        principal.AddIdentity(ci);

        return principal;
    }
}