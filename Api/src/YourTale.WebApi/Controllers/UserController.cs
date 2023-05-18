using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Security;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests;
using YourTale.Application.Contracts.Documents.Requests.User;

namespace WebApplication1.Controllers;

[Route("v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly IUserService _userService;

    public UserController(IUserService userService, TokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
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
        
        if(!response.IsValid())
            return BadRequest(response.Notifications);

        return Ok(response);
    }
}