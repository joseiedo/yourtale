using AutoMapper;
using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents;
using YourTale.Application.Contracts.Documents.Requests;
using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Application.Helpers;
using YourTale.Domain.Contracts.Repositories;
using YourTale.Domain.Models;

namespace YourTale.Application.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public UserService(IMapper mapper, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    } 
    public async Task<UserLoginResponse> ValidateLogin(UserLoginRequest request)
    {
        var user = await _userRepository.GetUser(request.Email, request.Password);
        
        if (user == null)
            return new UserLoginResponse();
        
        return new UserLoginResponse
            { IsAuthenticated = true, Email = user.Email, Id = user.Id, Role = user.Role };
    }

    public async Task<User> RegisterUser(UserRegisterRequest request)
    {

        if (await _userRepository.ExistsByEmail(request.Email))
        {
            // return null;
        }
        
        
        var user = _mapper.Map<User>(request);
        user.Password = Hash.Md5Hash(request.Password);
        user.Role = "USER";
        
        return await _userRepository.Add(user);
    }
    
}