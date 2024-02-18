using Api.Authorization;
using Api.Authorization.UserData;
using Api.Context;
using Api.Extensions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using User.V1;

namespace Api.Services.V1;

/// <inheritdoc />
public class UserServiceV1(
    ILogger<UserServiceV1> logger,
    IAuthorizationService authorizationService,
    AppDbContext dbContext) : UserService.UserServiceBase
{
    // [Authorize]
    // public override async Task<User.V1.User> BindUser(BindUserRequest request, ServerCallContext context)
    // {
    //     var email = context.GetHttpContext().User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email);
    //     if (email is null)
    //     {
    //         throw new RpcException(new Status(StatusCode.PermissionDenied, "Weird email."));
    //     }
    //
    //     var firebaseId = context.GetHttpContext().User.Claims.SingleOrDefault(c => c.Type == "user_id");
    //     if (firebaseId is null)
    //     {
    //         throw new RpcException(new Status(StatusCode.Internal, "Firebase ID does not exist in token."));
    //     }
    //
    //     var user = await dbContext.Users.FindAsync(request.Id);
    //     if (user is null || user.Email != email.Value)
    //     {
    //         throw new RpcException(new Status(StatusCode.PermissionDenied, "Mailbox issue, wrong email."));
    //     }
    //
    //     if (!string.IsNullOrWhiteSpace(user.FirebaseId))
    //     {
    //         throw new RpcException(new Status(StatusCode.FailedPrecondition, "Firebase ID already set for user."));
    //     }
    //
    //     user.FirebaseId = firebaseId.Value;
    //
    //     await dbContext.SaveChangesAsync();
    //
    //     return user.ToGrpcUser();
    // }
    //
    // [AuthorizeMembers(UserMemberType.Exco)]
    // public override async Task<User.V1.User> CreateUser(CreateUserRequest request, ServerCallContext context)
    // {
    //     var user = await dbContext.Users.AddAsync(request.User.ToUser());
    //     await dbContext.SaveChangesAsync();
    //     return user.Entity.ToGrpcUser();
    // }
    //
    // [AuthorizeMembers(UserMemberType.Exco)]
    // public override async Task<BatchCreateUsersResponse> BatchCreateUsers(BatchCreateUsersRequest request,
    //     ServerCallContext context)
    // {
    //     await using var txn = await dbContext.Database.BeginTransactionAsync();
    //
    //     var users = new List<Entities.User>();
    //     foreach (var r in request.Requests)
    //     {
    //         var us = r.User.ToUser();
    //         var u = await dbContext.Users.FindAsync(us.Id);
    //         if (u is null)
    //         {
    //             users.Add(us);
    //             await dbContext.Users.AddAsync(us);
    //         }
    //         else
    //         {
    //             u.MemberId = us.MemberId;
    //         }
    //     }
    //
    //     await txn.CommitAsync();
    //
    //     await dbContext.SaveChangesAsync();
    //
    //     return new BatchCreateUsersResponse
    //     {
    //         Users = { users.Select(u => u.ToGrpcUser()) }
    //     };
    // }
    //
    // [AuthorizeMembers(UserMemberType.Exco)]
    // public override async Task<User.V1.User> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    // {
    //     var user = await dbContext.Users.SingleAsync(c => c.Id.ToString() == request.User.Id);
    //     if (user is null)
    //     {
    //         throw new RpcException(new Status(StatusCode.NotFound, "User disappeared into the abyss."));
    //     }
    //
    //     var diff = new User.V1.User();
    //     request.UpdateMask.Merge(request.User, diff);
    //
    //     if (request.UpdateMask.Paths.Contains("memberId"))
    //     {
    //         user.MemberId = diff.MemberId;
    //     }
    //
    //     if (request.UpdateMask.Paths.Contains("name"))
    //     {
    //         user.Name = diff.Name;
    //     }
    //
    //     if (request.UpdateMask.Paths.Contains("email"))
    //     {
    //         user.Email = diff.Email;
    //     }
    //
    //     if (request.UpdateMask.Paths.Contains("graduationYear"))
    //     {
    //         user.GraduationYear = diff.GraduationYear;
    //     }
    //
    //     if (request.UpdateMask.Paths.Contains("memberType"))
    //     {
    //         user.MemberType = diff.MemberType.ToUserMemberType();
    //     }
    //
    //     await dbContext.SaveChangesAsync();
    //
    //     return user.ToGrpcUser();
    // }
    //
    // [AuthorizeMembers(UserMemberType.Exco)]
    // public override async Task<Empty> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    // {
    //     var id = context.GetHttpContext().User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier);
    //     if (request.Id == id.Value)
    //     {
    //         throw new RpcException(new Status(StatusCode.FailedPrecondition, "Cannot suicide."));
    //     }
    //
    //     var user = await dbContext.Users.SingleAsync(c => c.Id.ToString() == request.Id);
    //     if (user is null)
    //     {
    //         throw new RpcException(new Status(StatusCode.NotFound, "User disappeared into the abyss."));
    //     }
    //
    //     dbContext.Users.Remove(user);
    //
    //     return new Empty();
    // }

    [AuthorizeAdmin]
    public override async Task<ListUsersResponse> ListUsers(ListUsersRequest request, ServerCallContext context)
    {
        return new ListUsersResponse
        {
            Users =
            {
                dbContext.Users.Select(u => u.ToGrpcUser())
            }
        };
    }

    [Authorize]
    public override async Task<User.V1.User> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var authorized =
            await authorizationService.AuthorizeAsync(context.GetHttpContext().User, request.Id,
                UserDataOperations.Read);
        if (!authorized.Succeeded)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied,
                "User is not authorized to peak at others."));
        }

        var user = await dbContext.Users.FindAsync(Guid.Parse(request.Id));
        if (user is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User does not exist."));
        }

        return user.ToGrpcUser();
    }

    public override async Task<User.V1.User> BindUser(BindUserRequest request, ServerCallContext context)
    {
        return await base.BindUser(request, context);
    }

    public override async Task<User.V1.User> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        return await base.CreateUser(request, context);
    }

    public override async Task<BatchCreateUsersResponse> BatchCreateUsers(BatchCreateUsersRequest request,
        ServerCallContext context)
    {
        return await base.BatchCreateUsers(request, context);
    }

    public override async Task<User.V1.User> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        return await base.UpdateUser(request, context);
    }

    public override async Task<Empty> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        return await base.DeleteUser(request, context);
    }
}