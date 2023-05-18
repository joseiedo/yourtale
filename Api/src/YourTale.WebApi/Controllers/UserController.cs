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
    private readonly IUserService _userService;
    private readonly TokenService _tokenService;
   
    public UserController(IUserService userService, TokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("/login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] UserLoginRequest request)
    {
        var loginResponse = _userService.ValidateLogin(request);
       
        if (!loginResponse.IsAuthenticated)
            return BadRequest("Usuário e/ou senha inválidos");
       
        var token = _tokenService.GenerateToken(loginResponse);
         
        return Ok( new
        {
            user = loginResponse,
            token
        });
    }
    

}