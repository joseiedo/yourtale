namespace YourTale.Domain.Models;

public class User
{
    public int Id { get; }
    public string FullName { get; }
    public string Email { get; }
    public string NickName { get; }
    public DateTime BirthDate { get; }
    public string Cep { get; }
    public string Password { get; set; }
    public string Picture { get; }
    public string Role { get; set; }
    public virtual IList<Post> Posts { get; set; }

    public virtual IList<FriendRequest> FriendRequestsReceived { get; set; }

    public virtual IList<FriendRequest> FriendRequestsSent { get; set; }
    public virtual IList<Like> Likes { get; set; }
    public virtual IList<Comment> Comments { get; set; }
}