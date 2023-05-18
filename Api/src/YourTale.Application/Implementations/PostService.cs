using AutoMapper;
using Microsoft.AspNetCore.Http;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.Post;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;

namespace YourTale.Application.Implementations;

public class PostService : IPostService
{
    private readonly IMapper _mapper;
    private readonly IFriendRequestRepository _friendRequestRepository; 
    private readonly IPostRepository _postRepository;
    private readonly IUserService _userService;

    public PostService(IMapper mapper, 
        IPostRepository postRepository, 
        IFriendRequestRepository friendRequestRepository,
        IUserService userService
    )
    {
        _postRepository = postRepository;
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
   
    public async Task<Pageable<PostDto>> GetPosts(int page = 1, int take = 6)
    {
        var user = _userService.GetAuthenticatedUser();
        var friends = _friendRequestRepository.GetFriends(user.Id);

        var posts = await _postRepository.GetPosts(friends, user.Id, page, take);

        return new Pageable<PostDto>
        {
            Content = _mapper.Map<List<PostDto>>(posts),
            Page = page,
            IsLastPage = posts.Count < take,
        };
    }
    
}