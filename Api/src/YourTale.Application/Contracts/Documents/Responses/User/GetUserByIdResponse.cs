using YourTale.Application.Contracts.Documents.Responses.Core;
using static System.String;

namespace YourTale.Application.Contracts.Documents.Responses.User;

public class GetUserByIdResponse : Notifiable
{
    public UserDto? User { get; set; }

    public string Uf { get; set; } = Empty;

    public string City { get; set; } = Empty;

    public bool IsFriend { get; set; }
    public bool IsLoggedUser { get; set; }
    public bool FriendRequestPending { get; set; }
    public bool FriendRequestReceived { get; set; }
    public int FriendshipId { get; set; }
}