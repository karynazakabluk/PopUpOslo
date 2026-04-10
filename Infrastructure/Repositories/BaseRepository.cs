using Microsoft.Data.Sqlite;

public abstract class BaseRepository
{
    protected SqliteConnection GetConnection()
    {
        return DatabaseContext.GetConnection();
    }

    
    protected SqliteConnection GetOpenConnection()
    {
        var conn = GetConnection();
        conn.Open();
        return conn;
    }
}