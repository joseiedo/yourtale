using Microsoft.EntityFrameworkCore;
using YourTale.Domain.Models;

namespace YourTale.Infrastructure.Data;

public class YourTaleContext : DbContext
{
    public YourTaleContext(DbContextOptions<YourTaleContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<FriendRequest> FriendRequests => Set<FriendRequest>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}