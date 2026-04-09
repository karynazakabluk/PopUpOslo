using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;
using PopUpOslo.Infrastructure.Repositories;

namespace PopUpOslo.Services;

public class BookingService
{
    private readonly BookingRepository _bookingRepository = new();
    private readonly BookingOptionRepository _bookingOptionRepository = new();

    public bool CreateBooking(int userId, Event selectedEvent)
    {
        if (selectedEvent == null)
        {
            return false;
        }

        var existingBookings = _bookingRepository.GetBookingsByUser(userId);

        bool alreadyBooked = existingBookings.Any(b =>
            b.EventId == selectedEvent.EventId &&
            b.Status == BookingStatus.Booked);

        if (alreadyBooked)
        {
            return false;
        }

        var options = _bookingOptionRepository.GetOptionsByEvent(selectedEvent.EventId);
        var option = options.FirstOrDefault(o => o.RemainingCapacity > 0);

        if (option == null)
        {
            return false;
        }

        var booking = new Booking
        {
            UserId = userId,
            EventId = selectedEvent.EventId,
            OptionId = option.OptionId,
            PriceAtBooking = option.Price,
            Status = BookingStatus.Booked,
            BookingDate = DateTime.Now.ToString("s")
        };

        _bookingRepository.AddBooking(booking);
        _bookingOptionRepository.ReduceCapacity(option.OptionId);

        return true;
    }

    public List<Booking> GetBookingsByUser(int userId)
    {
        return _bookingRepository.GetBookingsByUser(userId);
    }

    public Booking? GetBookingById(int bookingId)
    {
        return _bookingRepository.GetBookingById(bookingId);
    }

    public bool CancelBooking(int bookingId, int userId)
    {
        Booking? booking = _bookingRepository.GetBookingById(bookingId);

        if (booking == null)
        {
            return false;
        }

        if (booking.UserId != userId)
        {
            return false;
        }

        if (booking.Status != BookingStatus.Booked)
        {
            return false;
        }

        _bookingRepository.CancelBooking(bookingId);
        _bookingOptionRepository.IncreaseCapacity(booking.OptionId);

        return true;
    }
}