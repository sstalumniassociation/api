using Api.Services.V1;

namespace Api.Extensions;

public static class UserExtensions
{
    public static Entities.UserMemberType ToUserMemberType(this User.V1.UserMemberType memberType)
    {
        return Enum.Parse<Entities.UserMemberType>(memberType.ToString());
    }
    
    public static User.V1.UserMemberType ToGrpcUserMemberType(this Entities.UserMemberType memberType)
    {
        return Enum.Parse<User.V1.UserMemberType>(memberType.ToString());
    }
    
    public static Entities.User ToUser(this User.V1.User user)
    {
        return new Entities.User
        {
            Id = Guid.Parse(user.Id),
            MemberId = user.MemberId,
            MemberType = user.MemberType.ToUserMemberType(),
            Name = user.Name,
            Email = user.Email,
            GraduationYear = user.GraduationYear,
        };
    }

    public static User.V1.User ToGrpcUser(this Entities.User user)
    {
        return new User.V1.User
        {
            Id = user.Id.ToString(),
            MemberId = user.MemberId,
            MemberType = user.MemberType.ToGrpcUserMemberType(),
            Name = user.Name,
            Email = user.Email,
            GraduationYear = user.GraduationYear,
        };
    }
}