using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Core;

namespace YourTale.Application.Contracts.Documents.Responses.Post;

public class UnlikePostResponse : Notifiable
{
    public PostDto? Post { get; set; }
}