using PopUpOslo.Domain.Enums;

namespace PopUpOslo.Domain.Entities;

public class Booking
{
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public int OptionId { get; set; }
    public double PriceAtBooking { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Booked;
    public string BookingDate { get; set; } = string.Empty;
}