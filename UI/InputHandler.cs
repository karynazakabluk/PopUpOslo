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