using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization;

/// <summary>
/// Syntax sugar for [Authorize(Policy = Policies.Admin)]
/// </summary>
public class AuthorizeAdminAttribute: AuthorizeAttribute
{
    public AuthorizeAdminAttribute()
    {
        Policy = Policies.Admin;
    }
}