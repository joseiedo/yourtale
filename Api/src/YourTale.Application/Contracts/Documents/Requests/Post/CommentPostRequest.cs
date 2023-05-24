using static System.String;

namespace YourTale.Application.Contracts.Documents.Requests.Post;

public class CommentPostRequest
{
    public int PostId { get; set; }
    public string Text { get; set; } = Empty;
}