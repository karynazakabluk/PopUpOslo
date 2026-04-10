using Microsoft.Data.Sqlite;

namespace PopUpOslo.Infrastructure.Repositories;

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