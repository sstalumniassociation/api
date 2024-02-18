using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization;

/// <summary>
/// Syntax sugar for [Authorize(Policy = Policies.Member)]
/// </summary>
public class AuthorizeMemberAttribute: AuthorizeAttribute
{
    public AuthorizeMemberAttribute()
    {
        Policy = Policies.Member;
    }
}