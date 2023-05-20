using YourTale.Application.Contracts.Documents.Responses.User;
using static System.String;

namespace YourTale.Application.Contracts.Documents.Responses.Post;

public class CommentDto
{
      public int Id { get; set; }
       public UserDto User { get; set; }
       public string Description { get; set; } = Empty;
       public DateTime CreatedAt { get; set; }
}