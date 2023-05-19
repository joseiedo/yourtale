namespace YourTale.Application.Contracts.Documents.Requests.Post;

public class EditPostRequest
{
    public int PostId { get; set; }
    public bool IsPrivate { get; set; }
}