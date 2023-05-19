using YourTale.Application.Contracts.Documents.Responses.FriendRequest;

namespace YourTale.Application.Contracts;

public interface IFriendRequestService
{
    Task<AddFriendResponse> AddFriend(int friendId);

    Task<AcceptFriendResponse> AcceptFriendRequest(int friendRequestId);

    void DeclineFriendRequest(int friendRequestId);

    Task<List<FriendRequestDto>> GetFriendRequests();
}