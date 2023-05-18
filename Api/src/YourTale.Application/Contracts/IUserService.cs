using YourTale.Application.Contracts.Documents;
using YourTale.Application.Contracts.Documents.Requests;
using YourTale.Application.Contracts.Documents.Responses.User;

namespace YourTale.Application.Contracts;

public interface IUserService
{
    UserLoginResponse ValidateLogin(UserLoginRequest request);
}