using Microsoft.Data.Sqlite;

public static class DatabaseContext
{
    private static string connectionString = "Data Source=Database/database.db";

    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(connectionString);
    }
}