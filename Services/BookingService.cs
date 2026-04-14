using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;
using PopUpOslo.Infrastructure.Repositories;

namespace PopUpOslo.Services;

public class BookingService
{
    private readonly BookingRepository _bookingRepository = new();
    private readonly BookingOptionRepository _bookingOptionRepository = new();

    // CREATE BOOKING (USER SELECTS TICKET TYPE)
    public bool CreateBooking(int userId, int eventId, int optionId)
    {
        // 1. Get selected ticket option
        var option = _bookingOptionRepository.GetOptionById(optionId);

        if (option == null || option.EventId != eventId)
        {
            return false;
        }

        // 2. Check capacity (EVENT CAPACITY FEATURE)
        if (option.RemainingCapacity <= 0)
        {
            Console.WriteLine($"{option.Name} tickets are sold out.");
            return false;
        }

        // 3. Prevent duplicate booking for same option
        var existingBookings = _bookingRepository.GetBookingsByUser(userId);

        bool alreadyBooked = existingBookings.Any(b =>
            b.EventId == eventId &&
            b.OptionId == optionId &&
            b.Status == BookingStatus.Booked);

        if (alreadyBooked)
        {
            return false;
        }

        // 4. Create booking
        var booking = new Booking
        {
            UserId = userId,
            EventId = eventId,
            OptionId = option.OptionId,
            PriceAtBooking = option.Price,
            Status = BookingStatus.Booked,
            BookingDate = DateTime.Now.ToString("s")
        };

        _bookingRepository.AddBooking(booking);

        // 5. Reduce capacity (IMPORTANT)
        _bookingOptionRepository.ReduceCapacity(option.OptionId);

        return true;
    }

    // GET BOOKINGS
    public List<Booking> GetBookingsByUser(int userId)
    {
        return _bookingRepository.GetBookingsByUser(userId);
    }

    // GET SINGLE BOOKING
    public Booking? GetBookingById(int bookingId)
    {
        return _bookingRepository.GetBookingById(bookingId);
    }

    // CANCEL BOOKING (RESTORES CAPACITY)
    public bool CancelBooking(int bookingId, int userId)
    {
        var booking = _bookingRepository.GetBookingById(bookingId);

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

        // 1. Update booking status
        _bookingRepository.CancelBooking(bookingId);

        // 2. Restore capacity
        _bookingOptionRepository.IncreaseCapacity(booking.OptionId);

        return true;
    }
}