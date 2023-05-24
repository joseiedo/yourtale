using YourTale.Application.Contracts.Documents.Responses.Core;

namespace YourTale.Application.Contracts.Documents.Responses.Post;

public class CreatePostResponse : Notifiable
{
    public PostDto? Post { get; set; }
}