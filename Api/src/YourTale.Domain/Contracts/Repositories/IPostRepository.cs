using YourTale.Domain.Models;

namespace YourTale.Domain.Contracts.Repositories;

public interface IPostRepository
{
    Task<Post> Add(Post post);
    Task<List<Post>> GetPosts(List<User?> friends, int userId, int page = 1, int take = 6);

}