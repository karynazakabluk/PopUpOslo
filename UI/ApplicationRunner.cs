using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;
using PopUpOslo.Services;
using System.Linq;


namespace PopUpOslo.UI;

public class ApplicationRunner
{
    private bool _isRunning = true;
    private bool _isLoggedIn = false;

    private User? _currentUser;

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
            if (!_isLoggedIn)
            {
                RunGuestMenu();
            }
            else
            {
                switch (_currentUser!.Role)
                {
                    case UserRole.Admin:
                        RunAdminMenu();
                        break;
                    case UserRole.Organizer:
                        RunOrganizerMenu();
                        break;
                    default:
                        RunUserMenu();
                        break;
                }
            }
        }
    }

    // =========================
    // MENUS
    // =========================

    private void RunGuestMenu()
    {
        Menu.ShowWelcome();
        Menu.ShowMainMenu();

        int choice = InputHandler.ReadInt("Choose an option: ");

        switch (choice)
        {
            case 1: HandleRegister(); break;
            case 2: HandleLogin(); break;
            case 3: HandleBrowseEvents(); break;
            case 0: HandleExit(); break;
            default: ShowInvalidOption(); break;
        }
    }

    private void RunUserMenu()
    {
        Menu.ShowSectionTitle($"User Menu - {_currentUser!.Username}");

        Console.WriteLine("1. Browse Events");
        Console.WriteLine("2. Book Event");
        Console.WriteLine("3. My Bookings");
        Console.WriteLine("4. Leave Review");
        Console.WriteLine("0. Logout");

        int choice = InputHandler.ReadInt("Choose: ");

        switch (choice)
        {
            case 1: HandleBrowseEvents(); break;
            case 2: HandleBookEvent(); break;
            case 3: HandleMyBookings(); break;
            case 4: HandleLeaveReview(); break;
            case 0: HandleLogout(); break;
            default: ShowInvalidOption(); break;
        }
    }

    private void RunOrganizerMenu()
    {
        Menu.ShowSectionTitle($"Organizer Menu - {_currentUser!.Username}");

        Console.WriteLine("1. Create Event");
        Console.WriteLine("2. My Events");
        Console.WriteLine("3. Browse Events");
        Console.WriteLine("4. Book Event");
        Console.WriteLine("0. Logout");

        int choice = InputHandler.ReadInt("Choose: ");

        switch (choice)
        {
            case 1: HandleCreateEvent(); break;
            case 2: HandleViewMyEvents(); break;
            case 3: HandleBrowseEvents(); break;
            case 4: HandleBookEvent(); break;
            case 0: HandleLogout(); break;
            default: ShowInvalidOption(); break;
        }
    }

    private void RunAdminMenu()
    {
        Menu.ShowSectionTitle($"Admin Menu - {_currentUser!.Username}");

        Console.WriteLine("1. Create Event");
        Console.WriteLine("2. View All Events");
        Console.WriteLine("3. Edit Event");
        Console.WriteLine("4. Change Status");
        Console.WriteLine("5. Book Event");
        Console.WriteLine("0. Logout");

        int choice = InputHandler.ReadInt("Choose: ");

        switch (choice)
        {
            case 1: HandleCreateEvent(); break;
            case 2: HandleViewAllEventsAdmin(); break;
            case 3: HandleEditEvent(); break;
            case 4: HandleChangeEventStatus(); break;
            case 5: HandleBookEvent(); break;
            case 0: HandleLogout(); break;
            default: ShowInvalidOption(); break;
        }
    }

    // =========================
    // AUTH
    // =========================

    private void HandleRegister()
    {
        string username = InputHandler.ReadRequiredString("Username: ");
        string password = InputHandler.ReadPassword("Password: ");

        Console.WriteLine("Select Role:");
        Console.WriteLine("1. User");
        Console.WriteLine("2. Organizer");
        Console.WriteLine("3. Admin");
        int roleChoice = InputHandler.ReadIntInRange("Choose role: ", 1, 3);

        UserRole role = (UserRole)(roleChoice - 1);
        bool success = _authService.Register(username, password, role);

        if (!success)
        {
            Menu.ShowError("Registration failed.");
            return;
        }

        Menu.ShowSuccess("User registered!");
    }

    private void HandleLogin()
    {
        string username = InputHandler.ReadRequiredString("Username: ");
        string password = InputHandler.ReadPassword("Password: ");

        var user = _authService.Login(username, password);

        if (user == null)
        {
            Menu.ShowError("Invalid login.");
            return;
        }

        _currentUser = user;
        _isLoggedIn = true;

        Menu.ShowSuccess($"Welcome {user.Username} ({user.Role})");
    }

    private void HandleLogout()
    {
        _isLoggedIn = false;
        _currentUser = null;
    }

    // =========================
    // EVENTS
    // =========================

    private void HandleCreateEvent()
    {
        string title = InputHandler.ReadRequiredString("Title: ");
        string desc = InputHandler.ReadRequiredString("Description: ");
        string venue = InputHandler.ReadRequiredString("Venue: ");
        DateTime date = InputHandler.ReadDateTime("Date (yyyy-MM-dd HH:mm): ");

        int id = _eventService.CreateEvent(
            title, desc, venue, date,
            EventCategory.Food,
            EventType.Workshop,
            _currentUser!.UserId
        );

        _bookingOptionService.CreateDefaultOption(id);

        Menu.ShowSuccess("Event created.");
    }

    private void HandleViewMyEvents()
    {
        var events = _eventService.GetEventsByOrganizer(_currentUser!.UserId);

        foreach (var ev in events)
        {
            Console.WriteLine($"{ev.EventId} - {ev.Title}");
        }
    }

   private void HandleViewAllEventsAdmin()
   {
    	var events = _eventService.GetAllEvents();

    	Console.WriteLine("\n===== ALL EVENTS =====\n");

    	foreach (var ev in events)
    	{
        	Console.WriteLine($"{ev.EventId} - {ev.Title} ({ev.Category}) - {ev.Status}");
    	}

    	Console.WriteLine("\nPress ENTER to continue...");
    	Console.ReadLine();
   }

    private void HandleEditEvent()
    {
        int id = InputHandler.ReadInt("Event ID: ");
        var ev = _eventService.GetEventById(id);

        if (ev == null) return;

        ev.Title = InputHandler.ReadOptionalString("New title: ") ?? ev.Title;

        _eventService.UpdateEvent(ev);
    }

    private void HandleChangeEventStatus()
    {
        int id = InputHandler.ReadInt("Event ID: ");
        var ev = _eventService.GetEventById(id);

        if (ev == null) return;

        ev.Status = EventStatus.Cancelled;
        _eventService.UpdateEvent(ev);
    }

    // =========================
    // BOOKING
    // =========================

    private void HandleBookEvent()
    {
        var events = _eventService.GetAllEvents();

        foreach (var ev in events)
            Console.WriteLine($"{ev.EventId}. {ev.Title}");

        int eventId = InputHandler.ReadInt("Select event: ");

        var options = _bookingOptionService.GetOptionsByEvent(eventId);

        foreach (var o in options)
            Console.WriteLine($"{o.OptionId}. {o.Name} - {o.Price} NOK");

        int optId = InputHandler.ReadInt("Select option: ");

        _bookingService.CreateBooking(_currentUser!.UserId, eventId, optId);

        Menu.ShowSuccess("Booked!");
    }

    private void HandleMyBookings()
    {
        var bookings = _bookingService.GetBookingsByUser(_currentUser!.UserId);

        foreach (var b in bookings)
            Console.WriteLine($"Booking {b.BookingId} - Event {b.EventId}");
    }

    private void HandleLeaveReview()
    {
        int eventId = InputHandler.ReadInt("Event ID: ");
        int rating = InputHandler.ReadIntInRange("Rating (1-5): ", 1, 5);
        string comment = InputHandler.ReadRequiredString("Comment: ");

        _reviewService.AddReview(_currentUser!.UserId, eventId, rating, comment);
    }

    private void HandleBrowseEvents()
    {
        var events = _eventService.GetAllEvents();

        foreach (var ev in events)
            Console.WriteLine($"{ev.EventId} - {ev.Title}");
    }

    private void HandleExit()
    {
        _isRunning = false;
    }

    private void ShowInvalidOption()
    {
        Console.WriteLine("Invalid option.");
    }
}