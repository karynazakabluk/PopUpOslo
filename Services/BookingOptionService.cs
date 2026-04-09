using PopUpOslo.Domain.Entities;
using PopUpOslo.Infrastructure.Repositories;

namespace PopUpOslo.Services;

public class BookingOptionService
{
    private readonly BookingOptionRepository _bookingOptionRepository = new();

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
}