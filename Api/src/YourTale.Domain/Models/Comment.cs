namespace YourTale.Domain.Models;

public class Comment
{
    public Comment(int userId, int postId, string description)
    {
        UserId = userId;
        PostId = postId;
        Description = description;
        CreatedAt = DateTime.Now;
    }


    public int Id { get; set; }
    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public int PostId { get; set; }
    public virtual Post? Post { get; set; }

    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}