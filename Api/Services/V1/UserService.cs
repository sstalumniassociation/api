using System.Security.Claims;
using Api.Authorization;
using Api.Context;
using Api.Extensions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using User.V1;
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

    [Authorize]
    public override async Task<User.V1.User> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var sub = context.GetHttpContext().User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (sub is null || sub.Value != request.FirebaseId)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied,
                "User is not authorized to peak at others."));
        }

        var user = await dbContext.Users.SingleAsync(u => u.FirebaseId == request.FirebaseId);

        return user.ToGrpcUser();
    }

    [Authorize]
    public override async Task<User.V1.User> BindUser(BindUserRequest request, ServerCallContext context)
    {
        var email = context.GetHttpContext().User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email);
        if (email is null)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Weird email."));
        }

        var user = await dbContext.Users.FindAsync(request.Id);
        if (user is null || user.Email != email.Value)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Mailbox issue, wrong email."));
        }

        user.FirebaseId = request.FirebaseId;

        await dbContext.SaveChangesAsync();

        return user.ToGrpcUser();
    }

    [AuthorizeMembers(UserMemberType.Exco)]
    public override async Task<User.V1.User> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var user = await dbContext.Users.AddAsync(request.User.ToUser());
        await dbContext.SaveChangesAsync();
        return user.Entity.ToGrpcUser();
    }

    public override async Task<BatchCreateUsersResponse> BatchCreateUsers(BatchCreateUsersRequest request,
        ServerCallContext context)
    {
        await using var txn = await dbContext.Database.BeginTransactionAsync();

        var users = new List<User.V1.User>();
        foreach (var r in request.Requests)
        {
            var u = await dbContext.Users.AddAsync(r.User.ToUser());
            users.Add(u.Entity.ToGrpcUser());
        }

        await dbContext.SaveChangesAsync();

        return new BatchCreateUsersResponse
        {
            Users = { users }
        };
    }

    [AuthorizeMembers(UserMemberType.Exco)]
    public override async Task<User.V1.User> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        var user = await dbContext.Users.FirstAsync(c => c.Id.ToString() == request.User.Id);
        if (user is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User disappeared into the abyss."));
        }

        var diff = new User.V1.User();
        request.UpdateMask.Merge(request.User, diff);

        if (!string.IsNullOrWhiteSpace(diff.MemberId))
        {
            user.MemberId = diff.MemberId;
        }
        
        return diff;
    }

    public override Task<Empty> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        return base.DeleteUser(request, context);
    }
}