namespace YourTale.Application.Contracts.Documents.Requests.User;

public class UserRegisterRequest
{
    public string Fullname { get; set; } 
    
    public string Email { get; set; }
    
    public string Nickname { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public string Cep { get; set; }
    
    public string Password { get; set; }
    
    public string ConfirmPassword { get; set; }
    
    public string Picture { get; set; }
}