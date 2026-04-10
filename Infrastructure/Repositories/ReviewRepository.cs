using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using PopUpOslo.Domain.Entities;

public class ReviewRepository : BaseRepository
{
    //  Add Review
    public void AddReview(Review review)
    {
        using var conn = GetOpenConnection(); // superclass

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Reviews 
        (UserId, EventId, Rating, Comment)
        VALUES (@u, @e, @r, @c)";

        cmd.Parameters.AddWithValue("@u", review.UserId);
        cmd.Parameters.AddWithValue("@e", review.EventId);
        cmd.Parameters.AddWithValue("@r", review.Rating);
        cmd.Parameters.AddWithValue("@c", review.Comment);

        cmd.ExecuteNonQuery();
    }

    // Get all reviews for an event
    public List<Review> GetReviewsByEvent(int eventId)
    {
        var list = new List<Review>();

        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Reviews WHERE EventId=@id";
        cmd.Parameters.AddWithValue("@id", eventId);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(MapReview(reader));
        }

        return list;
    }

    // Get review by user + event (important rule: one review per booking)
    public Review GetReviewByUserAndEvent(int userId, int eventId)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Reviews 
                            WHERE UserId=@u AND EventId=@e";

        cmd.Parameters.AddWithValue("@u", userId);
        cmd.Parameters.AddWithValue("@e", eventId);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return MapReview(reader);
        }
        else
        {
            throw new Exception("Review not found");
        }


    }

    // Get average rating for event
    public double GetAverageRating(int eventId)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT AVG(Rating) FROM Reviews WHERE EventId=@id";
        cmd.Parameters.AddWithValue("@id", eventId);

        var result = cmd.ExecuteScalar();

        return result == DBNull.Value ? 0 : System.Convert.ToDouble(result);
    }

    // Delete review (optional)
    public void DeleteReview(int reviewId)
    {
        using var conn = GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Reviews WHERE ReviewId=@id";
        cmd.Parameters.AddWithValue("@id", reviewId);

        cmd.ExecuteNonQuery();
    }

    // Private Mapper (clean code)
    private Review MapReview(SqliteDataReader reader)
    {
        return new Review
        {
            ReviewId = reader.GetInt32(0),
            UserId = reader.GetInt32(1),
            EventId = reader.GetInt32(2),
            Rating = reader.GetInt32(3),
            Comment = reader.IsDBNull(4) ? "" : reader.GetString(4)
        };
    }
}