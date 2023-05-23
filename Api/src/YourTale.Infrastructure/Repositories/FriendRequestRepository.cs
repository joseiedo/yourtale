using Microsoft.EntityFrameworkCore;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;
using YourTale.Infrastructure.Data;

namespace YourTale.Infrastructure.Repositories;

public class FriendRequestRepository : IFriendRequestRepository
{
    private readonly YourTaleContext _context;
    private readonly DbSet<FriendRequest> _friendRequests;

    public FriendRequestRepository(YourTaleContext context)
    {
        _context = context;
        _friendRequests = context.FriendRequests;
    }

    public async Task<FriendRequest> Add(FriendRequest friendRequest)
    {
        _friendRequests.Add(friendRequest);

        await _context.SaveChangesAsync();

        return friendRequest;
    }

    public List<User?> GetFriends(int userId)
    {
        return _friendRequests
            .Where(x => x.AcceptedAt != null && (x.UserId == userId || x.FriendId == userId))
            .Select(x => x.UserId == userId ? x.Friend : x.User)
            .ToList();
    }

    public async Task<List<User?>> GetFriendsByFullNameOrEmailEqual(int userId, string text, int page, int take)
    {
        var result = _friendRequests
            .Where(x => x.AcceptedAt != null && (x.UserId == userId || x.FriendId == userId))
            .Where(x => x.UserId == userId
                ? x.Friend!.FullName.Contains(text) || x.Friend.Email.Contains(text)
                : x.User!.FullName.Contains(text) || x.User.Email.Contains(text))
            .Select(x => x.UserId == userId ? x.Friend : x.User)
            .Skip((page - 1) * take)
            .Take(take);

        return await result.ToListAsync();
    }

    public async Task<bool> IsFriendRequestPending(int authenticatedUserId, int userId)
    {
        if (authenticatedUserId == userId) return false;

        return await _friendRequests.AnyAsync(x =>
            x.UserId == authenticatedUserId && x.FriendId == userId && x.AcceptedAt == null && x.RejectedAt == null);
    }

    public Task<bool> IsFriendRequestReceived(int authenticatedUserId, int userId)
    {
        if (authenticatedUserId == userId) return Task.FromResult(false);

        return _friendRequests.AnyAsync(x =>
            x.UserId == userId && x.FriendId == authenticatedUserId && x.AcceptedAt == null && x.RejectedAt == null);
    }

    public async Task<int> GetFriendshipId(int authenticatedUserId, int friendId)
    {
        var friendRequest = await _friendRequests.SingleOrDefaultAsync(x =>
            (x.UserId == authenticatedUserId && x.FriendId == friendId) ||
            (x.UserId == friendId && x.FriendId == authenticatedUserId));


        return friendRequest?.Id ?? 0;
    }

    public async Task<FriendRequest?> GetFriendship(int authenticatedUserId, int friendId)
    {
        return await _friendRequests.SingleOrDefaultAsync(x =>
            (x.UserId == authenticatedUserId && x.FriendId == friendId) ||
            (x.UserId == friendId && x.FriendId == authenticatedUserId));
    }


    public async Task Remove(FriendRequest friendship)
    {
        _friendRequests.Remove(friendship);
        await _context.SaveChangesAsync();
    }

    public Task<bool> IsFriend(int userId, int friendId)
    {
        return _friendRequests.AnyAsync(x => (x.UserId == userId && x.FriendId == friendId) || (x.FriendId == userId &&
            x.UserId == friendId
            && x.AcceptedAt != null && x.IsAccepted == true));
    }

    public FriendRequest? GetById(int friendRequestId)
    {
        return _friendRequests.SingleOrDefault(x => x.Id == friendRequestId);
    }

    public async Task SaveAllChanges()
    {
        await _context.SaveChangesAsync();
    }

    public bool FriendRequestAlreadyExists(int userId, int friendId)
    {
        return _friendRequests.Any(x => x.UserId == userId && x.FriendId == friendId);
    }

    public Task<List<FriendRequest>> GetFriendRequests(int userId)
    {
        return _friendRequests
                .Where(x => x.FriendId == userId && x.IsAccepted == false && x.RejectedAt == null)
                .ToListAsync()
            ;
    }
}