using Microsoft.Data.Sqlite;
using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;

namespace PopUpOslo.Infrastructure.Repositories;

public class EventRepository : BaseRepository
{
    // =========================
    // ADD EVENT
    // =========================
    public int AddEvent(Event ev)
    {
        using var conn = GetOpenConnection();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Events
            (Title, Description, Category, Type, DateTime, Venue, OrganizerId, Status)
            VALUES
            (@t, @d, @c, @ty, @dt, @v, @o, @s);

            SELECT last_insert_rowid();
        ";

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

    // =========================
    // GET ALL EVENTS
    // =========================
    public List<Event> GetAllEvents()
    {

        var list = new List<Event>();

        using var conn = GetOpenConnection();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Events ORDER BY EventID;";

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {

            list.Add(MapEvent(reader));
        }
Console.WriteLine("EVENT: " + list);
        return list;
    }

    // =========================
    // GET EVENTS BY ORGANIZER
    // =========================
    public List<Event> GetEventsByOrganizer(int organizerId)
    {
        var list = new List<Event>();

        using var conn = GetOpenConnection();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT * FROM Events
            WHERE OrganizerId = @id
            ORDER BY DateTime;
        ";

        cmd.Parameters.AddWithValue("@id", organizerId);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapEvent(reader));
        }

        return list;
    }

    // =========================
    // GET BY ID
    // =========================
    public Event? GetEventById(int eventId)
    {
        using var conn = GetOpenConnection();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Events WHERE EventId = @id;";
        cmd.Parameters.AddWithValue("@id", eventId);

        using var reader = cmd.ExecuteReader();

        return reader.Read() ? MapEvent(reader) : null;
    }

    // =========================
    // UPDATE EVENT
    // =========================
    public void UpdateEvent(Event ev)
    {
        using var conn = GetOpenConnection();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE Events
            SET Title = @t,
                Description = @d,
                Venue = @v,
                DateTime = @dt,
                Category = @c,
                Type = @ty,
                Status = @s
            WHERE EventId = @id;
        ";

        cmd.Parameters.AddWithValue("@t", ev.Title);
        cmd.Parameters.AddWithValue("@d", ev.Description);
        cmd.Parameters.AddWithValue("@v", ev.Venue);
        cmd.Parameters.AddWithValue("@dt", ev.DateTime.ToString("s"));
        cmd.Parameters.AddWithValue("@c", ev.Category.ToString());
        cmd.Parameters.AddWithValue("@ty", ev.Type.ToString());
        cmd.Parameters.AddWithValue("@s", ev.Status.ToString());
        cmd.Parameters.AddWithValue("@id", ev.EventId);

        cmd.ExecuteNonQuery();
    }

    // =========================
    // CANCEL EVENT
    // =========================
    public void CancelEvent(int eventId)
    {
        using var conn = GetOpenConnection();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE Events
            SET Status = 'Cancelled'
            WHERE EventId = @id;
        ";

        cmd.Parameters.AddWithValue("@id", eventId);
        cmd.ExecuteNonQuery();
    }

    // =========================
    // SEARCH EVENTS (FIXED SQL)
    // =========================
    public List<Event> SearchEvents(string keyword)
    {
        var list = new List<Event>();

        using var conn = GetOpenConnection();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT * FROM Events
            WHERE Title LIKE @k
               OR Description LIKE @k
               OR Venue LIKE @k
            ORDER BY DateTime;
        ";

        cmd.Parameters.AddWithValue("@k", $"%{keyword}%");

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapEvent(reader));
        }

        return list;
    }

    // =========================
    // MAP EVENT (FIXED SAFE VERSION)
    // =========================
    private Event MapEvent(SqliteDataReader reader)
{
    return new Event
    {
        EventId = Convert.ToInt32(reader["EventId"]),
        Title = reader["Title"]?.ToString() ?? "",
        Description = reader["Description"]?.ToString() ?? "",
        Venue = reader["Venue"]?.ToString() ?? "",

        OrganizerId = Convert.ToInt32(reader["OrganizerId"]),

        Category = ParseEnumSafe<EventCategory>(reader["Category"], EventCategory.Other),
        Type = ParseEnumSafe<EventType>(reader["Type"], EventType.Workshop),
        Status = ParseEnumSafe<EventStatus>(reader["Status"], EventStatus.Upcoming),

        DateTime = DateTime.TryParse(reader["DateTime"]?.ToString(), out var dt)
            ? dt
            : DateTime.Now
    };
}
private static T ParseEnumSafe<T>(object value, T defaultValue) where T : struct
{
    if (value == null)
        return defaultValue;

    if (Enum.TryParse(value.ToString(), true, out T result))
        return result;

    return defaultValue;
}
}