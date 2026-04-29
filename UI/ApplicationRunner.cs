using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;
using PopUpOslo.Services;
using System.Linq;

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
            	if (IsAdmin())
            	{
                	RunAdminMenu();
            	}
            	else
            	{
                	RunUserMenu();
            	}
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
	private void RunAdminMenu()
	{
    	Menu.ShowSectionTitle($"Admin Menu - {_currentUsername}");

    	Console.WriteLine("1. Create event");
		Console.WriteLine("2. View my events");
		Console.WriteLine("3. Browse events");
        Console.WriteLine("4. Book event");
		Console.WriteLine("5. View all events");
		Console.WriteLine("6. View event details");
		Console.WriteLine("7. Edit event");
		Console.WriteLine("8. Change event status");
		Console.WriteLine("9. View events by status");
		Console.WriteLine("10. My bookings");
		Console.WriteLine("0. Log out");
    	Console.WriteLine();

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
        		HandleViewAllEventsAdmin();
        		break;
			case 6:
        		HandleViewEventDetailsAdmin();
        		break;
			case 7:
        		HandleEditEvent();
        		break;
    		case 8:
        		HandleChangeEventStatus();
        		break;
			case 9:
        		HandleViewEventsByStatus();
        		break;
			case 10:
        		HandleMyBookings();
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
		Console.WriteLine();
  
    }

   	private void HandleLogin()
	{
    	while (true)
    	{
        	Console.Clear();
			Menu.ShowSectionTitle("Login");

        	string username = InputHandler.ReadRequiredString("Enter username: ");
        	string password = InputHandler.ReadPassword("Enter password: ");

        	User? user = _authService.Login(username, password);

        	if (user != null)
        	{
            	_isLoggedIn = true;
            	_currentUsername = user.Username;
            	_currentUserId = user.UserId;

            	Menu.ShowSuccess($"Welcome, {user.Username}!");
            	Console.WriteLine();
            	return;
        	}

        	Menu.ShowError("Invalid username or password.");

        	bool tryAgain = InputHandler.Confirm("Try again?");

        	if (!tryAgain)
        	{
            	return;
        	}

        	Console.WriteLine();
    	}
	}
    private void HandleBrowseEvents()
	{
    	while (true)
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

        	if (choice == 0)
        	{
            	return;
        	}

        	if (choice < 0 || choice > 4)
        	{
            	ShowInvalidOption();
            	continue;
        	}

        	List<Event> results = choice switch
        	{
            	1 => _eventService.GetAllEvents(),
            	2 => HandleKeywordSearch(allEvents),
            	3 => HandleCategoryFilter(allEvents),
            	4 => HandleTypeFilter(allEvents),
            	_ => new List<Event>()
        	};

        	DisplayEventResults(results);
    	}
	}

    private List<Event> HandleKeywordSearch(List<Event> allEvents)
    {
        string keyword = InputHandler.ReadRequiredString("Enter keyword to search: ");
        return _searchService.SearchEvents(allEvents, keyword);
    }

    private List<Event> HandleCategoryFilter(List<Event> allEvents)
    {
        Console.WriteLine("Categories");
		Console.WriteLine("----------------------------------------");
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
		Console.WriteLine("--------");
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
        Console.WriteLine("------------");

        if (results.Count == 0)
        {
            Menu.ShowMessage("No matching events found.");
			Menu.Pause();
          
            return;
        }
		Console.WriteLine("ID   | Title                          | Category    | Type       | Venue              | Date             | Status");
		Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
        foreach (var ev in results)
        {
            double avgRating = _reviewService.GetAverageRating(ev.EventId);

        Console.WriteLine(
    $"{ev.EventId,-4} | {ev.Title,-30} | {ev.Category,-11} | {ev.Type,-10} | {ev.Venue,-18} | {ev.DateTime,-16:g} | {ev.Status,-10}");
        
        }

        Console.WriteLine();
        

        int eventId = InputHandler.ReadInt("Enter event ID to view details (0 to go back): ");

		if (eventId == 0)
		{
    		return;
		}
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

    	Console.WriteLine("ID   | Title                          | Category    | Type       | Venue              | Date             | Status");
    	Console.WriteLine("------------------------------------------------------------------------------------------------------------------");

    	foreach (var ev in myEvents)
    	{
        	Console.WriteLine(
            	$"{ev.EventId,-4} | {ev.Title,-30} | {ev.Category,-11} | {ev.Type,-10} | {ev.Venue,-18} | {ev.DateTime,-16:g} | {ev.Status,-10}");
    	}

    	Console.WriteLine();

    	int eventId = InputHandler.ReadInt("Enter event ID to view details (0 to go back): ");

    	if (eventId == 0)
    	{
        	return;
    	}

    	Event? selected = _eventService.GetEventById(eventId);

    	if (selected == null)
    	{
        	Menu.ShowError("Event not found.");
        	Menu.Pause();
        	return;
    	}

    	DisplayEventDetails(selected);
	}



	private void HandleViewEventDetailsAdmin()
	{
    	Menu.ShowSectionTitle("Event Details");

    	var events = _eventService.GetAllEvents();

    	if (events.Count == 0)
    	{
        	Menu.ShowMessage("No events available.");
        	Menu.Pause();
        	return;
    	}
		Console.WriteLine("ID   | Title                          | Category   | Type       | Date             | Organizer | Status");
		Console.WriteLine("---------------------------------------------------------------------------------------------------");
    	foreach (var ev in events)
    	{
        	Console.WriteLine(
    $"{ev.EventId,-4} | {ev.Title,-30} | {ev.Category,-10} | {ev.Type,-10} | {ev.DateTime,-16:g} | {ev.OrganizerId,-9} | {ev.Status,-10}");
    	}

    	Console.WriteLine();

    	int eventId = InputHandler.ReadInt("Enter event ID to view details (0 to go back): ");

		if (eventId == 0)
		{
    		return;
		}
    	Event? selected = _eventService.GetEventById(eventId);

    	if (selected == null)
    	{
        	Menu.ShowError("Event not found.");
        	Menu.Pause();
        	return;
    	}

    	DisplayEventDetails(selected);
	}

	private void HandleEditEvent()
	{
    	Menu.ShowSectionTitle("Edit Event");

    	var events = _eventService.GetAllEvents();

    	if (events.Count == 0)
    	{
        	Menu.ShowMessage("No events available.");
        	Menu.Pause();
        	return;
    	}

    	Console.WriteLine("ID   | Title                          | Date             | Status");
		Console.WriteLine("--------------------------------------------------------------------------------");

		foreach (var ev in events)
		{
    		Console.WriteLine(
        		$"{ev.EventId,-4} | {ev.Title,-30} | {ev.DateTime,-16:g} | {ev.Status,-10}");
		}

		Console.WriteLine();

    	int eventId = InputHandler.ReadInt("Enter event ID to edit (0 to go back): ");

		if (eventId == 0)
		{
    		return;
		}
    	Event? selected = _eventService.GetEventById(eventId);

    	if (selected == null)
    	{
        	Menu.ShowError("Event not found.");
        	Menu.Pause();
        	return;
    	}

		Console.WriteLine("Leave fields empty to keep current values.");
		Console.WriteLine();
    	
    	string newTitle = InputHandler.ReadOptionalString($"Title ({selected.Title}): ");
    	string newDescription = InputHandler.ReadOptionalString($"Description ({selected.Description}): ");
    	string newVenue = InputHandler.ReadOptionalString($"Venue ({selected.Venue}): ");

    	DateTime? newDate = InputHandler.ReadOptionalDateTime(
    $"Date and time ({selected.DateTime:g}, e.g. 2026-07-15 18:00): ");

  
    	selected.Title = string.IsNullOrWhiteSpace(newTitle) ? selected.Title : newTitle;
    	selected.Description = string.IsNullOrWhiteSpace(newDescription) ? selected.Description : newDescription;
    	selected.Venue = string.IsNullOrWhiteSpace(newVenue) ? selected.Venue : newVenue;
    	selected.DateTime = newDate ?? selected.DateTime;

    	bool confirm = InputHandler.Confirm("Save changes?");

    	if (!confirm)
    	{
        	Menu.ShowMessage("Edit cancelled.");
			
        	return;
    	}

    	_eventService.UpdateEvent(selected);

    	Menu.ShowSuccess("Event updated successfully.");
		Menu.Pause();
	}
	private void HandleChangeEventStatus()
	{
    	Menu.ShowSectionTitle("Change Event Status");

    	var events = _eventService.GetAllEvents();

    	if (events.Count == 0)
    	{
        	Menu.ShowMessage("No events available.");
        	Menu.Pause();
        	return;
    	}

    	Console.WriteLine("ID   | Title                          | Date             | Organizer | Status");
		Console.WriteLine("--------------------------------------------------------------------------------");

		foreach (var ev in events)
		{
    		Console.WriteLine(
        		$"{ev.EventId,-4} | {ev.Title,-30} | {ev.DateTime,-16:g} | {ev.OrganizerId,-9} | {ev.Status,-10}");
		}

		Console.WriteLine();

    	int eventId = InputHandler.ReadInt("Enter event ID to update status (0 to go back): ");

		if (eventId == 0)
		{
    		return;
		}
    	Event? selected = _eventService.GetEventById(eventId);

    	if (selected == null)
    	{
        	Menu.ShowError("Event not found.");
        	Menu.Pause();
        	return;
    	}

    	Console.WriteLine("Choose new status:");
    	Console.WriteLine("1. Upcoming");
    	Console.WriteLine("2. Cancelled");
    	Console.WriteLine();

    	int statusChoice = InputHandler.ReadIntInRange("Choose status (1-2): ", 1, 2);

    	EventStatus newStatus = statusChoice == 1
        	? EventStatus.Upcoming
        	: EventStatus.Cancelled;

    	if (selected.Status == newStatus)
    	{
        	Menu.ShowMessage("This event already has that status.");
        	Menu.Pause();
        	return;
    	}

    	bool confirm = InputHandler.Confirm(
        	$"Change status of '{selected.Title}' to {newStatus}?");

    	if (!confirm)
    	{
        	Menu.ShowMessage("Status update aborted.");
        	return;
    	}

    	selected.Status = newStatus;
    	_eventService.UpdateEvent(selected);

    	Menu.ShowSuccess("Event status updated successfully.");
	}

	private void HandleViewEventsByStatus()
	{
    	Menu.ShowSectionTitle("Filter Events by Status");

    	Console.WriteLine("1. Upcoming");
    	Console.WriteLine("2. Cancelled");
    	Console.WriteLine();

    	int choice = InputHandler.ReadIntInRange("Choose status: ", 1, 2);

    	EventStatus selectedStatus = choice == 1
        	? EventStatus.Upcoming
        	: EventStatus.Cancelled;

    	var events = _eventService.GetAllEvents()
        	.Where(e => e.Status == selectedStatus)
        	.ToList();

    	if (events.Count == 0)
    	{
        	Menu.ShowMessage("No events with this status.");
        	Menu.Pause();
        	return;
    	}

    	Console.WriteLine("ID   | Title                          | Date             | Status");
		Console.WriteLine("------------------------------------------------------------------");

		foreach (var ev in events)
		{
    		Console.WriteLine(
        		$"{ev.EventId,-4} | {ev.Title,-30} | {ev.DateTime,-16:g} | {ev.Status,-10}");
		}

		Console.WriteLine();
		Menu.Pause();
			}

	private void HandleViewAllEventsAdmin()
	{
    	Menu.ShowSectionTitle("All Events");

    	var events = _eventService.GetAllEvents();

    	if (events.Count == 0)
    	{
        	Menu.ShowMessage("No events available.");
        	Menu.Pause();
        	return;
    	}
		Console.WriteLine("ID   | Title                          | Category    | Type       | Date             | Organizer | Status");
		Console.WriteLine("------------------------------------------------------------------------------------------------------------");
    	foreach (var ev in events)
    	{
        	Console.WriteLine(
    	$"{ev.EventId,-4} | {ev.Title,-30} | {ev.Category,-11} | {ev.Type,-10} | {ev.DateTime,-16:g} | {ev.OrganizerId,-9} | {ev.Status,-10}");
		}
		
    	Console.WriteLine();

		int eventId = InputHandler.ReadInt("Enter event ID to view details (0 to go back): ");

		if (eventId == 0)
		{
    		return;
		}

		Event? selected = _eventService.GetEventById(eventId);

		if (selected == null)
		{
    		Menu.ShowError("Event not found.");
    		Menu.Pause();
    		return;
		}

		DisplayEventDetails(selected);
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

    // Create index mapping (LOGICAL ID)
    var eventList = events.ToList();

    Console.WriteLine("No   | Title                          | Date             | Venue");
    Console.WriteLine("------------------------------------------------------------------");

    for (int i = 0; i < eventList.Count; i++)
    {
        var ev = eventList[i];

        Console.WriteLine(
            $"{i + 1,-4} | {ev.Title,-30} | {ev.DateTime,-16:g} | {ev.Venue,-15}");
    }

    Console.WriteLine();

    int selectedIndex = InputHandler.ReadInt("Select event number (0 to go back): ");

    if (selectedIndex == 0)
        return;

    if (selectedIndex < 1 || selectedIndex > eventList.Count)
    {
        Menu.ShowError("Invalid selection.");
        Menu.Pause();
        return;
    }

    // Convert logical index → real DB event
    var selectedEvent = eventList[selectedIndex - 1];
    int eventId = selectedEvent.EventId;

    // Fetch latest event from DB (optional but safe)
    selectedEvent = _eventService.GetEventById(eventId);

    if (selectedEvent == null)
    {
        Menu.ShowError("Event not found.");
        Menu.Pause();
        return;
    }

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

    // show with logical index
	for (int i = 0; i < options.Count; i++)
	{
    	var o = options[i];

    	string status = o.RemainingCapacity > 0
        ? $"Left: {o.RemainingCapacity}"
        : "SOLD OUT";

    	Console.WriteLine($"{i + 1}. {o.Name} - {o.Price} NOK ({status})");
	}

	int optionIndex = InputHandler.ReadInt("Select ticket option (0 to cancel): ");

	if (optionIndex == 0)
    return;

	if (optionIndex < 1 || optionIndex > options.Count)
	{
    	Menu.ShowError("Invalid ticket option.");
    	Menu.Pause();
    	return;
	}

	var selectedOption = options[optionIndex - 1];
	int selectedOptionId = selectedOption.OptionId;

    if (selectedOption == null)
    {
        Menu.ShowError("Invalid ticket option.");
        Menu.Pause();
        return;
    }

    if (selectedOption.RemainingCapacity <= 0)
    {
        Menu.ShowError("This ticket type is SOLD OUT.");
        Menu.Pause();
        return;
    }

    Console.WriteLine($"You selected: {selectedOption.Name} - {selectedOption.Price} NOK");

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

    Menu.ShowSuccess($"Booked: {selectedEvent.Title} ({selectedOption.Name})");

    // refresh
    options = _bookingOptionService.GetOptionsByEvent(eventId);

    Console.WriteLine();
    Console.WriteLine("Updated Ticket Availability:");
    Console.WriteLine("--------------------------------");

    foreach (var o in options)
    {
        Console.WriteLine($"{o.Name} - {o.Price} NOK (Left: {o.RemainingCapacity})");
    }

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

        Console.WriteLine("Booking Refno.| Event Title                    | Date             | Status      | Venue");
		Console.WriteLine("--------------------------------------------------------------------------------");

		foreach (var booking in bookings)
		{
    		Event? ev = _eventService.GetEventById(booking.EventId);
    		string title = ev?.Title ?? "Unknown Event";
    		string venue = ev?.Venue ?? "Unknown Venue";
    		string eventDate = ev != null ? ev.DateTime.ToString("g") : "Unknown Date";

    		Console.WriteLine(
        		$"{booking.BookingId,-13} | {title,-30} | {eventDate,-16:g} | {booking.Status,-11} | {venue,-15}");
		}

		Console.WriteLine();
        

        int bookingId = InputHandler.ReadInt("Enter booking ID to cancel (0 to go back): ");

		if (bookingId == 0)
		{
    		return;
		}
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

        int eventId = InputHandler.ReadInt("Enter event ID to review (0 to go back): ");

		if (eventId == 0)
		{
    		return;
		}

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
	private bool IsAdmin()
	{
    	return _currentUserId == 1;
	}


    private void ShowInvalidOption()
    {
        Menu.ShowError("Invalid option. Please try again.");
        Menu.Pause();
    }
	
}