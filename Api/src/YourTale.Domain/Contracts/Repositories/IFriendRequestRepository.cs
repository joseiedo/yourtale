using YourTale.Domain.Models;

namespace YourTale.Domain.Contracts.Repositories;

public interface IFriendRequestRepository
{
    Task<FriendRequest> Add(FriendRequest friendRequest);

    List<User?> GetFriends(int userId);
    FriendRequest? GetById(int friendRequestId);
    Task SaveAllChanges();
    bool FriendRequestAlreadyExists(int userId, int friendId);
    Task<List<FriendRequest>> GetFriendRequests(int userId);

    Task<bool> IsFriend(int userId, int friendId);
    Task<List<User?>> GetFriendsByFullNameOrEmailEqual(int userId, string text, int page, int take);
}