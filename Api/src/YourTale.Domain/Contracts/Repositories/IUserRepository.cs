using YourTale.Domain.Models;

namespace YourTale.Domain.Contracts.Repositories;

public interface IUserRepository
{
   User? GetUser(string email, string password);
}