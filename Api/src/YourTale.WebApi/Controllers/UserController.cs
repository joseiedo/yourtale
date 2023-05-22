using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Security;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests.Post;
using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.Core;

namespace WebApplication1.Controllers;

[Route("v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IFriendRequestService _friendRequestService;
    private readonly TokenService _tokenService;
    private readonly IUserService _userService;

    public UserController(IUserService userService, TokenService tokenService,
        IFriendRequestService friendRequestService)
    {
        _userService = userService;
        _tokenService = tokenService;
        _friendRequestService = friendRequestService;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var loginResponse = await _userService.ValidateLogin(request);

        if (!loginResponse.IsAuthenticated)
            return BadRequest(new ErrorResponse(new Notification("Usuário e/ou senha inválidos")));

        var token = _tokenService.GenerateToken(loginResponse);

        return Ok(new
        {
            user = loginResponse,
            token
        });
    }
    
    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(int id)
    {
        var response = await _userService.GetUserById(id);

        if (!response.IsValid())
            return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response);
    }

    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        var response = await _userService.RegisterUser(request);

        if (!response.IsValid())
            return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response);
    }
    


    [HttpGet]
    [Route("me")]
    [Authorize]
    public IActionResult GetAuthenticatedUserDetails()
    {
        var response = _userService.GetAuthenticatedUserDetails();

        return Ok(response);
    }

    [HttpPut]
    [Route("me")]
    [Authorize]
    public async Task<IActionResult> EditUser([FromBody] UserEditRequest request)
    {
        var response = await _userService.EditUser(request);

        if (!response.IsValid())
            return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response.User);
    }
    
    [HttpPost]
    [Route("friend-requests/{friendId:int}")]
    [Authorize]
    public async Task<IActionResult> AddFriend(int friendId)
    {
        var response = await _friendRequestService.AddFriend(friendId);

        if (!response.IsValid())
            return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response.FriendRequest);
    }

    [HttpPut]
    [Route("friend-requests/{friendRequestId:int}")]
    [Authorize]
    public async Task<IActionResult> AcceptFriendRequest(int friendRequestId)
    {
        var response = await _friendRequestService.AcceptFriendRequest(friendRequestId);

        if (!response.IsValid())
            return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response.FriendRequest);
    }

    [HttpPut]
    [Route("friend-requests/{friendRequestId:int}/decline")]
    [Authorize]
    public IActionResult RejectFriendRequest(int friendRequestId)
    {
        var response =  _friendRequestService.DeclineFriendRequest(friendRequestId);

        if (!response.IsValid())
            return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response.FriendRequest);
    }
    
    [HttpDelete]
    [Route("friend-requests/{friendshipId:int}/remove")]
    [Authorize]
    public async Task<IActionResult> RemoveFriend(int friendshipId)
    {
        var response = await _friendRequestService.RemoveFriend(friendshipId);

        if (!response.IsValid())
            return BadRequest(new ErrorResponse(response.Notifications));

        return NoContent();
    }
    
    [HttpGet]
    [Route("friend-requests")]
    [Authorize]
    public async Task<IActionResult> GetFriendRequests()
    {
        var response = await _friendRequestService.GetFriendRequests();

        return Ok(response);
    }
    
    [HttpGet]
    [Route("friends")]
    [Authorize]
    public async Task<IActionResult> GetFriendsByNameOrEmailEquals(
        [FromQuery] string text = "", 
        [FromQuery] int page = 1, 
        [FromQuery] int take = 6)
    {
        var response = await _friendRequestService.GetFriendsByNameOrEmailEquals(text, page, take);

        return Ok(response);
    }
    
      
    [HttpGet]
    [Route("search/")]
    [Authorize]
    public async Task<IActionResult> GetUsersByNameOrEmailEquals(
        [FromQuery] string text = "", 
        [FromQuery] int page = 1, 
        [FromQuery] int take = 6)
    {
        var response = await _userService.GetUsersByNameOrEmailEquals(text,page,take);

        return Ok(response);
    }
   
}