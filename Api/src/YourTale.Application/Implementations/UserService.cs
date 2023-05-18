using YourTale.Application.Contracts;
using YourTale.Application.Contracts.Documents;
using YourTale.Application.Contracts.Documents.Requests;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Domain.Contracts.Repositories;

namespace YourTale.Application.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    } 
    public UserLoginResponse ValidateLogin(UserLoginRequest request)
    {
        var user = _userRepository.GetUser(request.Email, request.Password);
        
        if (user == null)
            return new UserLoginResponse();
        
        return new UserLoginResponse
            { IsAuthenticated = true, Email = user.Email, Id = user.Id, Role = user.Role };
    }
}