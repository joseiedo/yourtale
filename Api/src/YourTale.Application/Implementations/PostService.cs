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
    private readonly IMapper _mapper;
    private readonly IPostRepository _postRepository;
    private readonly IUserService _userService;
    private readonly ILikeRepository _likeRepository;

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

    public async Task LikePost(int postId)
    {
        var post = _postRepository.GetById(postId);
        var user = _userService.GetAuthenticatedUser();
        
        if (post is null)
        {
            // TODO: return 
        }

        if (_likeRepository.IsLiked(user.Id, postId))
        {
            // TODO: return
        }

        var likeEntity = new Like
        {
            CreatedAt = DateTime.Now,
            Post = post,
            User = user
        };
        
        await _likeRepository.Add(likeEntity);
    }

    public void  UnlikePost(int postId)
    {
        
        var post = _postRepository.GetById(postId);
        var user = _userService.GetAuthenticatedUser();
        
        if (post is null)
        {
            // TODO: return 
        }

        if (_likeRepository.IsLiked(user.Id, postId))
        {
            // TODO: return
        }

        _likeRepository.RemoveById(user.Id, postId);
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
        
        
        var posts =  await _postRepository.GetPostsByUserId(isFriendOrCurrentUser, userId, page, take);

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