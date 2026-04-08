using System.Data.SQLite;
using System.Collections.Generic;

public class BookingOptionRepository
{
    // ➕ Add booking option (VIP, Standard, etc.)
    public void AddOption(BookingOption option)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO BookingOptions 
        (EventId, Name, Price, Capacity, RemainingCapacity)
        VALUES (@e, @n, @p, @c, @r)";

        cmd.Parameters.AddWithValue("@e", option.EventId);
        cmd.Parameters.AddWithValue("@n", option.Name);
        cmd.Parameters.AddWithValue("@p", option.Price);
        cmd.Parameters.AddWithValue("@c", option.Capacity);
        cmd.Parameters.AddWithValue("@r", option.RemainingCapacity);

        cmd.ExecuteNonQuery();
    }

    // 📋 Get all options for an event
    public List<BookingOption> GetOptionsByEvent(int eventId)
    {
        var list = new List<BookingOption>();

        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM BookingOptions WHERE EventId=@id";
        cmd.Parameters.AddWithValue("@id", eventId);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new BookingOption
            {
                OptionId = reader.GetInt32(0),
                EventId = reader.GetInt32(1),
                Name = reader.GetString(2),
                Price = reader.GetDouble(3),
                Capacity = reader.GetInt32(4),
                RemainingCapacity = reader.GetInt32(5)
            });
        }

        return list;
    }

    // 🔍 Get single option
    public BookingOption GetOptionById(int optionId)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM BookingOptions WHERE OptionId=@id";
        cmd.Parameters.AddWithValue("@id", optionId);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new BookingOption
            {
                OptionId = reader.GetInt32(0),
                EventId = reader.GetInt32(1),
                Name = reader.GetString(2),
                Price = reader.GetDouble(3),
                Capacity = reader.GetInt32(4),
                RemainingCapacity = reader.GetInt32(5)
            };
        }

        return null;
    }

    // ➖ Reduce capacity when booking
    public void ReduceCapacity(int optionId)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE BookingOptions 
                            SET RemainingCapacity = RemainingCapacity - 1 
                            WHERE OptionId = @id AND RemainingCapacity > 0";

        cmd.Parameters.AddWithValue("@id", optionId);

        cmd.ExecuteNonQuery();
    }

    // ➕ Restore capacity when cancelling booking
    public void IncreaseCapacity(int optionId)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE BookingOptions 
                            SET RemainingCapacity = RemainingCapacity + 1 
                            WHERE OptionId = @id";

        cmd.Parameters.AddWithValue("@id", optionId);

        cmd.ExecuteNonQuery();
    }
}