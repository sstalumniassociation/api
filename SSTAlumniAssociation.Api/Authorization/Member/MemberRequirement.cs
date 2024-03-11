using Microsoft.AspNetCore.Authorization;
using SSTAlumniAssociation.Api.Entities;

namespace SSTAlumniAssociation.Api.Authorization.Member;

/// <summary>
/// User must be a <see cref="Entities.Member"/> with a member type that is not <see cref="Membership.Revoked"/> .
/// </summary>
public class MemberRequirement : IAuthorizationRequirement;