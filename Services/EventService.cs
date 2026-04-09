using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;

namespace PopUpOslo.Services;

public class EventService
{
    private readonly List<Event> _events = new();
    private int _nextEventId = 1;

    public EventService()
    {
        SeedSampleEvents();
    }

    public void CreateEvent(
        string title,
        string description,
        string venue,
        DateTime dateTime,
        EventCategory category,
        EventType type,
        int organizerId)
    {
        var ev = new Event
        {
            EventId = _nextEventId++,
            Title = title,
            Description = description,
            Venue = venue,
            DateTime = dateTime,
            Category = category,
            Type = type,
            OrganizerId = organizerId,
            Status = EventStatus.Upcoming
        };

        _events.Add(ev);
    }

    public List<Event> GetAllEvents()
    {
        return _events
            .OrderBy(e => e.DateTime)
            .ToList();
    }

    public List<Event> GetEventsByOrganizer(int organizerId)
    {
        return _events
            .Where(e => e.OrganizerId == organizerId)
            .OrderBy(e => e.DateTime)
            .ToList();
    }

    public Event? GetEventById(int eventId)
    {
        return _events.FirstOrDefault(e => e.EventId == eventId);
    }

    public List<Event> SearchEvents(string keyword)
    {
        keyword = keyword.Trim().ToLower();

        return _events
            .Where(e =>
                e.Title.ToLower().Contains(keyword) ||
                e.Description.ToLower().Contains(keyword) ||
                e.Venue.ToLower().Contains(keyword))
            .OrderBy(e => e.DateTime)
            .ToList();
    }

    public List<Event> FilterByCategory(EventCategory category)
    {
        return _events
            .Where(e => e.Category == category)
            .OrderBy(e => e.DateTime)
            .ToList();
    }

    public List<Event> FilterByType(EventType type)
    {
        return _events
            .Where(e => e.Type == type)
            .OrderBy(e => e.DateTime)
            .ToList();
    }

    private void SeedSampleEvents()
    {
        _events.Add(new Event
        {
            EventId = _nextEventId++,
            Title = "Oslo Coffee Tasting",
            Description = "A guided tasting of speciality coffee.",
            Venue = "Vulkan",
            DateTime = new DateTime(2026, 5, 12, 18, 0, 0),
            Category = EventCategory.Food,
            Type = EventType.Dining,
            OrganizerId = 99,
            Status = EventStatus.Upcoming
        });

        _events.Add(new Event
        {
            EventId = _nextEventId++,
            Title = "Beginner Pottery Workshop",
            Description = "Introductory pottery workshop for beginners.",
            Venue = "Grünerløkka",
            DateTime = new DateTime(2026, 5, 15, 17, 30, 0),
            Category = EventCategory.Education,
            Type = EventType.Workshop,
            OrganizerId = 98,
            Status = EventStatus.Upcoming
        });

        _events.Add(new Event
        {
            EventId = _nextEventId++,
            Title = "Pop-up Sushi Night",
            Description = "A themed sushi dining experience.",
            Venue = "Tøyen",
            DateTime = new DateTime(2026, 5, 20, 19, 0, 0),
            Category = EventCategory.Culture,
            Type = EventType.Dining,
            OrganizerId = 97,
            Status = EventStatus.Upcoming
        });
    }
}