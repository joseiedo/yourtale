using YourTale.Application.Contracts.Documents.Responses.FriendRequest;

namespace YourTale.Application.Contracts;

public interface IFriendRequestService
{
   
    Task<AddFriendResponse> AddFriend(int friendId);
    
    void AcceptFriendRequest(int friendRequestId);
    
    void DeclineFriendRequest(int friendRequestId);

    Task<List<FriendRequestDto>> GetFriendRequests();
}