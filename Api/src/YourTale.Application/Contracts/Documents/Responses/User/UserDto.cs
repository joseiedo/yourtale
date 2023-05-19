namespace YourTale.Application.Contracts.Documents.Responses.User;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string NickName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Cep { get; set; }
    public string Picture { get; set; }
}