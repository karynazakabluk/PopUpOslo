namespace PopUpOslo.UI;

public class ApplicationRunner
{
    private bool _isRunning = true;
    private bool _isLoggedIn = false;
    private string _currentUsername = "Guest";

    public void Run()
    {
        while (_isRunning)
        {
            if (!_isLoggedIn)
            {
                RunMainMenu();
            }
            else
            {
                RunUserMenu();
            }
        }
    }

    private void RunMainMenu()
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
        string password = InputHandler.ReadRequiredString("Enter password: ");

        // TODO: connect AuthService later
        Menu.ShowSuccess($"User '{username}' registered successfully (temporary demo).");
        Menu.Pause();
    }

    private void HandleLogin()
    {
        Menu.ShowSectionTitle("Login");

        string username = InputHandler.ReadRequiredString("Enter username: ");
        string password = InputHandler.ReadRequiredString("Enter password: ");

        // TODO: connect AuthService later
        _isLoggedIn = true;
        _currentUsername = username;

        Menu.ShowSuccess($"Welcome, {username}!");
        Menu.Pause();
    }

    private void HandleBrowseEvents()
    {
        Menu.ShowSectionTitle("Browse Events");

        // TODO: connect EventService later
        Menu.ShowMessage("No events available yet.");
        Menu.Pause();
    }

    private void HandleCreateEvent()
    {
        Menu.ShowSectionTitle("Create Event");

        // TODO: connect EventService later
        Menu.ShowMessage("Create Event feature will be implemented later.");
        Menu.Pause();
    }

    private void HandleViewMyEvents()
    {
        Menu.ShowSectionTitle("My Events");

        // TODO: connect EventService later
        Menu.ShowMessage("You have not created any events yet.");
        Menu.Pause();
    }

    private void HandleBookEvent()
    {
        Menu.ShowSectionTitle("Book Event");

        // TODO: connect BookingService later
        Menu.ShowMessage("Book Event feature will be implemented later.");
        Menu.Pause();
    }

    private void HandleMyBookings()
    {
        Menu.ShowSectionTitle("My Bookings");

        // TODO: connect BookingService later
        Menu.ShowMessage("No bookings found.");
        Menu.Pause();
    }

    private void HandleLeaveReview()
    {
        Menu.ShowSectionTitle("Leave Review");

        // TODO: connect ReviewService later
        Menu.ShowMessage("Leave Review feature will be implemented later.");
        Menu.Pause();
    }

    private void HandleLogout()
    {
        _isLoggedIn = false;
        _currentUsername = "Guest";

        Menu.ShowSuccess("You have been logged out.");
        Menu.Pause();
    }

    private void HandleExit()
    {
        bool confirmExit = InputHandler.Confirm("Are you sure you want to exit?");

        if (confirmExit)
        {
            Menu.ShowMessage("Goodbye!");
            _isRunning = false;
        }
    }
}