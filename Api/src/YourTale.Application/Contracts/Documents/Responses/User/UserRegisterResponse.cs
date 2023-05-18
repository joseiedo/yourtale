using YourTale.Application.Contracts.Documents.Responses.Core;

namespace YourTale.Application.Contracts.Documents.Responses.User;

public class UserRegisterResponse : Notifiable
{
    public UserDto? User { get; set; }
}