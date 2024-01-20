using Auth.V1;
using Grpc.Core;

namespace Api.Services.V1;

public class AuthServiceV1: AuthService.AuthServiceBase
{
    public override Task<VerifyUserResponse> VerifyUser(VerifyUserRequest request, ServerCallContext context)
    {
        return base.VerifyUser(request, context);
    }
}
