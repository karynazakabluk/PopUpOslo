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
                Menu.ShowError("Invalid option. Please try again.");
                Menu.Pause();
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
                Menu.ShowError("Invalid option. Please try again.");
                Menu.Pause();
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
        Console.WriteLine("3. Filter by category");Console.WriteLine("4. Filter by type");
        Console.WriteLine("0. Back");
        Console.WriteLine();

        int choice = InputHandler.ReadInt("Choose an option: ");
        Console.WriteLine();

        List<Event> results = new();

        switch (choice)
        {
            case 1:
                results = _eventService.GetAllEvents();
                break;

            case 2:
                string keyword = InputHandler.ReadRequiredString("Enter keyword: ");
                results = _eventService.SearchEvents(keyword);
                break;

            case 3:
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

                results = _eventService.FilterByCategory(selectedCategory);
                break;

            case 4:
                Console.WriteLine("Types");
                Console.WriteLine("1. Workshop");
                Console.WriteLine("2. Dining");
                Console.WriteLine();

                int typeChoice = InputHandler.ReadIntInRange("Choose type (1-2): ", 1, 2);

                EventType selectedType = typeChoice == 1
                    ? EventType.Workshop
                    : EventType.Dining;

                results = _eventService.FilterByType(selectedType);
                break;

            case 0:
                return;

            default:
                Menu.ShowError("Invalid option.");
                Menu.Pause();
                return;
        }

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
            Console.WriteLine($"{ev.EventId}. {ev.Title} | {ev.Category} | {ev.Type} | {ev.Venue} | {ev.DateTime:g}");
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

        Menu.ShowSectionTitle("Event Details");
        Console.WriteLine($"Title: {selected.Title}");
        Console.WriteLine($"Description: {selected.Description}");
        Console.WriteLine($"Venue: {selected.Venue}");
        Console.WriteLine($"Date: {selected.DateTime:g}");
        Console.WriteLine($"Category: {selected.Category}");
        Console.WriteLine($"Type: {selected.Type}");
        Console.WriteLine($"Status: {selected.Status}");
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

        _eventService.CreateEvent(
            title,
            description,
            venue,
            dateTime,
            category,
            type,
            _currentUserId);

        Menu.ShowSuccess("Event created successfully.");
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
            Console.WriteLine($"{ev.EventId}. {ev.Title} | {ev.Category} | {ev.Type} | {ev.DateTime:g}");
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

        Menu.ShowSuccess($"Booked: {selected.Title}");
        Menu.Pause();
    }

    private void HandleMyBookings()
    {
        Menu.ShowSectionTitle("My Bookings");

        Menu.ShowMessage("Booking list will be connected later. Temporary flow only.");
        Menu.Pause();
    }

    private void HandleLeaveReview()
    {
        Menu.ShowSectionTitle("Leave Review");

        int eventId = InputHandler.ReadInt("Enter event id: ");
        int rating = InputHandler.ReadIntInRange("Enter rating (1-5): ", 1, 5);
        string comment = InputHandler.ReadRequiredString("Enter comment: ");

        Console.WriteLine();
        Console.WriteLine("Review Summary");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Event ID: {eventId}");
        Console.WriteLine($"Rating: {rating}");
        Console.WriteLine($"Comment: {comment}");
        Console.WriteLine();

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
    }

    private void HandleExit()
    {
        bool confirmExit = InputHandler.Confirm("Are you sure you want to exit");

        if (confirmExit)
        {
            Menu.ShowMessage("Goodbye!");_isRunning = false;
        }
    }
}