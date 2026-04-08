using PopUpOslo.Domain.Enums;

namespace PopUpOslo.Domain.Entities;

public class DiningEvent : Event
{
    public string CuisineType { get; set; } = string.Empty;

    public DiningEvent()
    {
        Type = EventType.Dining;
    }

    public override string GetExtraInfo()
    {
        return $"Cuisine type: {CuisineType}";
    }
}