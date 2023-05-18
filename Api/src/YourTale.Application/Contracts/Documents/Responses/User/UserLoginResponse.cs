namespace YourTale.Application.Contracts.Documents.Responses.User;

public class UserLoginResponse
{
    public bool IsAuthenticated { get; set; } = false;
    
    public int Id { get; set; }
    
    public string Email { get; set; }
    
    public string Role { get; set; }
    
}