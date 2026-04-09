using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;

namespace PopUpOslo.Services;

public class SearchService
{
    public List<Event> SearchEvents(List<Event> events, string keyword)
    {
        keyword = keyword.Trim().ToLower();

        return events
            .Where(e =>
                e.Title.ToLower().Contains(keyword) ||
                e.Description.ToLower().Contains(keyword) ||
                e.Venue.ToLower().Contains(keyword))
            .OrderBy(e => e.DateTime)
            .ToList();
    }

    public List<Event> FilterByCategory(List<Event> events, EventCategory category)
    {
        return events
            .Where(e => e.Category == category)
            .OrderBy(e => e.DateTime)
            .ToList();
    }

    public List<Event> FilterByType(List<Event> events, EventType type)
    {
        return events
            .Where(e => e.Type == type)
            .OrderBy(e => e.DateTime)
            .ToList();
    }
}