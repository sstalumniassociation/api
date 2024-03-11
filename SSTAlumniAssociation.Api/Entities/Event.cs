namespace SSTAlumniAssociation.Api.Entities;

public class Event
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string BadgeImage { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }

    public List<User> Attendees { get; set; }
    public List<UserEvent> UserEvents { get; set; }
}