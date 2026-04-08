namespace PopUpOslo.Domain.Entities;

public class Review
{
    public int ReviewId { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
}