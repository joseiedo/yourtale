using Microsoft.EntityFrameworkCore;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;
using YourTale.Infrastructure.Data;

namespace YourTale.Infrastructure.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly YourTaleContext _context;
    private readonly DbSet<Like> _likes;
    
    public LikeRepository(YourTaleContext context)
    {
        _context = context;
        _likes = context.Likes;
    }
    
    public bool IsLiked(int userId, int postId)
    {
        return _likes.Any(x => x.PostId == postId && x.UserId == userId);
    }

    public async Task<Like> Add(Like like)
    {
        _likes.Add(like);
        await _context.SaveChangesAsync();
        return like;
    }

    public async void RemoveById(int userId, int postId)
    {
        var like = await _likes.FirstOrDefaultAsync(x => x.PostId == postId && x.UserId == userId);
        if (like is null) return;
        
        _likes.Remove(like);
        await _context.SaveChangesAsync();
    }
}