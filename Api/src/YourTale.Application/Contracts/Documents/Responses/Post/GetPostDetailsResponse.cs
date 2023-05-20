using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Core;

namespace YourTale.Application.Contracts.Documents.Responses.Post;

public class GetPostDetailsResponse : Notifiable
{
    public PostDto? Post { get; set; }
    public List<CommentDto>? Comments { get; set; }
    public int LikesQuantity { get; set; }
}