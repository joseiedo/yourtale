using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;

namespace YourTale.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public User? GetUser(string email, string password)
    {
        throw new NotImplementedException();
    }
}