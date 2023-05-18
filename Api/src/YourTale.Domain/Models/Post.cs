namespace YourTale.Domain.Models;

public class Post
{
    

    public int Id { get;  set; } 
    
    public virtual User Author { get; set; }
    
    public string Description { get;  set; }
    
    public string Picture { get;  set; }
    
    public DateTime CreatedAt { get;  set; }
    
    public bool IsPrivate { get; set; }
    
    public List<Comment> Comments { get; set; }
    public IList<Like> Likes { get; set; }
}