using System.Text.Json;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;
using YourTale.Infrastructure.Repositories.Documents;

namespace YourTale.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly HttpClient _httpClient;

    public AddressRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Address?> ConsultCep(string cep)
    {
        var requestUri = $"/ws/{cep}/json";

        try
        {
            using var response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
                return null;


            var content = await response.Content.ReadAsStringAsync();

            var cepResponse = JsonSerializer.Deserialize<CepConsultResponse>(content);

            if (cepResponse is null) return null;

            return new Address
            {
                City = cepResponse.Localidade,
                Uf = cepResponse.Uf
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro na consulta de CEPs: {e}");
            return null;
        }
    }
}