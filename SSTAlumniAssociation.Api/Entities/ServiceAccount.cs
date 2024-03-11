namespace SSTAlumniAssociation.Api.Entities;

public enum ServiceAccountType
{
    GuardHouse,
}

public class ServiceAccount : User
{
    public ServiceAccount(Guid id, string name, string email, string firebaseId, ServiceAccountType serviceAccountType)
        : base(id, name, email, firebaseId)
    {
        ServiceAccountType = serviceAccountType;
    }

    public ServiceAccount(User user, ServiceAccountType serviceAccountType) : base(user)
    {
        ServiceAccountType = serviceAccountType;
    }

    public ServiceAccountType ServiceAccountType { get; set; }
}