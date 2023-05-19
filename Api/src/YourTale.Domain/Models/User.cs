namespace YourTale.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string NickName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Cep { get; set; }
    public string Password { get; set; }
    public string Picture { get; set; }
    public string Role { get; set; }
    public virtual IList<Post> Posts { get; set; }

    public virtual IList<FriendRequest> FriendRequestsReceived { get; set; }

    public virtual IList<FriendRequest> FriendRequestsSent { get; set; }
    public virtual IList<Like> Likes { get; set; }
    public virtual IList<Comment> Comments { get; set; }
}