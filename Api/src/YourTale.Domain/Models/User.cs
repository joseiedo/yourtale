namespace YourTale.Domain.Models;

public class User
{

    public int Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string NickName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string Cep { get; private set; }
    public string Password { get; set; }
    public string Picture { get; private set; }
    public string Role { get; set; }
    public virtual IList<Post> Posts { get; set; }
    
    public virtual IList<FriendRequest> FriendRequestsReceived { get; set; }
    
    public virtual IList<FriendRequest> FriendRequestsSent { get; set; }
    public virtual IList<Like> Likes { get; set; }
    public virtual IList<Comment> Comments { get; set; }
}