using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;

public class EventRepository : BaseRepository
{
    //  Create Event
    public void AddEvent(Event ev)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Events 
        (Title, Description, Category, Type, DateTime, Venue, OrganizerId, Status)
        VALUES (@t, @d, @c, @ty, @dt, @v, @o, @s)";

        cmd.Parameters.AddWithValue("@t", ev.Title);
        cmd.Parameters.AddWithValue("@d", ev.Description);
        cmd.Parameters.AddWithValue("@c", ev.Category);
        cmd.Parameters.AddWithValue("@ty", ev.Type);
        cmd.Parameters.AddWithValue("@dt", ev.DateTime);
        cmd.Parameters.AddWithValue("@v", ev.Venue);
        cmd.Parameters.AddWithValue("@o", ev.OrganizerId);
        cmd.Parameters.AddWithValue("@s", ev.Status);

        cmd.ExecuteNonQuery();
    }

    //  Get All Events
    public List<Event> GetAllEvents()
    {
        var list = new List<Event>();

        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Events";

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapEvent(reader));
        }

        return list;
    }

    //  Get Event By ID
    public Event GetEventById(int eventId)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Events WHERE EventId=@id";
        cmd.Parameters.AddWithValue("@id", eventId);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapEvent(reader);
        }
        else
        {
            throw new Exception("Event not found");
        }
    }

    // Edit Event
    public void UpdateEvent(Event ev)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Events SET
            Title = @t,
            Description = @d,
            Category = @c,
            Type = @ty,
            DateTime = @dt,
            Venue = @v
            WHERE EventId = @id";

        cmd.Parameters.AddWithValue("@t", ev.Title);
        cmd.Parameters.AddWithValue("@d", ev.Description);
        cmd.Parameters.AddWithValue("@c", ev.Category);
        cmd.Parameters.AddWithValue("@ty", ev.Type);
        cmd.Parameters.AddWithValue("@dt", ev.DateTime);
        cmd.Parameters.AddWithValue("@v", ev.Venue);
        cmd.Parameters.AddWithValue("@id", ev.EventId);

        cmd.ExecuteNonQuery();
    }

    // Cancel Event
    public void CancelEvent(int eventId)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Events 
                            SET Status = 'Cancelled' 
                            WHERE EventId = @id";

        cmd.Parameters.AddWithValue("@id", eventId);

        cmd.ExecuteNonQuery();
    }

    // Search Events
    public List<Event> SearchEvents(string keyword)
    {
        var list = new List<Event>();

        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Events 
                            WHERE Title LIKE @k 
                               OR Description LIKE @k 
                               OR Venue LIKE @k";

        cmd.Parameters.AddWithValue("@k", $"%{keyword}%");

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapEvent(reader));
        }

        return list;
    }

    // Filter Events
    public List<Event> FilterEvents(string category, string type)
    {
        var list = new List<Event>();

        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Events 
                            WHERE Category = @c AND Type = @t";

        cmd.Parameters.AddWithValue("@c", category);
        cmd.Parameters.AddWithValue("@t", type);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapEvent(reader));
        }

        return list;
    }

    // Private Mapper (VERY CLEAN APPROACH)
    private Event MapEvent(SqliteDataReader reader)
    {
        var dateString = reader.GetString(5);
        return new Event
        {
            EventId = reader.GetInt32(0),
            Title = reader.GetString(1),
            Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
            Category = reader.IsDBNull(3)
                ? default
                : Enum.Parse<EventCategory>(reader.GetString(3)),
            Type = reader.IsDBNull(4)
                ? default
                : Enum.TryParse<EventType>(
                    reader.GetString(4),
                    out var type)
                    ? type
                    : default,
            

            DateTime = DateTime.TryParse(dateString, out var dt)
            ? dt
            : default,
            Venue = reader.GetString(6),
            OrganizerId = reader.GetInt32(7),
            Status = Enum.Parse<EventStatus>(reader.GetString(8))
        };
    }
}