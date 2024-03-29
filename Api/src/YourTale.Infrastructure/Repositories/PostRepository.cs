using Microsoft.EntityFrameworkCore;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;
using YourTale.Infrastructure.Data;

namespace YourTale.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly YourTaleContext _context;
    private readonly DbSet<Post> _posts;

    public PostRepository(YourTaleContext context)
    {
        _context = context;
        _posts = context.Posts;
    }

    public async Task<Post> Add(Post post)
    {
        _posts.Add(post);
        await _context.SaveChangesAsync();

        return post;
    }


    public Task<List<Post>> GetPosts(List<User?> friends,
        int userId,
        int page = 1,
        int take = 6)
    {
        IQueryable<Post> posts;

        if (!friends.Any())
            posts = _posts
                .Where(x => x.Author.Id == userId);
        else
            posts = _posts
                .Where(x => x.Author.Id == userId || friends.Contains(x.Author));


        return posts
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * take)
            .Take(take)
            .ToListAsync();
    }

    public Task<List<Post>> GetPostsByUserId(bool isFriendOrCurrentUser, int userId, int page = 1, int take = 6)
    {
        return _posts
            .Where(x => x.Author.Id == userId)
            .Where(x => x.IsPrivate == false || isFriendOrCurrentUser)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * take)
            .Take(take)
            .ToListAsync();
    }

    public Post? GetById(int postId)
    {
        return _posts.FirstOrDefault(x => x.Id == postId);
    }

    public async Task SaveAll()
    {
        await _context.SaveChangesAsync();
    }
}