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
        Menu.Pause();
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
        Menu.Pause();
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
            Menu.ShowError("Invalid option.");
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

        EventCategory selectedCategory = categoryChoice switch
        {
            1 => EventCategory.Food,
            2 => EventCategory.Networking,
            3 => EventCategory.Education,
            4 => EventCategory.Culture,
            _ => EventCategory.Other
        };

        return _searchService.FilterByCategory(allEvents, selectedCategory);
    }

    private List<Event> HandleTypeFilter(List<Event> allEvents)
    {
        Console.WriteLine("Types");
        Console.WriteLine("1. Workshop");
        Console.WriteLine("2. Dining");
        Console.WriteLine();

        int typeChoice = InputHandler.ReadIntInRange("Choose type (1-2): ", 1, 2);

        EventType selectedType = typeChoice == 1
            ? EventType.Workshop
            : EventType.Dining;

        return _searchService.FilterByType(allEvents, selectedType);
    }

    private void DisplayEventResults(List<Event> results)
    {
        Console.WriteLine("Results");
        Console.WriteLine("----------------------------------------");

        if (results.Count == 0)
        {
            Menu.ShowMessage("No matching events found.");
            Menu.Pause();
            return;
        }

        foreach (var ev in results)
        {
            double avgRating = _reviewService.GetAverageRating(ev.EventId);

            Console.WriteLine($"{ev.EventId}. {ev.Title} | {ev.Category} | {ev.Type} | {ev.Venue} | {ev.DateTime:g}");

            if (avgRating > 0)
            {
                Console.WriteLine($"   Average rating: {avgRating:F1}/5");
            }
        }

        Console.WriteLine();

        bool viewDetails = InputHandler.Confirm("Do you want to view event details");

        if (!viewDetails)
        {
            Menu.Pause();
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
        if (avgRating > 0)
        {
            Console.WriteLine($"Average rating: {avgRating:F1}/5");
        }

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

        EventCategory category = categoryChoice switch
        {
            1 => EventCategory.Food,
            2 => EventCategory.Networking,
            3 => EventCategory.Education,
            4 => EventCategory.Culture,
            _ => EventCategory.Other
        };

        EventType type = typeChoice == 1 ? EventType.Workshop : EventType.Dining;

        int eventId = _eventService.CreateEvent(
    		title,
    		description,
    		venue,
    		dateTime,
    		category,
    		type,
    		_currentUserId);

		_bookingOptionService.CreateDefaultOption(eventId);

		Menu.ShowSuccess("Event created successfully");
        Menu.Pause();
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

            Console.WriteLine($"{ev.EventId}. {ev.Title} | {ev.Category} | {ev.Type} | {ev.DateTime:g}");

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
        Event? selected = _eventService.GetEventById(eventId);

        if (selected == null)
        {
            Menu.ShowError("Event not found.");
            Menu.Pause();
            return;
        }

        bool success = _bookingService.CreateBooking(_currentUserId, selected);

        if (!success)
        {
            Menu.ShowError("You already booked this event.");
            Menu.Pause();
            return;
        }

        Menu.ShowSuccess($"Booked: {selected.Title}");
        Menu.Pause();
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

        bool cancelBooking = InputHandler.Confirm("Do you want to cancel a booking");

        if (!cancelBooking)
        {
            Menu.Pause();
            return;
        }

        int bookingId = InputHandler.ReadInt("Enter booking id: ");
        bool success = _bookingService.CancelBooking(bookingId, _currentUserId);

        if (!success)
        {
            Menu.ShowError("Booking not found or cannot be cancelled.");
            Menu.Pause();
            return;
        }

        Menu.ShowSuccess("Booking cancelled successfully.");
        Menu.Pause();
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
            Menu.ShowError("Review could not be submitted.");
            Menu.Pause();
            return;
        }

        Menu.ShowSuccess("Review submitted successfully.");
        Menu.Pause();
    }

    private void HandleLogout()
    {
        bool confirmLogout = InputHandler.Confirm("Are you sure you want to log out");

        if (!confirmLogout)
        {
            return;
        }

        _isLoggedIn = false;
        _currentUsername = "Guest";
        _currentUserId = 0;

        Menu.ShowSuccess("You have been logged out.");
        Menu.Pause();
    }private void HandleExit()
    {
        bool confirmExit = InputHandler.Confirm("Are you sure you want to exit");

        if (confirmExit)
        {
            Menu.ShowMessage("Goodbye!");
            _isRunning = false;
        }
    }

    private void ShowInvalidOption()
    {
        Menu.ShowError("Invalid option. Please try again.");
        Menu.Pause();
    }
}