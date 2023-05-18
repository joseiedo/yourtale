using YourTale.Domain.Models;

namespace YourTale.Domain.Contracts.Repositories;

public interface IPostRepository
{
    Task<Post> Add(Post post);
}