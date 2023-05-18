namespace YourTale.Domain.Models;

public class FriendRequest
{
    public FriendRequest(int userId, int friendId, bool isAccepted, DateTime createdAt, DateTime? acceptedAt,
        DateTime? rejectedAt)
    {
        UserId = userId;
        FriendId = friendId;
        IsAccepted = isAccepted;
        CreatedAt = createdAt;
        AcceptedAt = acceptedAt;
        RejectedAt = rejectedAt;
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public virtual User? User { get; set; }
    public int FriendId { get; set; }
    public virtual User? Friend { get; set; }
    public bool IsAccepted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
}