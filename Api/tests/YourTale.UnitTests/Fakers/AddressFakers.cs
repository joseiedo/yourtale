using Bogus;
using YourTale.Domain.Models;

namespace YourTale.UnitTests.Fakers;

public class AddressFakers
{
    public readonly Faker<Address> Address = new Faker<Address>()
        .RuleFor(x => x.City, f => f.Address.City())
        .RuleFor(x => x.Uf, f => f.Address.StateAbbr());
}