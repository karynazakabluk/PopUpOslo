using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;
using PopUpOslo.Services;

namespace PopUpOslo.UI;

public class ApplicationRunner
{
    private bool _isRunning = true;
    private bool _isLoggedIn = false;
    private string _currentUsername = "Guest";
    private int _currentUserId = 0;

    private readonly EventService _eventService = new();
    private readonly AuthService _authService = new();
    private readonly BookingService _bookingService = new();
    private readonly ReviewService _reviewService = new();
    private readonly SearchService _searchService = new();
    private readonly BookingOptionService _bookingOptionService = new();

    public void Run()
    {
        while (_isRunning)
        {
            if (_isLoggedIn)
            {
                RunUserMenu();
            }
            else
            {
                RunGuestMenu();
            }
        }
    }

    private void RunGuestMenu()
    {
        Menu.ShowWelcome();
        Menu.ShowMainMenu();

        int choice = InputHandler.ReadInt("Choose an option: ");
        Console.WriteLine();

        switch (choice)
        {
            case 1:
                HandleRegister();
                break;
            case 2:
                HandleLogin();
                break;
            case 3:
                HandleBrowseEvents();
                break;
            case 0:
                HandleExit();
                break;
            default:
                ShowInvalidOption();
                break;
        }
    }

    private void RunUserMenu()
    {
        Menu.ShowUserMenu(_currentUsername);

        int choice = InputHandler.ReadInt("Choose an option: ");
        Console.WriteLine();

        switch (choice)
        {
            case 1:
                HandleCreateEvent();
                break;
            case 2:
                HandleViewMyEvents();
                break;
            case 3:
                HandleBrowseEvents();
                break;
            case 4:
                HandleBookEvent();
                break;
            case 5:
                HandleMyBookings();
                break;
            case 6:
                HandleLeaveReview();
                break;
            case 0:
                HandleLogout();
                break;
            default:
                ShowInvalidOption();
                break;
        }
    }

    private void HandleRegister()
    {
        Menu.ShowSectionTitle("Register");

        string username = InputHandler.ReadRequiredString("Enter username: ");
        string password = InputHandler.ReadPassword("Enter password: ");

        bool success = _authService.Register(username, password);

        if (!success)
        {
            Menu.ShowError("Registration failed. Username may already exist.");
            Menu.Pause();
            return;
        }

        Menu.ShowSuccess($"Account for '{username}' created successfully.");
  
    }

    private void HandleLogin()
    {
        Menu.ShowSectionTitle("Login");

        string username = InputHandler.ReadRequiredString("Enter username: ");
        string password = InputHandler.ReadPassword("Enter password: ");

        User? user = _authService.Login(username, password);

        if (user == null)
        {
            Menu.ShowError("Invalid username or password.");
            Menu.Pause();
            return;
        }

        _isLoggedIn = true;
        _currentUsername = user.Username;
        _currentUserId = user.UserId;

        Menu.ShowSuccess($"Welcome, {user.Username}!");
        
    }

    private void HandleBrowseEvents()
    {
        Menu.ShowSectionTitle("Browse Events");

        var allEvents = _eventService.GetAllEvents();

        if (allEvents.Count == 0)
        {
            Menu.ShowMessage("No events available.");
            Menu.Pause();
            return;
        }

        Console.WriteLine("Browse Options");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1. View all events");
        Console.WriteLine("2. Search by keyword");
        Console.WriteLine("3. Filter by category");
        Console.WriteLine("4. Filter by type");
        Console.WriteLine("0. Back");
        Console.WriteLine();

        int choice = InputHandler.ReadInt("Choose an option: ");
        Console.WriteLine();

        List<Event> results = choice switch
        {
            1 => _eventService.GetAllEvents(),
            2 => HandleKeywordSearch(allEvents),
            3 => HandleCategoryFilter(allEvents),
            4 => HandleTypeFilter(allEvents),
            0 => new List<Event>(),
            _ => new List<Event>()
        };

        if (choice == 0)
        {
            return;
        }

        if (choice < 0 || choice > 4)
        {
            Menu.ShowError("Invalid option");
            Menu.Pause();
            return;
        }

        DisplayEventResults(results);
    }

    private List<Event> HandleKeywordSearch(List<Event> allEvents)
    {
        string keyword = InputHandler.ReadRequiredString("Enter keyword: ");
        return _searchService.SearchEvents(allEvents, keyword);
    }

