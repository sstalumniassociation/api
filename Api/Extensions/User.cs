using Api.Entities;

namespace Api.Extensions;

public static class UserExtensions
{
    public static ServiceAccountType ToServiceAccountType(this User.V1.ServiceAccountType serviceAccountType)
    {
        return Enum.Parse<ServiceAccountType>(serviceAccountType.ToString());
    }

    public static Membership ToMembership(this User.V1.Membership membership)
    {
        return Enum.Parse<Membership>(membership.ToString());
    }

    public static User.V1.Membership ToGrpcMembership(this Membership membership)
    {
        return Enum.Parse<User.V1.Membership>(membership.ToString());
    }

    public static User.V1.ServiceAccountType ToGrpcServiceAccountType(this ServiceAccountType serviceAccountType)
    {
        return Enum.Parse<User.V1.ServiceAccountType>(serviceAccountType.ToString());
    }

    public static Entities.User ToUser(this User.V1.User user)
    {
        var u = new Entities.User(
            id: Guid.Parse(user.Id),
            name: user.Name,
            email: user.Email,
            firebaseId: user.FirebaseId
        );

        switch (user.UserTypeCase)
        {
            case User.V1.User.UserTypeOneofCase.None:
                return u;
            case User.V1.User.UserTypeOneofCase.ServiceAccount:
                return new ServiceAccount(
                    u,
                    serviceAccountType: user.ServiceAccount.ServiceAccountType.ToServiceAccountType()
                );
            case User.V1.User.UserTypeOneofCase.SystemAdmin:
                return new SystemAdmin(u);
            case User.V1.User.UserTypeOneofCase.Employee:
                return new Employee(u);
            case User.V1.User.UserTypeOneofCase.Member:
                switch (user.Member.MemberTypeCase)
                {
                    case User.V1.Member.MemberTypeOneofCase.AlumniMember:
                        return new AlumniMember(u)
                        {
                            GraduationYear = user.Member.AlumniMember.GraduationYear
                        };
                    case User.V1.Member.MemberTypeOneofCase.EmployeeMember:
                        return new EmployeeMember(u)
                        {
                            GraduationYear = user.Member.EmployeeMember.GraduationYear
                        };
                    case User.V1.Member.MemberTypeOneofCase.None:
                    default:
                        throw new Exception("User is not a valid member.");
                }
            default:
                throw new Exception("Unrecognized user type.");
        }
    }

    public static User.V1.User ToGrpcUser(this Entities.User user)
    {
        switch (user)
        {
            case Employee e:
                return new User.V1.User
                {
                    Id = e.Id.ToString(),
                    Name = e.Name,
                    Email = e.Email,
                    FirebaseId = e.FirebaseId,
                };
            case SystemAdmin systemAdmin:
                return new User.V1.User
                {
                    Id = systemAdmin.Id.ToString(),
                    Name = systemAdmin.Name,
                    Email = systemAdmin.Email,
                    FirebaseId = systemAdmin.FirebaseId,
                    SystemAdmin = new User.V1.SystemAdmin()
                };
            case ServiceAccount serviceAccount:
                return new User.V1.User
                {
                    Id = serviceAccount.Id.ToString(),
                    Name = serviceAccount.Name,
                    Email = serviceAccount.Email,
                    FirebaseId = serviceAccount.FirebaseId,
                    ServiceAccount = new User.V1.ServiceAccount
                    {
                        ServiceAccountType = serviceAccount.ServiceAccountType.ToGrpcServiceAccountType()
                    }
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