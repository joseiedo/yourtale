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
        var entity = _posts.Add(post).Entity;
        await _context.SaveChangesAsync();

        return entity;
    }
}