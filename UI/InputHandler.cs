namespace PopUpOslo.UI;

public static class InputHandler
{
    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int result))
            {
                return result;
            }

            Console.WriteLine("Invalid number. Please enter a valid integer.");
            Console.WriteLine();
        }
    }

    public static int ReadIntInRange(string prompt, int min, int max)
    {
        while (true)
        {
            int value = ReadInt(prompt);

            if (value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine($"Please enter a number between {min} and {max}.");
            Console.WriteLine();
        }
    }

    public static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (double.TryParse(input, out double result))
            {
                return result;
            }

            Console.WriteLine("Invalid number. Please enter a valid numeric value.");
            Console.WriteLine();
        }
    }

    public static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            Console.WriteLine("This field cannot be empty.");
            Console.WriteLine();
        }
    }

    public static string ReadPassword(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            Console.WriteLine("Password cannot be empty.");
            Console.WriteLine();
        }
    }

    public static DateTime ReadDateTime(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (DateTime.TryParse(input, out DateTime result))
            {
                return result;
            }

            Console.WriteLine("Invalid date/time. Example: 2026-05-10 18:30");
            Console.WriteLine();
        }
    }
    
    public static string ReadOptionalString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    public static DateTime? ReadOptionalDateTime(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        while (!DateTime.TryParse(input, out DateTime parsedDate))
        {
            Console.Write("Invalid date format. Try again or press Enter to keep current value: ");
            input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
        }

        return DateTime.Parse(input);
    }
    

    public static bool Confirm(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt} (y/n): ");
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (input == "y" || input == "yes")
            {
                return true;
            }

            if (input == "n" || input == "no")
            {
                return false;
            }

            Console.WriteLine("Please enter 'y' or 'n'.");
            Console.WriteLine();
        }
    }
}