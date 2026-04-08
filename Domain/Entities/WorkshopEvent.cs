using PopUpOslo.Domain.Enums;

namespace PopUpOslo.Domain.Entities;

public class WorkshopEvent : Event
{
    public string RequiredMaterials { get; set; } = string.Empty;

    public WorkshopEvent()
    {
        Type = EventType.Workshop;
    }

    public override string GetExtraInfo()
    {
        return $"Required materials: {RequiredMaterials}";
    }
}