using Microsoft.Data.Sqlite;
using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;

namespace PopUpOslo.Infrastructure.Repositories;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using PopUpOslo.Domain.Entities;
using PopUpOslo.Domain.Enums;

public class BookingRepository : BaseRepository
{
    public void AddBooking(Booking booking)
    {
        using var conn = GetOpenConnection();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Bookings
            (UserId, EventId, OptionId, Price, Status, BookingDate)
            VALUES (@u, @e, @o, @p, @s, @d);";

        cmd.Parameters.AddWithValue("@u", booking.UserId);
        cmd.Parameters.AddWithValue("@e", booking.EventId);
        cmd.Parameters.AddWithValue("@o", booking.OptionId);
        cmd.Parameters.AddWithValue("@p", booking.PriceAtBooking);
        cmd.Parameters.AddWithValue("@s", booking.Status.ToString());
        cmd.Parameters.AddWithValue("@d", booking.BookingDate);

        cmd.ExecuteNonQuery();
    }

    public List<Booking> GetBookingsByUser(int userId)
    {
        var list = new List<Booking>();

        using var conn = GetOpenConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = "SELECT * FROM Bookings WHERE UserId = @id;";
        cmd.Parameters.AddWithValue("@id", userId);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapBooking(reader));
        }

        return list;
    }

    public void CancelBooking(int bookingId)
    {
        using var conn = GetOpenConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = @"
            UPDATE Bookings
            SET Status = 'Cancelled'
            WHERE BookingId = @id;";

        cmd.Parameters.AddWithValue("@id", bookingId);

        cmd.ExecuteNonQuery();
    }

    public Booking? GetBookingById(int bookingId)
    {
        using var conn = GetOpenConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText = "SELECT * FROM Bookings WHERE BookingId = @id;";
        cmd.Parameters.AddWithValue("@id", bookingId);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapBooking(reader);
        }
        else
        {
            throw new Exception("Booking not found");
        }

       
    }

    private Booking MapBooking(SqliteDataReader reader)
    {
        string statusText = reader["Status"]?.ToString() ?? "Booked";
        Enum.TryParse(statusText, out BookingStatus status);

        return new Booking
        {
            BookingId = Convert.ToInt32(reader["BookingId"]),
            UserId = Convert.ToInt32(reader["UserId"]),
            EventId = Convert.ToInt32(reader["EventId"]),
            OptionId = Convert.ToInt32(reader["OptionId"]),
            PriceAtBooking = Convert.ToDouble(reader["Price"]),
            Status = status,
            BookingDate = reader["BookingDate"]?.ToString() ?? string.Empty
        };
    }
}