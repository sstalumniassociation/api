namespace Api.Entities;

public class UserEvent
{
    public Guid Id { get; set; }

    /// <summary>
    /// The admission key is the same as the ID generated
    /// </summary>
    public Guid AdmissionKey => Id;

    /// <summary>
    /// UserEvent is deleted if parent <see cref="Event"/> is deleted
    /// </summary>
    public bool Deleted => Event.Deleted;

    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid EventId { get; set; }
    public Event Event { get; set; }
}