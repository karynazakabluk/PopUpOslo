using System.Data.SQLite;
using System.Collections.Generic;

public class BookingRepository
{
    public void AddBooking(Booking booking)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Bookings 
        (UserId, EventId, OptionId, PriceAtBooking, Status, BookingDate)
        VALUES (@u,@e,@o,@p,@s,@d)";

        cmd.Parameters.AddWithValue("@u", booking.UserId);
        cmd.Parameters.AddWithValue("@e", booking.EventId);
        cmd.Parameters.AddWithValue("@o", booking.OptionId);
        cmd.Parameters.AddWithValue("@p", booking.PriceAtBooking);
        cmd.Parameters.AddWithValue("@s", booking.Status);
        cmd.Parameters.AddWithValue("@d", booking.BookingDate);

        cmd.ExecuteNonQuery();
    }

    public List<Booking> GetBookingsByUser(int userId)
    {
        var list = new List<Booking>();

        using var conn = Database.GetConnection();
        conn.Open();

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
                PriceAtBooking = reader.GetDouble(4),
                Status = reader.GetString(5),
                BookingDate = reader.GetString(6)
            });
        }

        return list;
    }

    public void UpdateBookingStatus(int bookingId, string status)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Bookings SET Status=@s WHERE BookingId=@id";
        cmd.Parameters.AddWithValue("@s", status);
        cmd.Parameters.AddWithValue("@id", bookingId);

        cmd.ExecuteNonQuery();
    }
}