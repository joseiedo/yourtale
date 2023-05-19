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
        var entity = (await _friendRequests.AddAsync(friendRequest)).Entity;

        await _context.SaveChangesAsync();

        return entity;
    }

    public List<User?> GetFriends(int userId)
    {
        return _friendRequests
            .Where(x => x.AcceptedAt != null && (x.UserId == userId || x.FriendId == userId))
            .Select(x => x.UserId == userId ? x.Friend : x.User)
            .ToList();
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
                .Where(x => x.FriendId == userId && x.IsAccepted == false)
                .ToListAsync()
            ;
    }
}