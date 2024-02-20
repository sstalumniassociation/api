namespace Api.Entities;

public class Employee : User
{
    public Employee(User user) : base(user)
    {
    }

    public Employee(Guid id, string name, string email, string firebaseId) : base(id, name, email, firebaseId)
    {
    }
};