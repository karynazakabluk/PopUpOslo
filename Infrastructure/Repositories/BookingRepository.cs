using System.Collections.Generic;
using System.Data.SQLite;

public class BookingRepository : BaseRepository
{
    //  Create booking
    public void AddBooking(Booking booking)
    {
        using var conn = GetOpenConnection(); // from superclass

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Bookings 
        (UserId, EventId, OptionId, Price, Status)
        VALUES (@u, @e, @o, @p, @s)";

        cmd.Parameters.AddWithValue("@u", booking.UserId);
        cmd.Parameters.AddWithValue("@e", booking.EventId);
        cmd.Parameters.AddWithValue("@o", booking.OptionId);
        cmd.Parameters.AddWithValue("@p", booking.Price);
        cmd.Parameters.AddWithValue("@s", booking.Status);

        cmd.ExecuteNonQuery();
    }

    //  Get bookings by user
    public List<Booking> GetBookingsByUser(int userId)
    {
        var list = new List<Booking>();

        using var conn = GetOpenConnection(); // reuse

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Bookings WHERE UserId=@id";
        cmd.Parameters.AddWithValue("@id", userId);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Booking
            {
                BookingId = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                EventId = reader.GetInt32(2),
                OptionId = reader.GetInt32(3),
                Price = reader.GetDouble(4),
                Status = reader.GetString(5)
            });
        }

        return list;
    }

    //  Cancel booking
    public void CancelBooking(int bookingId)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"UPDATE Bookings 
                            SET Status = 'Cancelled' 
                            WHERE BookingId = @id";

        cmd.Parameters.AddWithValue("@id", bookingId);

        cmd.ExecuteNonQuery();
    }

    //  Get booking by ID
    public Booking GetBookingById(int bookingId)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Bookings WHERE BookingId=@id";
        cmd.Parameters.AddWithValue("@id", bookingId);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Booking
            {
                BookingId = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                EventId = reader.GetInt32(2),
                OptionId = reader.GetInt32(3),
                Price = reader.GetDouble(4),
                Status = reader.GetString(5)
            };
        }

        return null;
    }
}