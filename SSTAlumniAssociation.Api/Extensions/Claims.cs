using System.Security.Claims;

namespace SSTAlumniAssociation.Api.Extensions;

public static class Claims
{
    /// <summary>
    /// Retrieves the name identifier (represents <see cref="Entities.User.Id"/> as modified by <see cref="Authorization.ClaimsTransformation"/>)
    /// </summary>
    /// <param name="claims"></param>
    /// <returns>ID of user</returns>
    public static string GetNameIdentifier(this IEnumerable<Claim> claims)
    {
        return claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
    }

    /// <summary>
    /// Retrieves the name identifier (represents <see cref="Entities.User.Id"/> as modified by <see cref="Authorization.ClaimsTransformation"/>)
    /// </summary>
    /// <param name="claims"></param>
    /// <returns>ID of user</returns>
    public static Guid GetNameIdentifierGuid(this IEnumerable<Claim> claims)
    {
        return Guid.Parse(claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}