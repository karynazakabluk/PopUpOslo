using System.Data.SQLite;
using System.Collections.Generic;

public class EventRepository
{
    public void AddEvent(Event ev)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Events 
        (Title, Description, Category, Type, EventDate, Venue, OrganizerId, Status)
        VALUES (@t,@d,@c,@ty,@dt,@v,@o,@s)";

        cmd.Parameters.AddWithValue("@t", ev.Title);
        cmd.Parameters.AddWithValue("@d", ev.Description);
        cmd.Parameters.AddWithValue("@c", ev.Category);
        cmd.Parameters.AddWithValue("@ty", ev.Type);
        cmd.Parameters.AddWithValue("@dt", ev.EventDate);
        cmd.Parameters.AddWithValue("@v", ev.Venue);
        cmd.Parameters.AddWithValue("@o", ev.OrganizerId);
        cmd.Parameters.AddWithValue("@s", ev.Status);

        cmd.ExecuteNonQuery();
    }

    public List<Event> GetAllEvents()
    {
        var list = new List<Event>();

        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Events";

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Event
            {
                EventId = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.GetString(2),
                Category = reader.GetString(3),
                Type = reader.GetString(4),
                EventDate = reader.GetString(5),
                Venue = reader.GetString(6),
                OrganizerId = reader.GetInt32(7),
                Status = reader.GetString(8)
            });
        }

        return list;
    }

    public Event GetEventById(int id)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Events WHERE EventId = @id";
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Event
            {
                EventId = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.GetString(2),
                Category = reader.GetString(3),
                Type = reader.GetString(4),
                EventDate = reader.GetString(5),
                Venue = reader.GetString(6),
                OrganizerId = reader.GetInt32(7),
                Status = reader.GetString(8)
            };
        }

        return null;
    }

    public void UpdateEvent(Event ev)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Events SET 
        Title=@t, Description=@d, Category=@c, Type=@ty,
        EventDate=@dt, Venue=@v, Status=@s
        WHERE EventId=@id";

        cmd.Parameters.AddWithValue("@t", ev.Title);
        cmd.Parameters.AddWithValue("@d", ev.Description);
        cmd.Parameters.AddWithValue("@c", ev.Category);
        cmd.Parameters.AddWithValue("@ty", ev.Type);
        cmd.Parameters.AddWithValue("@dt", ev.EventDate);
        cmd.Parameters.AddWithValue("@v", ev.Venue);
        cmd.Parameters.AddWithValue("@s", ev.Status);
        cmd.Parameters.AddWithValue("@id", ev.EventId);

        cmd.ExecuteNonQuery();
    }

    public void CancelEvent(int eventId)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Events SET Status='Cancelled' WHERE EventId=@id";
        cmd.Parameters.AddWithValue("@id", eventId);

        cmd.ExecuteNonQuery();
    }
}