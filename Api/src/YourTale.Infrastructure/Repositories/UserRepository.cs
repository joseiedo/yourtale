using Microsoft.EntityFrameworkCore;
using YourTale.Application.Helpers;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;
using YourTale.Infrastructure.Data;

namespace YourTale.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly YourTaleContext _context;
    private readonly DbSet<User> _users;

    public UserRepository(YourTaleContext context)
    {
        _context = context;
        _users = _context.Users;
    }

    public Task<User?> GetUser(string email, string password)
    {
        return _users.FirstOrDefaultAsync(x =>
            string.Equals(x.Email, email)
            && string.Equals(x.Password, Hash.Md5Hash(password)));
    }

    public async Task<User> Add(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public Task<bool> ExistsByEmail(string email)
    {
        return _users.AnyAsync(user => user.Email.Equals(email));
    }

    public User? GetUserById(int userId)
    {
        return _users.FirstOrDefault(user => user.Id == userId);
    }

    public async Task SaveAll()
    {
        await _context.SaveChangesAsync();
    }

    public Task<List<User>> GetUsersByFullNameOrEmailEqual(int userId, string text, int page, int take)
    {
        return _users
            .Where(x => x.Id != userId)
            .Where(x => x.FullName.ToLower().Contains(text.ToLower()) || x.Email.ToLower().Contains(text.ToLower()))
            .Skip((page - 1) * take)
            .Take(take)
            .ToListAsync();
    }
}