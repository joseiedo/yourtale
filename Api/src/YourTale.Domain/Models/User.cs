namespace YourTale.Domain.Models;

public class User
{

    public int Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string NickName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string Cep { get; private set; }
    public string Password { get; private set; }
    public string Picture { get; private set; }
    public string Role { get; private set; }
    public IList<Post> Posts { get; set; }
    
    
    public IList<FriendRequest> FriendRequestsReceived { get; set; }
    
    public IList<FriendRequest> FriendRequestsSent { get; set; }
    public IList<Like> Likes { get; set; }
    public IList<Comment> Comments { get; set; }
}