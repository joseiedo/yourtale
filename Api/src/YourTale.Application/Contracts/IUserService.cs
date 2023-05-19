using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.User;
using YourTale.Domain.Models;

namespace YourTale.Application.Contracts;

public interface IUserService
{
    Task<UserLoginResponse> ValidateLogin(UserLoginRequest request);
    Task<UserRegisterResponse> RegisterUser(UserRegisterRequest request);
    UserDto GetAuthenticatedUserDetails();
    User GetAuthenticatedUser();
    Task<UserEditResponse> EditUser(UserEditRequest request);
    
    Task<GetUserByIdResponse> GetUserById(int id); 
}