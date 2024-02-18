using Api.Entities;

namespace Api.Extensions;

public static class UserExtensions
{
    public static Membership ToMembership(this User.V1.Membership membership)
    {
        return Enum.Parse<Membership>(membership.ToString());
    }

    public static User.V1.Membership ToGrpcMembership(this Membership membership)
    {
        return Enum.Parse<User.V1.Membership>(membership.ToString());
    }

    public static Entities.User ToUser(this User.V1.User user)
    {
        return new Entities.User
        {
            Id = Guid.Parse(user.Id),
            Name = user.Name,
            Email = user.Email,
            FirebaseId = user.FirebaseId
        };
    }

    public static User.V1.User ToGrpcUser(this Entities.User user)
    {
        switch (user)
        {
            case Employee:
                return new User.V1.User
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    Email = user.Email,
                    FirebaseId = user.FirebaseId,
                    Teacher = new User.V1.Teacher()
                };
            case SystemAdmin:
                return new User.V1.User
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    Email = user.Email,
                    FirebaseId = user.FirebaseId,
                    SystemAdmin = new User.V1.SystemAdmin()
                };
            case ServiceAccount:
                return new User.V1.User
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    Email = user.Email,
                    FirebaseId = user.FirebaseId,
                    ServiceAccount = new User.V1.ServiceAccount()
                };
            case Member m:
                var member = new User.V1.Member
                {
                        MemberId = m.MemberId,
                        Membership = m.Membership.ToGrpcMembership(),
                };
                
                switch (m)
                {
                    case AlumniMember am:
                        member.AlumniMember = new User.V1.AlumniMember();
                        if (am.GraduationYear.HasValue)
                        {
                            member.AlumniMember.GraduationYear = am.GraduationYear.GetValueOrDefault();
                        }
                        break;
                    case EmployeeMember sm:
                        member.EmployeeMember = new User.V1.EmployeeMember();
                        if (sm.GraduationYear.HasValue)
                        {
                            member.AlumniMember.GraduationYear = sm.GraduationYear.GetValueOrDefault();
                        }
                        break;
                    default:
                        throw new Exception("Unknown member type detected.");
                }
                
                return new User.V1.User
                {
                    Id = m.Id.ToString(),
                    Name = m.Name,
                    Email = m.Email,
                    FirebaseId = m.FirebaseId,
                    Member = member,
                };
            default:
                throw new Exception("Unknown user type detected.");
        }
    }
}