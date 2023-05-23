using YourTale.Domain.Models;

namespace YourTale.Domain.Contracts.Repositories;

public interface ILikeRepository
{
    bool IsLiked(int userId, int postId);

    Task<Like> Add(Like like);

    Task RemoveById(int userId, int postId);
}