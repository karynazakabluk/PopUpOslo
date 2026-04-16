using PopUpOslo.Domain.Entities;
using PopUpOslo.Infrastructure.Repositories;

namespace PopUpOslo.Services;

public class BookingOptionService
{
    private readonly BookingOptionRepository _bookingOptionRepository = new();

    // Create default ticket option
    public void CreateDefaultOption(int eventId)
    {
        var options = new List<BookingOption>
        {
            new BookingOption
            {
                EventId = eventId,
                Name = "Standard",
                Price = 200,
                Capacity = 50,
                RemainingCapacity = 50
            },
            new BookingOption
            {
                EventId = eventId,
                Name = "VIP",
                Price = 500,
                Capacity = 10,
                RemainingCapacity = 10
            }
        };

        foreach (var option in options)
        {
            _bookingOptionRepository.AddOption(option);
        }
    }
    
    public List<BookingOption> GetOptionsByEvent(int eventId)
    {
        return _bookingOptionRepository.GetOptionsByEvent(eventId);
    }

    // Optional but useful
    public BookingOption? GetById(int optionId)
    {
        return _bookingOptionRepository.GetOptionById(optionId);
    }
}