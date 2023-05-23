using AutoMapper;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.Post;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;

namespace YourTale.Application.Implementations;

public class PostService : IPostService
{
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly IMapper _mapper;
    private readonly IPostRepository _postRepository;
    private readonly IUserService _userService;

    public PostService(IMapper mapper,
        IPostRepository postRepository,
        IFriendRequestRepository friendRequestRepository,
        ILikeRepository likeRepository,
        IUserService userService
    )
    {
        _postRepository = postRepository;
        _likeRepository = likeRepository;
        _userService = userService;
        _friendRequestRepository = friendRequestRepository;
        _mapper = mapper;
    }


    public async Task<CreatePostResponse> CreatePost(CreatePostRequest request)
    {
        var response = new CreatePostResponse();
        var author = _userService.GetAuthenticatedUser();


        var post = _mapper.Map<Post>(request);
        post.Author = author;

        var entity = await _postRepository.Add(post);
        response.Post = _mapper.Map<PostDto>(entity);

        return response;
    }

    public GetPostDetailsResponse GetPostDetails(int postId)
    {
        var response = new GetPostDetailsResponse();
        var post = _postRepository.GetById(postId);

        if (post is null)
        {
            response.AddNotification(new Notification("Post inválido"));
            return response;
        }

        response.Post = _mapper.Map<PostDto>(post);
        response.Comments = _mapper.Map<List<CommentDto>>(post.Comments);
        response.LikesQuantity = post.Likes.Count;
        response.Post.IsLiked = _likeRepository.IsLiked(_userService.GetAuthenticatedUser().Id, postId);

        return response;
    }

    public async Task<EditPostResponse> EditPost(EditPostRequest request)
    {
        var response = new EditPostResponse();

        var post = _postRepository.GetById(request.PostId);

        if (post is null)
        {
            response.AddNotification(new Notification("Post inválido"));
            return response;
        }

        post.IsPrivate = request.IsPrivate;

        await _postRepository.SaveAll();

        response.Post = _mapper.Map<PostDto>(post);

        return response;
    }

    public async Task CommentPost(CommentPostRequest request)
    {
        var post = _postRepository.GetById(request.PostId);

        if (post is null) return;

        var user = _userService.GetAuthenticatedUser();

        var comment = new Comment
        {
            Description = request.Text,
            CreatedAt = DateTime.Now,
            Post = post,
            User = user
        };

        post.Comments.Add(comment);
        await _postRepository.SaveAll();
    }


    public async Task<LikePostResponse> LikePost(int postId)
    {
        var post = _postRepository.GetById(postId);
        var user = _userService.GetAuthenticatedUser();
        var response = new LikePostResponse();

        if (post is null)
        {
            response.AddNotification(new Notification("Post inválido"));
            return response;
        }

        if (_likeRepository.IsLiked(user.Id, postId))
        {
            response.AddNotification(new Notification("Post já curtido"));
            return response;
        }

        var likeEntity = new Like
        {
            CreatedAt = DateTime.Now,
            Post = post,
            User = user
        };

        await _likeRepository.Add(likeEntity);

        response.Post = _mapper.Map<PostDto>(post);
        response.Post.IsLiked = true;

        return response;
    }

    public async Task<UnlikePostResponse> UnlikePost(int postId)
    {
        var post = _postRepository.GetById(postId);
        var user = _userService.GetAuthenticatedUser();
        var response = new UnlikePostResponse();

        if (post is null)
        {
            response.AddNotification(new Notification("Post inválido"));
            return response;
        }

        if (!_likeRepository.IsLiked(user.Id, postId))
        {
            response.AddNotification(new Notification("Post já não foi curtido"));
            return response;
        }

        await _likeRepository.RemoveById(user.Id, postId);

        response.Post = _mapper.Map<PostDto>(post);
        response.Post.IsLiked = true;

        return response;
    }


    public async Task<Pageable<PostDto>> GetPosts(int page, int take)
    {
        var user = _userService.GetAuthenticatedUser();
        var friends = _friendRequestRepository.GetFriends(user.Id);

        var posts = await _postRepository.GetPosts(friends, user.Id, page, take);


        return new Pageable<PostDto>
        {
            Content = ListPostsDto(posts, user.Id),
            Page = page,
            IsLastPage = posts.Count < take
        };
    }

    public async Task<Pageable<PostDto>> GetPostsByUserId(int userId, int page, int take)
    {
        var user = _userService.GetAuthenticatedUser();
        var friends = _friendRequestRepository.GetFriends(user.Id);

        var isFriendOrCurrentUser = userId == user.Id || friends.Any(f => f != null && f.Id == userId);


        var posts = await _postRepository.GetPostsByUserId(isFriendOrCurrentUser, userId, page, take);

        return new Pageable<PostDto>
        {
            Content = ListPostsDto(posts, userId),
            Page = page,
            IsLastPage = posts.Count < take
        };
    }

    private List<PostDto> ListPostsDto(IEnumerable<Post> posts, int userId)
    {
        return posts.Select(post =>
        {
            var postDto = _mapper.Map<PostDto>(post);
            postDto.IsLiked = _likeRepository.IsLiked(userId, post.Id);
            return postDto;
        }).ToList();
    }
}