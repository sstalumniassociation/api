using Api.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization;

public class AuthorizeMembersAttribute: AuthorizeAttribute
{
    public AuthorizeMembersAttribute(params UserMemberType[] memberTypes)
    {
        Roles = string.Join(",", memberTypes);
    }
}