using SQLitePCL;
using PopUpOslo.Data;
using PopUpOslo.UI;

class Program
{
    static void Main(string[] args)
    {
        Batteries.Init();
        bool seed = args.Contains("--seed");
        if (seed)
        {
            Console.WriteLine(" Resetting database...");

            if (File.Exists("Database/database.db"))
            {
                File.Delete("Database/database.db");
            }
        }
        DatabaseInitializer.Initialize(seed:true);
        Console.WriteLine("Application started...");
        
        var app = new ApplicationRunner();
        app.Run();
    }
}