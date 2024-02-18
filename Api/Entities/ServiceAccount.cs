namespace Api.Entities;

public enum ServiceAccountType
{
    GuardHouse,
}

public class ServiceAccount : User
{
    public ServiceAccountType ServiceAccountType { get; set; }
}