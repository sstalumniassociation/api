using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SSTAlumniAssociation.Api.Entities;

[Index(nameof(FirebaseId), IsUnique = true)]
public class User
{
    public User(Guid id, string name, string email, string firebaseId)
    {
        Id = id;
        Name = name;
        Email = email;
        FirebaseId = firebaseId;
    }

    public User(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        FirebaseId = user.FirebaseId;
    }

    public Guid Id { get; set; }

    /// <summary>
    /// Ignore Firebase Auth provided values and force user to provide their own.
    /// </summary>
    public string Name { get; set; }

    public string Email { get; set; }

    /// <summary>
    /// Use Firebase Auth provided ID as SSOT.
    /// </summary>
    public string FirebaseId { get; set; }

    public List<Event> Events { get; set; }
    public List<UserEvent> UserEvents { get; set; }
}
