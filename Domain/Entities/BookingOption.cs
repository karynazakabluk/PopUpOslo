namespace PopUpOslo.Domain.Entities;

public class BookingOption
{
    public int OptionId { get; set; }
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public int Capacity { get; set; }
    public int RemainingCapacity { get; set; }
}