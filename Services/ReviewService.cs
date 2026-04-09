using PopUpOslo.Domain.Entities;

namespace PopUpOslo.Services;

public class ReviewService
{
    private readonly List<Review> _reviews = new();
    private int _nextReviewId = 1;

    public bool AddReview(int userId, int eventId, int rating, string comment)
    {
        bool alreadyReviewed = _reviews.Any(r =>
            r.UserId == userId &&
            r.EventId == eventId);

        if (alreadyReviewed)
        {
            return false;
        }

        var review = new Review
        {
            ReviewId = _nextReviewId++,
            UserId = userId,
            EventId = eventId,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.Now.ToString("g")
        };

        _reviews.Add(review);
        return true;
    }

    public List<Review> GetReviewsByEvent(int eventId)
    {
        return _reviews
            .Where(r => r.EventId == eventId)
            .OrderByDescending(r => r.ReviewId)
            .ToList();
    }

    public bool HasUserReviewed(int userId, int eventId)
    {
        return _reviews.Any(r =>
            r.UserId == userId &&
            r.EventId == eventId);
    }

    public double GetAverageRating(int eventId)
    {
        var eventReviews = _reviews.Where(r => r.EventId == eventId).ToList();

        if (eventReviews.Count == 0)
        {
            return 0;
        }

        return eventReviews.Average(r => r.Rating);
    }
}