using Microsoft.EntityFrameworkCore;

namespace Api.Entities;

[Index(nameof(FirebaseId), IsUnique = true)]
public class User
{
    public Guid Id { get; set; }

    /// <summary>
    /// Use Firebase Auth provided ID as SSOT.
    /// </summary>
    public string FirebaseId { get; set; }

    /// <summary>
    /// Ignore Firebase Auth provided values and force user to provide their own.
    /// </summary>
    public string Name { get; set; }
    public string Email { get; set; }

    public List<Event> Events { get; set; }
    public List<UserEvent> UserEvents { get; set; }
}

