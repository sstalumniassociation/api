using Microsoft.AspNetCore.Authorization;

namespace SSTAlumniAssociation.Api.Authorization;

/// <summary>
/// Syntax sugar for [Authorize(Policy = Policies.Admin)]
/// </summary>
public class AuthorizeAdminAttribute : AuthorizeAttribute
{
    public AuthorizeAdminAttribute()
    {
        Policy = Policies.Admin;
    }
}