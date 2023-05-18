using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Post;

namespace YourTale.Application.Contracts;

public interface IPostService
{
    Task<CreatePostResponse> CreatePost(CreatePostRequest request);
}