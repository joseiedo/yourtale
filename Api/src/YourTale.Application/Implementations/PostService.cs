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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public PostService(IMapper mapper, IHttpContextAccessor httpContextAccessor, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<CreatePostResponse> CreatePost(CreatePostRequest request)
    {
        var response = new CreatePostResponse();
        var authenticatedUserId =
            _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
        var author = _userRepository.GetUserById(int.Parse(authenticatedUserId));

        if (author is null)
        {
            response.AddNotification(new Notification("Usuário não encontrado."));
            return response;
        }


        var post = _mapper.Map<Post>(request);
        post.Author = author;

        var entity = await _postRepository.Add(post);
        response.Post = _mapper.Map<PostDto>(entity);

        return response;
    }
}