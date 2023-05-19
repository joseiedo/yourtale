using YourTale.Application.Contracts.Documents.Responses.Core;

namespace YourTale.Application.Contracts.Documents.Responses.User;

public class UserEditResponse : Notifiable
{
    public UserDto? User { get; set; } 
}