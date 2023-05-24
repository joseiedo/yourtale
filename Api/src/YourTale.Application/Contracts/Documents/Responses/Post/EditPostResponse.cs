using YourTale.Application.Contracts.Documents.Responses.Core;

namespace YourTale.Application.Contracts.Documents.Responses.Post;

public class EditPostResponse : Notifiable
{
    public PostDto? Post { get; set; }
}