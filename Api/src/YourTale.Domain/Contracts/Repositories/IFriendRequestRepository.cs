using YourTale.Domain.Models;

namespace YourTale.Domain.Contracts.Repositories;

public interface IFriendRequestRepository
{
    Task<FriendRequest> Add(FriendRequest friendRequest);
    
    List<User?> GetFriends(int userId);
}