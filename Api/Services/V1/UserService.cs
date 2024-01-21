using Api.Authorization;
using Api.Context;
using Api.Extensions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using User.V1;
using Enum = System.Enum;
using UserMemberType = Api.Entities.UserMemberType;

namespace Api.Services.V1;

/// <inheritdoc />
public class UserServiceV1(ILogger<UserServiceV1> logger, AppDbContext dbContext) : UserService.UserServiceBase
{
    [AuthorizeMembers(UserMemberType.Exco)]
    public override async Task<ListUsersResponse> ListUsers(ListUsersRequest request, ServerCallContext context)
    {
        return new ListUsersResponse
        {
            Users =
            {
                dbContext.Users.Select(u => u.ToGrpcUser())
            },
        };
    }

    public override Task<User.V1.User> GetUser(GetUserRequest request, ServerCallContext context)
    {
        return base.GetUser(request, context);
    }

    public override Task<User.V1.User> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        return base.CreateUser(request, context);
    }

    public override Task<BatchCreateUsersResponse> BatchCreateUsers(BatchCreateUsersRequest request, ServerCallContext context)
    {
        return base.BatchCreateUsers(request, context);
    }

    public override Task<User.V1.User> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        return base.UpdateUser(request, context);
    }

    public override Task<Empty> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        return base.DeleteUser(request, context);
    }
}