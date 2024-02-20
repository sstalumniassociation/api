using Api.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.Member;

/// <summary>
/// User must be a <see cref="Entities.Member"/> with a member type that is not <see cref="Membership.Revoked"/> .
/// </summary>
public class MemberRequirement : IAuthorizationRequirement;