using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.Post;

namespace YourTale.Application.Contracts;

public interface IPostService
{
    Task<CreatePostResponse> CreatePost(CreatePostRequest request);

    Task<Pageable<PostDto>> GetPosts(int page = 1, int take = 6);
}