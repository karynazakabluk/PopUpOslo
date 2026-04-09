using SQLitePCL;
using PopUpOslo.Data;
using PopUpOslo.UI;

class Program
{
    static void Main(string[] args)
    {
        Batteries.Init();

        DatabaseInitializer.Initialize();

        Console.WriteLine("Application started...");
        
        var app = new ApplicationRunner();
        app.Run();
    }
}