    private List<Event> HandleCategoryFilter(List<Event> allEvents)
    {
        Console.WriteLine("Categories");
        Console.WriteLine("1. Food");
        Console.WriteLine("2. Networking");
        Console.WriteLine("3. Education");
        Console.WriteLine("4. Culture");
        Console.WriteLine("5. Other");
        Console.WriteLine();

        int categoryChoice = InputHandler.ReadIntInRange("Choose category (1-5): ", 1, 5);

        EventCategory selectedCategory = MapCategory(categoryChoice);

        return _searchService.FilterByCategory(allEvents, selectedCategory);
    }

    private List<Event> HandleTypeFilter(List<Event> allEvents)
    {
        Console.WriteLine("Types");
        Console.WriteLine("1. Workshop");
        Console.WriteLine("2. Dining");
        Console.WriteLine();

        int typeChoice = InputHandler.ReadIntInRange("Choose type (1-2): ", 1, 2);

        EventType selectedType = MapType(typeChoice);

        return _searchService.FilterByType(allEvents, selectedType);
    }

    private void DisplayEventResults(List<Event> results)
    {
        Console.WriteLine("Results");
        Console.WriteLine("----------------------------------------");

        if (results.Count == 0)
        {
            Menu.ShowMessage("No matching events found.");
          
            return;
        }

        foreach (var ev in results)
        {
            double avgRating = _reviewService.GetAverageRating(ev.EventId);

            Console.WriteLine($"{ev.EventId}. {ev.Title} | {ev.Category} | {ev.Type} | {ev.Venue} | {ev.DateTime:g} | {ev.Status}");

            if (avgRating > 0)
            {
                Console.WriteLine($"   Average rating: {avgRating:F1}/5");
            }
        }

        Console.WriteLine();

        bool viewDetails = InputHandler.Confirm("View event details?");

        if (!viewDetails)
        {
           
            return;
        }

        int eventId = InputHandler.ReadInt("Enter event id: ");
        Event? selected = _eventService.GetEventById(eventId);

        if (selected == null)
        {
            Menu.ShowError("Event not found.");
            Menu.Pause();
            return;
        }

        DisplayEventDetails(selected);
    }

    private void DisplayEventDetails(Event selected)
    {
        Menu.ShowSectionTitle("Event Details");
        Console.WriteLine($"Title: {selected.Title}");
        Console.WriteLine($"Description: {selected.Description}");
        Console.WriteLine($"Venue: {selected.Venue}");
        Console.WriteLine($"Date: {selected.DateTime:g}");
        Console.WriteLine($"Category: {selected.Category}");
        Console.WriteLine($"Type: {selected.Type}");
        Console.WriteLine($"Status: {selected.Status}");

        double avgRating = _reviewService.GetAverageRating(selected.EventId);

        Console.WriteLine(
    		avgRating > 0
        		? $"Average rating: {avgRating:F1}/5"
        		: "Average rating: No ratings yet");

        Console.WriteLine();
        Console.WriteLine("Reviews");
        Console.WriteLine("----------------------------------------");

        var reviews = _reviewService.GetReviewsByEvent(selected.EventId);

        if (reviews.Count == 0)
        {
            Console.WriteLine("No reviews yet.");
        }
        else
        {
            foreach (var review in reviews)
            {
                Console.WriteLine($"Rating: {review.Rating}/5");
                Console.WriteLine($"Comment: {review.Comment}");
                Console.WriteLine($"Created: {review.CreatedAt}");
                Console.WriteLine("----------------------------------------");
            }
        }

        Console.WriteLine();
		Menu.Pause();
       
    }

    private void HandleCreateEvent()
    {
        Menu.ShowSectionTitle("Create Event");

        string title = InputHandler.ReadRequiredString("Enter title: ");
        string description = InputHandler.ReadRequiredString("Enter description: ");
        string venue = InputHandler.ReadRequiredString("Enter venue: ");
        DateTime dateTime = InputHandler.ReadDateTime("Enter date and time (e.g. 2026-05-10 18:30): ");

        Console.WriteLine();
        Console.WriteLine("Choose category:");
        Console.WriteLine("1. Food");
        Console.WriteLine("2. Networking");
        Console.WriteLine("3. Education");
        Console.WriteLine("4. Culture");
        Console.WriteLine("5. Other");
        int categoryChoice = InputHandler.ReadIntInRange("Choose category (1-5): ", 1, 5);

        Console.WriteLine();
        Console.WriteLine("Choose event type:");
        Console.WriteLine("1. Workshop");
        Console.WriteLine("2. Dining");
        int typeChoice = InputHandler.ReadIntInRange("Choose type (1-2): ", 1, 2);

        EventCategory category = MapCategory(categoryChoice);
		EventType type = MapType(typeChoice);

        int eventId = _eventService.CreateEvent(
    		title,
    		description,
    		venue,
    		dateTime,
    		category,
    		type,
    		_currentUserId);

		_bookingOptionService.CreateDefaultOption(eventId);

		Menu.ShowSuccess("Event created successfully.");
      
    }

