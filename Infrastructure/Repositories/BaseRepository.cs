using System.Data.SQLite;

public abstract class BaseRepository
{
    protected SQLiteConnection GetConnection()
    {
        return DatabaseContext.GetConnection();
    }

    
    protected SQLiteConnection GetOpenConnection()
    {
        var conn = GetConnection();
        conn.Open();
        return conn;
    }
}