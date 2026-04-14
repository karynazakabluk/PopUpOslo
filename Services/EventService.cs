using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;
using PopUpOslo.Infrastructure.Repositories;

namespace PopUpOslo.Services;

public class EventService
{
    private readonly EventRepository _eventRepository = new();

    public int CreateEvent(
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

    	return _eventRepository.AddEvent(ev);
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
  
    public void CancelEvent(int eventId)
    {
        _eventRepository.CancelEvent(eventId);
    }


	public void UpdateEvent(Event updatedEvent)
	{
    	_eventRepository.UpdateEvent(updatedEvent);
	}


}