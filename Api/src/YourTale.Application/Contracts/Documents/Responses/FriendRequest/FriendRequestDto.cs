using YourTale.Application.Contracts.Documents.Responses.User;

namespace YourTale.Application.Contracts.Documents.Responses.FriendRequest;

public class FriendRequestDto
{
    public int Id { get; set; }
    public UserDto User { get; set; }
    public DateTime CreatedAt { get; set; }
}