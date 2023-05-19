using YourTale.Domain.Models;

namespace YourTale.Domain.Contracts.Repositories;

public interface ILikeRepository 
{
    bool IsLiked(int userId, int postId);
    
    Task<Like> Add(Like like);

    void RemoveById(int userId, int postId);
}