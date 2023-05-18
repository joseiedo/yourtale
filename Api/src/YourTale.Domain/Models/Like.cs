namespace YourTale.Domain.Models;

public class Like
{
    public Like(int userId, int postId)
    {
        UserId = userId;
        PostId = postId;
        CreatedAt = DateTime.Now;
    }

    public int Id { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public int PostId { get; set; }
    public virtual Post? Post { get; set; }

    public DateTime CreatedAt { get; set; }
}