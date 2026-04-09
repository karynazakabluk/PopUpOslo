using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;

namespace PopUpOslo.Services;

public class BookingService
{
    private readonly List<Booking> _bookings = new();
    private int _nextBookingId = 1;

    public bool CreateBooking(int userId, Event ev)
    {
        bool alreadyBooked = _bookings.Any(b =>
            b.UserId == userId &&
            b.EventId == ev.EventId &&
            b.Status == BookingStatus.Booked);

        if (alreadyBooked)
        {
            return false;
        }

        var booking = new Booking
        {
            BookingId = _nextBookingId++,
            UserId = userId,
            EventId = ev.EventId,
            OptionId = 0,
            PriceAtBooking = 0,
            Status = BookingStatus.Booked,
            BookingDate = DateTime.Now.ToString("g")
        };

        _bookings.Add(booking);
        return true;
    }

    public List<Booking> GetBookingsByUser(int userId)
    {
        return _bookings
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingId)
            .ToList();
    }

    public Booking? GetBookingById(int bookingId)
    {
        return _bookings.FirstOrDefault(b => b.BookingId == bookingId);
    }

    public bool CancelBooking(int bookingId, int userId)
    {
        var booking = _bookings.FirstOrDefault(b =>
            b.BookingId == bookingId &&
            b.UserId == userId &&
            b.Status == BookingStatus.Booked);

        if (booking == null)
        {
            return false;
        }

        booking.Status = BookingStatus.Cancelled;
        return true;
    }
}