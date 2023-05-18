using AutoMapper;
using Microsoft.AspNetCore.Http;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.Core;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Application.Helpers;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;

namespace YourTale.Application.Implementations;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    

    public UserService(IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public User GetAuthenticatedUser()
    {
        var authenticatedUserId =
            _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
        
        return _userRepository.GetUserById(int.Parse(authenticatedUserId))!;
    }
    


    public async Task<UserLoginResponse> ValidateLogin(UserLoginRequest request)
    {
        var user = await _userRepository.GetUser(request.Email, request.Password);

        if (user == null)
            return new UserLoginResponse();

        return new UserLoginResponse
            { IsAuthenticated = true, Email = user.Email, Id = user.Id, Role = user.Role };
    }

    public async Task<UserRegisterResponse> RegisterUser(UserRegisterRequest request)
    {
        var response = new UserRegisterResponse();

        if (await _userRepository.ExistsByEmail(request.Email))
        {
            response.AddNotification(new Notification("Email já cadastrado"));
            return response;
        }


        var user = _mapper.Map<User>(request);
        user.Password = Hash.Md5Hash(request.Password);
        user.Role = "USER";


        var userEntity = await _userRepository.Add(user);

        response.User = _mapper.Map<UserDto>(userEntity);

        return response;
    }

    
    public Task<UserDto> GetUserById(int id)
    {
        var user = _userRepository.GetUserById(id);

        return Task.FromResult(_mapper.Map<UserDto>(user));
    }
}