using YourTale.Application.Contracts.Documents.Responses.Core;

namespace YourTale.Application.Contracts.Documents.Responses.FriendRequest;

public class AcceptFriendResponse : Notifiable
{
    public FriendRequestDto? FriendRequest { get; set; }
}