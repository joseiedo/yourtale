namespace YourTale.Application.Contracts.Documents.Responses.User;

public class LoginTokenResponse
{
    public UserLoginResponse User { get; set; }
    public string Token { get; set; }
}