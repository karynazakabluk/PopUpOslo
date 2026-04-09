namespace PopUpOslo.UI;

public static class Menu
{
    public static void ShowWelcome()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("            Welcome to PopUpOslo        ");
        Console.WriteLine("========================================");
        Console.WriteLine("A console-based pop-up event platform");
        Console.WriteLine();
    }

    public static void ShowMainMenu()
    {
        Console.WriteLine("Main Menu");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Login");
        Console.WriteLine("3. Browse Events");
        Console.WriteLine("0. Exit");
        Console.WriteLine();
    }

    public static void ShowUserMenu(string username)
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine($" Logged in as: {username}");
        Console.WriteLine("========================================");
        Console.WriteLine("User Menu");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1. Create Event");
        Console.WriteLine("2. View My Events");
        Console.WriteLine("3. Browse Events");
        Console.WriteLine("4. Book Event");
        Console.WriteLine("5. My Bookings");
        Console.WriteLine("6. Leave Review");
        Console.WriteLine("0. Logout");
        Console.WriteLine();
    }

    public static void ShowSectionTitle(string title)
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine($" {title}");
        Console.WriteLine("========================================");
        Console.WriteLine();
    }

    public static void ShowMessage(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine();
    }

    public static void ShowSuccess(string message)
    {
        Console.WriteLine($"SUCCESS: {message}");
        Console.WriteLine();
    }

    public static void ShowError(string message)
    {
        Console.WriteLine($"ERROR: {message}");
        Console.WriteLine();
    }

    public static void Pause()
    {
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}