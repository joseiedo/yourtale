using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.FriendRequest;
using YourTale.Application.Contracts.Documents.Responses.User;

namespace YourTale.Application.Contracts;

public interface IFriendRequestService
{
    Task<AddFriendResponse> AddFriend(int friendId);

    Task<AcceptFriendResponse> AcceptFriendRequest(int friendRequestId);

    void DeclineFriendRequest(int friendRequestId);

    Task<List<FriendRequestDto>> GetFriendRequests();

    Task<Pageable<UserDto>> GetFriendsByNameOrEmailEquals(string text, int page, int take);
}