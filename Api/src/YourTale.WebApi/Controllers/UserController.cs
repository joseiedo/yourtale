using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Security;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Application.Implementations;

namespace WebApplication1.Controllers;

[Route("v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly TokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IFriendRequestService _friendRequestService;

    public UserController(IUserService userService, IPostService postService, TokenService tokenService, IFriendRequestService friendRequestService)
    {
        _userService = userService;
        _tokenService = tokenService;
        _postService = postService;
        _friendRequestService = friendRequestService;
    }

    [HttpPost]
    [Route("/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var loginResponse = await _userService.ValidateLogin(request);

        if (!loginResponse.IsAuthenticated)
            return BadRequest("Usuário e/ou senha inválidos");

        var token = _tokenService.GenerateToken(loginResponse);

        return Ok(new
        {
            user = loginResponse,
            token
        });
    }

    [HttpPost]
    [Route("/register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        var response = await _userService.RegisterUser(request);

        if (!response.IsValid())
            return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response);
    }


    [HttpPost]
    [Route("/post")]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] CreatePostRequest request)
    {
        var response = await _postService.CreatePost(request);

        if (!response.IsValid())
            return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response.Post);
    }


    [HttpGet]
    [Route("/me")]
    [Authorize]
    public IActionResult GetAuthenticatedUserDetails()
    {
        var response =  _userService.GetAuthenticatedUserDetails();

        return Ok(response);
    }
    
    [HttpPost]
    [Route("/friend-requests/{friendId:int}")]
    [Authorize]
    public async Task<IActionResult> AddFriend(int friendId)
    {
        var response = await _friendRequestService.AddFriend(friendId); 

        if (!response.IsValid())
            return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response.FriendRequest);
    }

    [HttpGet]
    [Route("/friend-requests")]
    [Authorize]
    public async Task<IActionResult> GetFriendRequests()
    {
        var response = await _friendRequestService.GetFriendRequests();
        
        return Ok(response);
    }
}