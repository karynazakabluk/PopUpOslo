using System.Data.SQLite;
using System.Collections.Generic;

public class ReviewRepository
{
    public void AddReview(Review review)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Reviews 
        (UserId, EventId, Rating, Comment, CreatedAt)
        VALUES (@u,@e,@r,@c,@d)";

        cmd.Parameters.AddWithValue("@u", review.UserId);
        cmd.Parameters.AddWithValue("@e", review.EventId);
        cmd.Parameters.AddWithValue("@r", review.Rating);
        cmd.Parameters.AddWithValue("@c", review.Comment);
        cmd.Parameters.AddWithValue("@d", review.CreatedAt);

        cmd.ExecuteNonQuery();
    }

    public List<Review> GetReviewsByEvent(int eventId)
    {
        var list = new List<Review>();

        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Reviews WHERE EventId=@id";
        cmd.Parameters.AddWithValue("@id", eventId);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Review
            {
                ReviewId = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                EventId = reader.GetInt32(2),
                Rating = reader.GetInt32(3),
                Comment = reader.GetString(4),
                CreatedAt = reader.GetString(5)
            });
        }

        return list;
    }

    public Review GetReviewByUserAndEvent(int userId, int eventId)
    {
        using var conn = Database.GetConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Reviews WHERE UserId=@u AND EventId=@e";
        cmd.Parameters.AddWithValue("@u", userId);
        cmd.Parameters.AddWithValue("@e", eventId);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Review
            {
                ReviewId = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                EventId = reader.GetInt32(2),
                Rating = reader.GetInt32(3),
                Comment = reader.GetString(4),
                CreatedAt = reader.GetString(5)
            };
        }

        return null;
    }
}