using Microsoft.AspNetCore.Authorization;

namespace SSTAlumniAssociation.Api.Authorization;

/// <summary>
/// Syntax sugar for [Authorize(Policy = Policies.Member)]
/// </summary>
public class AuthorizeMemberAttribute : AuthorizeAttribute
{
    public AuthorizeMemberAttribute()
    {
        Policy = Policies.Member;
    }
}