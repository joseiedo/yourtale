using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Core;

namespace YourTale.Application.Contracts.Documents.Responses.Post;

public class LikePostResponse : Notifiable
{
    public PostDto? Post { get; set; }
}