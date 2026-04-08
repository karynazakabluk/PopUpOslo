using SQLitePCL;
using PopUpOslo.Data;
class Program
{
    static void Main(string[] args)
    {
        
        Batteries.Init();

        DatabaseInitializer.Initialize();

        Console.WriteLine("Application started...");
    }
}