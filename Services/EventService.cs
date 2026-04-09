using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;
using PopUpOslo.Infrastructure.Repositories;

namespace PopUpOslo.Services;

public class EventService
{
    private readonly EventRepository _eventRepository = new();

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
            Title = title,
            Description = description,
            Venue = venue,
            DateTime = dateTime,
            Category = category,
            Type = type,
            OrganizerId = organizerId,
            Status = EventStatus.Upcoming
        };

        _eventRepository.AddEvent(ev);
    }

    public List<Event> GetAllEvents()
    {
        return _eventRepository.GetAllEvents();
    }

    public List<Event> GetEventsByOrganizer(int organizerId)
    {
        return _eventRepository.GetEventsByOrganizer(organizerId);
    }

    public Event? GetEventById(int eventId)
    {
        return _eventRepository.GetEventById(eventId);
    }

    public List<Event> SearchEvents(string keyword)
    {
        return _eventRepository.SearchEvents(keyword);
    }

    public List<Event> FilterByCategory(EventCategory category)
    {
        return _eventRepository.FilterByCategory(category.ToString());
    }

    public List<Event> FilterByType(EventType type)
    {
        return _eventRepository.FilterByType(type.ToString());
    }

    public void CancelEvent(int eventId)
    {
        _eventRepository.CancelEvent(eventId);
    }

    public void UpdateEvent(Event ev)
    {
        _eventRepository.UpdateEvent(ev);
    }
}