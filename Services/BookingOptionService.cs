using PopUpOslo.Domain.Entities;
using PopUpOslo.Infrastructure.Repositories;

namespace PopUpOslo.Services;

public class BookingOptionService
{
    private readonly BookingOptionRepository _bookingOptionRepository = new();

    // Create default ticket option
    public void CreateDefaultOption(int eventId)
    {
        var option = new BookingOption
        {
            EventId = eventId,
            Name = "Standard",
            Price = 0,
            Capacity = 50,
            RemainingCapacity = 50
        };

        _bookingOptionRepository.AddOption(option);
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