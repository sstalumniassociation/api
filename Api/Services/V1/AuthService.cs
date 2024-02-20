using Api.Context;
using Api.Extensions;
using Auth.V1;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.V1;

[Authorize]
public class AuthServiceV1(AppDbContext dbContext) : AuthService.AuthServiceBase
{
    [AllowAnonymous]
    public override async Task<VerifyUserResponse> VerifyUser(VerifyUserRequest request, ServerCallContext context)
    {
        var user = await dbContext.Users.Where(u => u.Email == request.Email).SingleOrDefaultAsync();
        if (user is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User does not exist."));
        }

        return new VerifyUserResponse
        {
            Id = user.Id.ToString(),
            Linked = !string.IsNullOrWhiteSpace(user.FirebaseId)
        };
    }

    public override async Task<WhoAmIResponse> WhoAmI(WhoAmIRequest request, ServerCallContext context)
    {
        var id = context.GetHttpContext().User.Claims.GetNameIdentifierGuid();

        var user = await dbContext.Users.FindAsync(id);
        if (user is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User does not exist."));
        }

        return new WhoAmIResponse
        {
            Id = user.Id.ToString()
        };
    }
}