    private void HandleViewMyEvents()
    {
        Menu.ShowSectionTitle("My Events");

        var myEvents = _eventService.GetEventsByOrganizer(_currentUserId);

        if (myEvents.Count == 0)
        {
            Menu.ShowMessage("You have not created any events yet.");
            Menu.Pause();
            return;
        }

        foreach (var ev in myEvents)
        {
            double avgRating = _reviewService.GetAverageRating(ev.EventId);

            Console.WriteLine($"{ev.EventId}. {ev.Title} | {ev.Category} | {ev.Type} | {ev.Venue} | {ev.DateTime:g} | {ev.Status}");

            if (avgRating > 0)
    		{
        		Console.WriteLine($"   Average rating: {avgRating:F1}/5");
    		}

        }

        Console.WriteLine();
        Menu.Pause();
    }

    private void HandleBookEvent()
{
    Menu.ShowSectionTitle("Book Event");

    var events = _eventService.GetAllEvents();

    if (events.Count == 0)
    {
        Menu.ShowMessage("No events available to book.");
        Menu.Pause();
        return;
    }

    foreach (var ev in events)
    {
        Console.WriteLine($"{ev.EventId}. {ev.Title}");
    }

    Console.WriteLine();

    int eventId = InputHandler.ReadInt("Enter event id: ");
    Event? selectedEvent = _eventService.GetEventById(eventId);

    if (selectedEvent == null)
    {
        Menu.ShowError("Event not found.");
        Menu.Pause();
        return;
    }

    // 🔄 ALWAYS LOAD FRESH DATA FROM DATABASE
    var options = _bookingOptionService.GetOptionsByEvent(eventId);

    if (options.Count == 0)
    {
        Menu.ShowError("No ticket options available for this event.");
        Menu.Pause();
        return;
    }

    Console.WriteLine();
    Console.WriteLine("Ticket Options:");
    Console.WriteLine("--------------------------------");

    foreach (var o in options)
    {
        string status = o.RemainingCapacity == 0
            ? "SOLD OUT"
            : $"Left: {o.RemainingCapacity}";

        Console.WriteLine($"{o.OptionId}. {o.Name} - {o.Price} NOK ({status})");
    }

    Console.WriteLine();

    int selectedOptionId = InputHandler.ReadInt("Select ticket option id: ");

    // 🧠 GET OPTION AGAIN (ENSURE FRESH VALUE)
    var selectedOption = _bookingOptionService.GetById(selectedOptionId);

    if (selectedOption == null)
    {
        Menu.ShowError("Invalid ticket option.");
        Menu.Pause();
        return;
    }

    if (selectedOption.RemainingCapacity <= 0)
    {
        Menu.ShowError("This ticket is SOLD OUT.");
        Menu.Pause();
        return;
    }

    bool confirm = InputHandler.Confirm($"Book '{selectedEvent.Title}'?");

    if (!confirm)
    {
        Menu.ShowMessage("Booking cancelled.");
        return;
    }

    bool success = _bookingService.CreateBooking(
        _currentUserId,
        selectedEvent.EventId,
        selectedOptionId
    );

    if (!success)
    {
        Menu.ShowError("Booking failed. You may already have a booking or it is sold out.");
        Menu.Pause();
        return;
    }

    Menu.ShowSuccess($"Booked: {selectedEvent.Title}");

    // 🔄 REFRESH AFTER BOOKING (IMPORTANT FIX)
    var updatedOptions = _bookingOptionService.GetOptionsByEvent(eventId);

    Console.WriteLine();
    Console.WriteLine("Updated Ticket Status:");
    Console.WriteLine("--------------------------------");

    foreach (var o in updatedOptions)
    {
        string status = o.RemainingCapacity == 0
            ? "SOLD OUT"
            : $"Left: {o.RemainingCapacity}";

        Console.WriteLine($"{o.OptionId}. {o.Name} - {o.Price} NOK ({status})");
    }
}

