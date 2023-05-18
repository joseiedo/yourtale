using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
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
    
    public User? GetUser(string email, string password)
    {
        return _users.FirstOrDefault(x =>
            string.Equals(x.Email, email, StringComparison.CurrentCultureIgnoreCase)
            && string.Equals(x.Password, Hash(password)));
    }

    private static string Hash(string input)
    {
        using var md5Hash = MD5.Create();
        var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sBuilder = new StringBuilder();
        
        foreach (var t in data)
        {
            sBuilder.Append(t.ToString("x2"));
        }
        
        return sBuilder.ToString();
    }
}