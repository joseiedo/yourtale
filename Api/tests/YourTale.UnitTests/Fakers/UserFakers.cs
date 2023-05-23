using Bogus;
using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Domain.Models;

namespace YourTale.UnitTests.Fakers;

public class UserFakers
{
    public readonly Faker<User> User = new Faker<User>()
        .RuleFor(x => x.Id, f => f.Random.Int())
        .RuleFor(x => x.FullName, f => f.Person.FullName)
        .RuleFor(x => x.Email, f => f.Person.Email)
        .RuleFor(x => x.Password, f => f.Random.String2(8))
        .RuleFor(x => x.Role, "User")
        .RuleFor(x => x.Cep, "03873000");

    public readonly Faker<UserDto> UserDto = new Faker<UserDto>()
        .RuleFor(x => x.Id, f => f.Random.Int())
        .RuleFor(x => x.FullName, f => f.Person.FullName)
        .RuleFor(x => x.Email, f => f.Person.Email)
        .RuleFor(x => x.Cep, "03873000");

    public readonly Faker<UserEditRequest> UserEditRequest = new Faker<UserEditRequest>()
        .RuleFor(x => x.Nickname, f => f.Person.UserName);

    public readonly Faker<UserRegisterRequest> UserRegisterRequest = new Faker<UserRegisterRequest>()
        .RuleFor(x => x.Email, f => f.Person.Email)
        .RuleFor(x => x.Cep, "03873000")
        .RuleFor(x => x.FullName, f => f.Person.FullName)
        .RuleFor(x => x.Password, "Senhamuitoboa")
        .RuleFor(x => x.ConfirmPassword, "Senhamuitoboa")
        .RuleFor(x => x.Nickname, f => f.Person.UserName)
        .RuleFor(x => x.BirthDate, f => f.Person.DateOfBirth);
}