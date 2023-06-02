using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Security;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.FriendRequest;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Domain.Models;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginTokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var loginResponse = await _userService.ValidateLogin(request);

        if (!loginResponse.IsAuthenticated)
            return BadRequest(new ErrorResponse(new Notification("Usuário e/ou senha inválidos")));

        var token = _tokenService.GenerateToken(loginResponse);

        return Ok(new LoginTokenResponse
        {
            User = loginResponse,
            Token = token
        });
    }

    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserByIdResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRegisterResponse))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    public IActionResult GetAuthenticatedUserDetails()
    {
        var response = _userService.GetAuthenticatedUserDetails();

        return Ok(response);
    }

    [HttpPut]
    [Route("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FriendRequest))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public IActionResult RejectFriendRequest(int friendRequestId)
    {
        var response = _friendRequestService.DeclineFriendRequest(friendRequestId);

        if (!response.IsValid())
            return BadRequest(new ErrorResponse(response.Notifications));

        return Ok(response.FriendRequest);
    }

    [HttpDelete]
    [Route("friends/{friendshipId:int}/remove")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FriendRequestDto>))]
    public async Task<IActionResult> GetFriendRequests()
    {
        var response = await _friendRequestService.GetFriendRequests();

        return Ok(response);
    }

    [HttpGet]
    [Route("friends")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pageable<UserDto>))]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pageable<UserDto>))]
    public async Task<IActionResult> GetUsersByNameOrEmailEquals(
        [FromQuery] string text = "",
        [FromQuery] int page = 1,
        [FromQuery] int take = 6)
    {
        var response = await _userService.GetUsersByNameOrEmailEquals(text, page, take);

        return Ok(response);
    }
}