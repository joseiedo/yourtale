using YourTale.Application.Contracts.Documents.Requests.User;
using YourTale.Application.Contracts.Documents.Responses.User;

namespace YourTale.Application.Contracts;

public interface IUserService
{
    Task<UserLoginResponse> ValidateLogin(UserLoginRequest request);
    Task<UserRegisterResponse> RegisterUser(UserRegisterRequest request);
    Task<UserDto> GetUserById(int id);
}