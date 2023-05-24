using YourTale.Application.Contracts.Documents.Responses.User;

namespace YourTale.Application.Contracts.Documents.Responses.Post;

public class PostDto
{
    public int Id { get; set; }

    public string Description { get; set; }

    public string Picture { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsPrivate { get; set; }

    public UserDto Author { get; set; }

    public bool IsLiked { get; set; }
}