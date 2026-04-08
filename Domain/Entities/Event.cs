using PopUpOslo.Domain.Enums;

namespace PopUpOslo.Domain.Entities;

public class Event
{
    public int EventId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public EventCategory Category { get; set; }
    public EventType Type { get; set; }
    public DateTime DateTime { get; set; }
    public string Venue { get; set; } = string.Empty;
    public int OrganizerId { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Upcoming;

    public virtual string GetExtraInfo()
    {
        return string.Empty;
    }
}