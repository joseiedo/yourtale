using Bogus;
using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.Post;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Domain.Models;

namespace YourTale.UnitTests.Fakers;

public class PostFakers
{
    public readonly Faker<Post> Post = new Faker<Post>()
            .RuleFor(x => x.Id, f => f.Random.Int())
            .RuleFor(x => x.Likes, new List<Like>())
            .RuleFor(x => x.Comments, new List<Comment>())
        ;
    
    public readonly Faker<CreatePostRequest> CreatePostRequest = new Faker<CreatePostRequest>()
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.Picture, f => f.Internet.Avatar())
        ;

    public readonly Faker<PostDto> PostDto = new Faker<PostDto>()
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.Id, f => f.Random.Int())
            .RuleFor(x => x.Picture, f => f.Internet.Avatar())
        ;

    public readonly Faker<CommentDto> CommentDto = new Faker<CommentDto>()
            .RuleFor(x => x.Description, f => f.Lorem.Sentence())
            .RuleFor(x => x.Id, f => f.Random.Int())
        ;
}