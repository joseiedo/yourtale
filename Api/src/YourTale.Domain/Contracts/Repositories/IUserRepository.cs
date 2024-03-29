using YourTale.Domain.Models;

namespace YourTale.Domain.Contracts.Repositories;

public interface IUserRepository
{
    Task<User?> GetUser(string email, string password);

    Task<User> Add(User user);

    Task<bool> ExistsByEmail(string email);

    User? GetUserById(int userId);
    Task SaveAll();

    Task<List<User>> GetUsersByFullNameOrEmailEqual(int userId, string text, int page, int take);
}