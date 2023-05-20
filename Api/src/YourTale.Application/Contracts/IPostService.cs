using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.Post;

namespace YourTale.Application.Contracts;

public interface IPostService
{
    Task<CreatePostResponse> CreatePost(CreatePostRequest request);
    
    Task<LikePostResponse> LikePost(int postId);
    
    Task<UnlikePostResponse> UnlikePost(int postId);

    Task<Pageable<PostDto>> GetPosts(int page, int take);
    
    Task<Pageable<PostDto>> GetPostsByUserId(int userId, int page, int take);

    GetPostDetailsResponse GetPostDetails(int postId);

    Task<EditPostResponse> EditPost(EditPostRequest request);
    
    Task CommentPost(CommentPostRequest request);
}