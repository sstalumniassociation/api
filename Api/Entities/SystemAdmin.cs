namespace Api.Entities;

public class SystemAdmin : User
{
    public SystemAdmin(Guid id, string name, string email, string firebaseId) : base(id, name, email, firebaseId)
    {
    }

    public SystemAdmin(User user) : base(user)
    {
    }
};