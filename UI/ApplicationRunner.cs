using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;

namespace PopUpOslo.UI;

public class ApplicationRunner
{
    private bool _isRunning = true;
    private bool _isLoggedIn = false;
    private string _currentUsername = "Guest";

    private readonly List<Event> _events = new();
    private int _nextEventId = 1;

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

        Menu.ShowSuccess($"Account for '{username}' created successfully (temporary flow).");
        Menu.Pause();
    }

    private void HandleLogin()
    {
        Menu.ShowSectionTitle("Login");

        string username = InputHandler.ReadRequiredString("Enter username: ");
        string password = InputHandler.ReadPassword("Enter password: ");

        _isLoggedIn = true;
        _currentUsername = username;

        Menu.ShowSuccess($"Welcome, {username}!");
        Menu.Pause();
    }

    private void HandleBrowseEvents()
    {
        Menu.ShowSectionTitle("Browse Events");

        if (_events.Count == 0)
        {
            Menu.ShowMessage("No events available.");
            Menu.Pause();
            return;
        }

        Console.WriteLine("Available Events");
        Console.WriteLine("----------------------------------------");

        foreach (var ev in _events)
        {
            Console.WriteLine($"{ev.EventId}. {ev.Title} | {ev.Venue} | {ev.DateTime:g}");
        }

        Console.WriteLine();

        bool viewDetails = InputHandler.Confirm("Do you want to view event details");

        if (!viewDetails)
        {
            Menu.Pause();
            return;
        }

        int eventId = InputHandler.ReadInt("Enter event id: ");
        Event? selected = _events.FirstOrDefault(e => e.EventId == eventId);

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

        var ev = new Event
        {
            EventId = _nextEventId++,
            Title = title,
            Description = description,
            Venue = venue,
            DateTime = dateTime,
            Category = category,
            Type = type,
            OrganizerId = 1,
            Status = EventStatus.Upcoming
        };

        _events.Add(ev);

        Menu.ShowSuccess("Event created successfully.");
        Menu.Pause();
    }

    private void HandleViewMyEvents()
    {
        Menu.ShowSectionTitle("My Events");

        var myEvents = _events.Where(e => e.OrganizerId == 1).ToList();

        if (myEvents.Count == 0)
        {
            Menu.ShowMessage("You have not created any events yet.");
            Menu.Pause();
            return;
        }

        foreach (var ev in myEvents)
        {
            Console.WriteLine($"{ev.EventId}. {ev.Title} | {ev.Venue} | {ev.DateTime:g}");
        }

        Console.WriteLine();
        Menu.Pause();
    }

    private void HandleBookEvent()
    {
        Menu.ShowSectionTitle("Book Event");

        if (_events.Count == 0)
        {
            Menu.ShowMessage("No events available to book.");
            Menu.Pause();
            return;
        }

        foreach (var ev in _events)
        {
            Console.WriteLine($"{ev.EventId}. {ev.Title}");
        }

        Console.WriteLine();

        int eventId = InputHandler.ReadInt("Enter event id: ");
        Event? selected = _events.FirstOrDefault(e => e.EventId == eventId);

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

        Menu.ShowSuccess("You have been logged out.");
        Menu.Pause();
    }

    private void HandleExit()
    {
        bool confirmExit = InputHandler.Confirm("Are you sure you want to exit");

        if (confirmExit)
        {
            Menu.ShowMessage("Goodbye!");
            _isRunning = false;
        }
    }
}