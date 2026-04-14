using Microsoft.Data.Sqlite;
using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;

namespace PopUpOslo.Infrastructure.Repositories;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;

public class EventRepository : BaseRepository
{
    public int AddEvent(Event ev)
{
    	using var conn = GetOpenConnection();

    	using var cmd = conn.CreateCommand();
    	cmd.CommandText = @"
            INSERT INTO Events
            (Title, Description, Category, Type, DateTime, Venue, OrganizerId, Status)
            VALUES (@t, @d, @c, @ty, @dt, @v, @o, @s);
            SELECT last_insert_rowid();";

    	cmd.Parameters.AddWithValue("@t", ev.Title);
    	cmd.Parameters.AddWithValue("@d", ev.Description);
    	cmd.Parameters.AddWithValue("@c", ev.Category.ToString());
    	cmd.Parameters.AddWithValue("@ty", ev.Type.ToString());
    	cmd.Parameters.AddWithValue("@dt", ev.DateTime.ToString("s"));
    	cmd.Parameters.AddWithValue("@v", ev.Venue);
    	cmd.Parameters.AddWithValue("@o", ev.OrganizerId);
    	cmd.Parameters.AddWithValue("@s", ev.Status.ToString());

    	return Convert.ToInt32(cmd.ExecuteScalar());
}

    public List<Event> GetAllEvents()
    {
        var list = new List<Event>();

        using var conn = GetOpenConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = "SELECT * FROM Events ORDER BY DateTime;";

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapEvent(reader));
        }

        return list;
    }

    public Event? GetEventById(int eventId)
    {
        using var conn = GetOpenConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = "SELECT * FROM Events WHERE EventId = @id;";
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

    public List<Event> GetEventsByOrganizer(int organizerId)
    {
        var list = new List<Event>();

        using var conn = GetOpenConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = "SELECT * FROM Events WHERE OrganizerId = @id ORDER BY DateTime;";
        cmd.Parameters.AddWithValue("@id", organizerId);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapEvent(reader));
        }

        return list;
    }

    
    public void CancelEvent(int eventId)
    {
        using var conn = GetOpenConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = @"
            UPDATE Events
            SET Status = 'Cancelled'
            WHERE EventId = @id;";

        cmd.Parameters.AddWithValue("@id", eventId);

        cmd.ExecuteNonQuery();
    }
	
	public void UpdateEvent(Event updatedEvent)
	{
    	using var conn = GetOpenConnection();
    	using var cmd = conn.CreateCommand();

    	cmd.CommandText = @"
            UPDATE Events
            SET Title = @t,
                Description = @d,
                Venue = @v,
                DateTime = @dt,
                Status=@s
            WHERE EventId = @id;";

    	cmd.Parameters.AddWithValue("@t", updatedEvent.Title);
    	cmd.Parameters.AddWithValue("@d", updatedEvent.Description);
    	cmd.Parameters.AddWithValue("@v", updatedEvent.Venue);
    	cmd.Parameters.AddWithValue("@dt", updatedEvent.DateTime.ToString("s"));
		cmd.Parameters.AddWithValue("@s", updatedEvent.Status.ToString());
    	cmd.Parameters.AddWithValue("@id", updatedEvent.EventId);

    	cmd.ExecuteNonQuery();
	}
	

    public List<Event> SearchEvents(string keyword)
    {
        var list = new List<Event>();

        using var conn = GetOpenConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = @"
            SELECT * FROM Events
            WHERE Title LIKE @k
               OR Description LIKE @kOR Venue LIKE @k
            ORDER BY DateTime;";

        cmd.Parameters.AddWithValue("@k", $"%{keyword}%");

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapEvent(reader));
        }

        return list;
    }

    public List<Event> FilterByCategory(string category)
    {
        var list = new List<Event>();

        using var conn = GetOpenConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = @"
            SELECT * FROM Events
            WHERE Category = @c
            ORDER BY DateTime;";

        cmd.Parameters.AddWithValue("@c", category);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapEvent(reader));
        }

        return list;
    }

    public List<Event> FilterByType(string type)
    {
        var list = new List<Event>();

        using var conn = GetOpenConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = @"
            SELECT * FROM Events
            WHERE Type = @t
            ORDER BY DateTime;";

        cmd.Parameters.AddWithValue("@t", type);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapEvent(reader));
        }

        return list;
    }

    private Event MapEvent(SqliteDataReader reader)
    {
        string categoryText = reader["Category"]?.ToString() ?? "Other";
        string typeText = reader["Type"]?.ToString() ?? "Workshop";
        string statusText = reader["Status"]?.ToString() ?? "Upcoming";
        string dateText = reader["DateTime"]?.ToString() ?? DateTime.Now.ToString("s");

        Enum.TryParse(categoryText, out EventCategory category);
        Enum.TryParse(typeText, out EventType type);
        Enum.TryParse(statusText, out EventStatus status);
        DateTime.TryParse(dateText, out DateTime dateTime);

        return new Event
        {
            EventId = Convert.ToInt32(reader["EventId"]),
            Title = reader["Title"]?.ToString() ?? string.Empty,
            Description = reader["Description"]?.ToString() ?? string.Empty,
            Category = category,
            Type = type,
            DateTime = dateTime,
            Venue = reader["Venue"]?.ToString() ?? string.Empty,
            OrganizerId = Convert.ToInt32(reader["OrganizerId"]),
            Status = status
        };
    }
}