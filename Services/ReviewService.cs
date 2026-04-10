using PopUpOslo.Domain.Entities;
using PopUpOslo.Infrastructure.Repositories;

namespace PopUpOslo.Services;

public class ReviewService
{
    private readonly ReviewRepository _reviewRepository = new();

    public bool AddReview(int userId, int eventId, int rating, string comment)
    {
        if (rating < 1 || rating > 5)
        {
            return false;
        }

        Review? existingReview = _reviewRepository.GetReviewByUserAndEvent(userId, eventId);

        if (existingReview != null)
        {
            return false;
        }

        var review = new Review
        {
            UserId = userId,
            EventId = eventId,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.Now.ToString("g")
        };

        _reviewRepository.AddReview(review);
        return true;
    }

    public List<Review> GetReviewsByEvent(int eventId)
    {
        return _reviewRepository.GetReviewsByEvent(eventId);
    }

    public bool HasUserReviewed(int userId, int eventId)
    {
        Review? review = _reviewRepository.GetReviewByUserAndEvent(userId, eventId);
        return review != null;
    }

    public double GetAverageRating(int eventId)
    {
        return _reviewRepository.GetAverageRating(eventId);
    }
}