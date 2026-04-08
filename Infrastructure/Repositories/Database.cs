using System.Data.SQLite;

public static class Database
{
    private static string connectionString = "Data Source=Database/database.db";

    public static SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(connectionString);
    }
}