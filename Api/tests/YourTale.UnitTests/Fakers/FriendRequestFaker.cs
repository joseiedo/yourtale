using Bogus;
using YourTale.Application.Contracts.Documents.Responses.FriendRequest;
using YourTale.Domain.Models;

namespace YourTale.UnitTests.Fakers;

public class FriendRequestFaker
{
    public readonly Faker<FriendRequest> FriendRequest = new Faker<FriendRequest>()
            .RuleFor(x => x.CreatedAt, DateTime.Now)
            .RuleFor(x => x.Id, f => f.Random.Int())
            .RuleFor(x => x.UserId, f => f.Random.Int())
            .RuleFor(x => x.FriendId, f => f.Random.Int())
        ;
    
   public readonly Faker<FriendRequestDto> FriendRequestDto = new Faker<FriendRequestDto>()
           .RuleFor(x => x.Id, f => f.Random.Int())
           .RuleFor(x => x.User, f => new UserFakers().UserDto.Generate()) 
           .RuleFor(x => x.CreatedAt, DateTime.Now)
       ;
}