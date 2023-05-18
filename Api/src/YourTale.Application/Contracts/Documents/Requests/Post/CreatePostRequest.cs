namespace YourTale.Application.Contracts.Documents.Requests.Post;

public class CreatePostRequest
{
    public string Description { get; set; }
    public string Picture { get; set; }
    public bool IsPrivate { get; set; }
}