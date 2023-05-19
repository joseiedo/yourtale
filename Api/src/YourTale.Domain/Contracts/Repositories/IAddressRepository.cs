using YourTale.Domain.Models;

namespace YourTale.Domain.Contracts.Repositories;

public interface IAddressRepository
{
    Task<Address?> ConsultCep(string cep);
}