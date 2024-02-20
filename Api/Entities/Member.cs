using Microsoft.EntityFrameworkCore;

namespace Api.Entities;

/// <summary>
/// The role that the member has within SSTAA.
/// This value is stored in the database as a string.
/// This enum should rarely ever be modified, as it will very likely cause a breaking change.
/// </summary>
public enum Membership
{
    Exco,

    /// <summary>
    /// All past/present staff and students who completed at least 1 year of study in SST but did not graduate
    /// </summary>
    Associate,

    /// <summary>
    /// All graduated alumni who are under 21
    /// </summary>
    Affiliate,
    Ordinary,
    Revoked,
}

[Index(nameof(MemberId), IsUnique = true)]
public abstract class Member : User
{
    public Member(User user) : base(user)
    {
    }

    public Member(Guid id, string name, string email, string firebaseId, Membership membership, string memberId) :
        base(id, name, email, firebaseId)
    {
        Membership = membership;
        MemberId = memberId;
    }

    public Membership Membership { get; set; }

    /// <summary>
    /// Internal member ID used for tracking by SSTAA admin.
    /// </summary>
    public string MemberId { get; set; }
}

/// <summary>
/// If the member was previously a student of SST, they are considered an alumni member.
/// </summary>
public class AlumniMember : Member
{
    public AlumniMember(User user) : base(user)
    {
    }

    public AlumniMember(Guid id, string name, string email, string firebaseId, Membership membership, string memberId) :
        base(id, name, email, firebaseId, membership, memberId)
    {
    }

    public int? GraduationYear { get; set; }
}

/// <summary>
/// If the member is a current staff, or ex-staff of SST, they are considered a staff member.
/// This differs from <see cref="Employee"/> as any user with StaffMember can be assumed to be a registered SSTAA member.
/// <see cref="Employee"/> can also only be used for current staff.
///
/// This member can only have membership of <see cref="Membership.Associate"/>.
/// </summary>
public class EmployeeMember : Member
{
    public EmployeeMember(User user) : base(user)
    {
    }

    public EmployeeMember(Guid id, string name, string email, string firebaseId, Membership membership, string memberId)
        : base(id, name, email, firebaseId, membership, memberId)
    {
    }

    public int? GraduationYear { get; set; }
};