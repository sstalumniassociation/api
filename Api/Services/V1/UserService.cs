using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using User.V1;

namespace Api.Services.V1;

public class UserServiceV1(ILogger<UserServiceV1> logger): User.V1.User.UserBase
{
    public override Task<ListUsersResponse> ListUsers(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new ListUsersResponse
        {
            Users =
            {
            }
        });
    }
}