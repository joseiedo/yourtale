using YourTale.Domain.Models;

namespace YourTale.Domain.Contracts.Repositories;

public interface IPostRepository
{
    Task<Post> Add(Post post);
    Task<List<Post>> GetPosts(List<User?> friends, int userId, int page = 1, int take = 6);

    public Task<List<Post>> GetPostsByUserId(bool isFriendOrCurrentUser, int userId, int page = 1, int take = 6);
    
    Post? GetById(int postId);
    Task SaveAll();
}