using static System.String;

namespace YourTale.Application.Contracts.Documents.Requests.User;

public class UserRegisterRequest
{
    public string FullName { get; set; } = Empty;

    public string Email { get; set; }= Empty;

    public string Nickname { get; set; } = Empty;

    public DateTime BirthDate { get; set; }

    public string Cep { get; set; } = Empty;

    public string Password { get; set; } = Empty;

    public string ConfirmPassword { get; set; } = Empty;

    public string Picture { get; set; } = Empty;
}