    private void HandleMyBookings()
    {
        Menu.ShowSectionTitle("My Bookings");

        var bookings = _bookingService.GetBookingsByUser(_currentUserId);

        if (bookings.Count == 0)
        {
            Menu.ShowMessage("You do not have any bookings yet.");
            Menu.Pause();
            return;
        }

        foreach (var booking in bookings)
        {
            Event? ev = _eventService.GetEventById(booking.EventId);
            string title = ev?.Title ?? "Unknown Event";

            Console.WriteLine($"{booking.BookingId}. {title} | {booking.Status} | {booking.BookingDate}");
        }

        Console.WriteLine();
        

        int bookingId = InputHandler.ReadInt("Enter booking id: ");
		bool confirmCancel = InputHandler.Confirm($"Cancel booking #{bookingId}?");

		if (!confirmCancel)
		{
    		Menu.ShowMessage("Cancellation aborted.");
    
    		return;
		}

        bool success = _bookingService.CancelBooking(bookingId, _currentUserId);

        if (!success)
        {
            Menu.ShowError("Booking not found or cannot be cancelled.");
            Menu.Pause();
            return;
        }

        Menu.ShowSuccess("Booking cancelled successfully.");
      
    }

    private void HandleLeaveReview()
    {
        Menu.ShowSectionTitle("Leave Review");

        var bookings = _bookingService.GetBookingsByUser(_currentUserId)
            .Where(b => b.Status == BookingStatus.Booked)
            .ToList();

        if (bookings.Count == 0)
        {
            Menu.ShowMessage("You need at least one booking to leave a review.");
            Menu.Pause();
            return;
        }

        Console.WriteLine("Your Booked Events");
        Console.WriteLine("----------------------------------------");

        foreach (var booking in bookings)
        {
            Event? ev = _eventService.GetEventById(booking.EventId);
            string title = ev?.Title ?? "Unknown Event";

            Console.WriteLine($"{booking.EventId}. {title}");
        }

        Console.WriteLine();

        int eventId = InputHandler.ReadInt("Enter event id: ");
        Event? selected = _eventService.GetEventById(eventId);

        if (selected == null)
        {
            Menu.ShowError("Event not found.");
            Menu.Pause();
            return;
        }

        bool hasBooking = bookings.Any(b => b.EventId == eventId);

        if (!hasBooking)
        {
            Menu.ShowError("You can only review events you booked.");
            Menu.Pause();
            return;
        }

        bool alreadyReviewed = _reviewService.HasUserReviewed(_currentUserId, eventId);

        if (alreadyReviewed)
        {
            Menu.ShowError("You have already reviewed this event.");
            Menu.Pause();
            return;
        }

        int rating = InputHandler.ReadIntInRange("Enter rating (1-5): ", 1, 5);
        string comment = InputHandler.ReadRequiredString("Enter comment: ");

        bool success = _reviewService.AddReview(_currentUserId, eventId, rating, comment);

        if (!success)
        {
            Menu.ShowError("Review could not be submitted. Please check your input and try again.");
            Menu.Pause();
            return;
        }

        Menu.ShowSuccess("Review submitted successfully.");
        
    }

    private void HandleLogout()
    {
        bool confirmLogout = InputHandler.Confirm("Log out?");

        if (!confirmLogout)
        {
            return;
        }

        _isLoggedIn = false;
        _currentUsername = "Guest";
        _currentUserId = 0;

        Menu.ShowSuccess("You have been logged out.");
        
    }

	private void HandleExit()
    {
        bool confirmExit = InputHandler.Confirm("Exit application?");

        if (confirmExit)
        {
            Menu.ShowMessage("Goodbye!");
            _isRunning = false;
        }
    }
	
	private EventCategory MapCategory(int categoryChoice)
	{
    	return categoryChoice switch
    	{
        	1 => EventCategory.Food,
        	2 => EventCategory.Networking,
        	3 => EventCategory.Education,
        	4 => EventCategory.Culture,
        	_ => EventCategory.Other
    	};
	}

	private EventType MapType(int typeChoice)
	{
    	return typeChoice == 1
        	? EventType.Workshop
        	: EventType.Dining;
	}

    private void ShowInvalidOption()
    {
        Menu.ShowError("Invalid option. Please try again.");
        Menu.Pause();
    }
}