using SQLitePCL;
using PopUpOslo.Data;
using PopUpOslo.UI;

class Program
{
    static void Main(string[] args)
    {
        Batteries.Init();
        bool seed = args.Contains("--seed");

        DatabaseInitializer.Initialize(seed);

        Console.WriteLine("Application started...");
        
        var app = new ApplicationRunner();
        app.Run();
    }
}