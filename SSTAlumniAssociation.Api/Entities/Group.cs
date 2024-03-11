namespace SSTAlumniAssociation.Api.Entities;

/// <summary>
/// A group represents any collection of users, for example, a Special Interest Group (SIG)
/// </summary>
public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Member> Members { get; set; }
}